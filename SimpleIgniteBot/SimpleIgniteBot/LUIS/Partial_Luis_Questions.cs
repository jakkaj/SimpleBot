using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBot.EditableDialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleIgniteBot.Services.Entities;

namespace SimpleIgniteBot.LUIS
{
    public partial class LuisModel
    {
        private async Task PromptForTimeslot(IDialogContext context)
        {
            try
            {
                DateTime selectedDate;
                context.UserData.TryGetValue<DateTime>("SessionFinderSelectedDate", out selectedDate);

                List<string> choices = new List<string>() { "Morning", "Afternoon" };

                EditablePromptDialog.Choice(context,
                                    TimeSlotChoiceAsync,
                                    choices,
                                    "Which time?",
                                    "I didn't understand that. Please choose one of the options",
                                    2);
            }
            catch (Exception e)
            {
                _telemetry.TrackTrace("PromptForTimeslot exception thrown");
                _telemetry.TrackException(e);

                // Cleanup context data to ensure next time round we don't remember anything
                await RemoveSessionFinderContextData(context);

                await context.PostAsync("An error ocurred whilst trying to ascertain the time you were interested in, please try again later.");
                context.Wait(MessageReceived);
            }
        }

        public async Task TimeSlotChoiceAsync(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                _telemetry.TrackTrace("TimeSlotChoiceAsync invoked");

                string choice = await result;

                context.UserData.SetValue<string>("SessionFinderSelectedTime", choice);

                // Where now
                await WhereNext(context);
            }
            catch (TooManyAttemptsException tme)
            {
                // If the user doesn't click or type one of the entries we stop the experience
                _telemetry.TrackTrace("TooManyAttemptsException exception thrown");
                _telemetry.TrackException(tme);

                // Cleanup context data to ensure next time round we don't remember anything
                await RemoveSessionFinderContextData(context);

                await context.PostAsync("Sorry, I wasn't able to understand your response. Please try asking for session information again.");
                context.Wait(MessageReceived);
            }
            catch (Exception e)
            {
                _telemetry.TrackTrace("TimeSlotChoiceAsync exception thrown");
                _telemetry.TrackException(e);

                // Cleanup context data to ensure next time round we don't remember anything
                await RemoveSessionFinderContextData(context);

                await context.PostAsync("An error ocurred within TimeSlotChoiceAsync, please try again later.");
                context.Wait(MessageReceived);
            }
        }

        private async Task ShowSessionFinderResults(IDialogContext context)
        {
            try
            {
                string selectedTime;
                context.UserData.TryGetValue<string>("SessionFinderSelectedTime", out selectedTime);

                // Cleanup context data to ensure next time round we don't remember anything
                await RemoveSessionFinderContextData(context);


                var matchingSessions = await _backEndService.GetSessionByTime(selectedTime);


                _telemetry.TrackTrace($"TimeSlotChoiceAsync::{Enumerable.Count<Session>(matchingSessions)} sessions found in the selected timeslot/track");

                if (Enumerable.Any<Session>(matchingSessions))
                {
                    matchingSessions = Enumerable.Take<Session>(matchingSessions, 8).ToList();
                    await context.PostAsync($"{Enumerable.Count<Session>(matchingSessions)} sessions found in the {selectedTime}");
                    Activity replyActivity = await CreateSessionCardReply();

                    foreach (var session in matchingSessions)
                    {
                        string room;

                        if (session.Room == null) { room = "Room TBC"; } else { room = session.Room; }

                        AddSessionCardToReply(replyActivity,
                              session,
                                true);
                    }

                    await context.PostAsync(replyActivity);
                }
                else
                {
                    string responseMessage = $"Looks like there isn't any other sessions in that timeslot ({selectedTime})";
                    await context.PostAsync(responseMessage);
                }

                context.Wait(MessageReceived);
            }
            catch (Exception e)
            {
                _telemetry.TrackTrace("TrackChoiceAsync exception thrown");
                _telemetry.TrackException(e);

                // Cleanup context data to ensure next time round we don't remember anything
                await RemoveSessionFinderContextData(context);

                await context.PostAsync("An error ocurred within TrackChoiceAsync, please try again later.");
                context.Wait(MessageReceived);
            }
        }

        private async Task WhereNext(IDialogContext context)
        {
          
            if (!await HaveTime(context))
            {
                // We don't have morning/afternoon
                await PromptForTimeslot(context);
            }
            else
            {
                // We have everything
                await ShowSessionFinderResults(context);
            }
        }

        private async Task<bool> HaveTime(IDialogContext context)
        {
            object o;
            if (context.UserData.TryGetValue<object>("SessionFinderSelectedTime", out o))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task RemoveSessionFinderContextData(IDialogContext context)
        {
            context.UserData.RemoveValue("SessionFinderSelectedDate");
            context.UserData.RemoveValue("SessionFinderSelectedTime");
            context.UserData.RemoveValue("SessionFinderSelectedTopic");
        }
    }
}