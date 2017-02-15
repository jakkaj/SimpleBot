using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace SimpleIgniteBot.LUIS
{
    public partial class LuisModel : LuisDialog<object>
    {
        public async Task LanuageSelectionChoicesAsync(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                _telemetry.TrackTrace("LanuageSelectionChoicesAsync invoked");

                string choice = await result;

                if (choice.ToLower() == "no")
                {
                    await context.PostAsync("No troubles, ignore me.");
                }
                else
                {
                    string checkLanguage;
                    context.UserData.TryGetValue<string>("checkLanguage", out checkLanguage);
                    if (string.IsNullOrWhiteSpace(checkLanguage))
                    {
                        await context.PostAsync("Something went wrong and I could not detect the language.");
                    }
                    else
                    {
                        _translatorService.SetLanguage(context, checkLanguage);
                        await context.PostAsync("No problem - it has been set!");
                    }
                }
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
    }
}