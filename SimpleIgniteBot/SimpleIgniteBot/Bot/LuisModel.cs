using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ApplicationInsights;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using SimpleIgniteBot.Services;

namespace SimpleIgniteBot.Bot
{
    [Serializable]
    [LuisModel("9da30c54-0d95-4da3-84df-93cccf9ddd36", "bcb9ff185bdb4528915da32797310016")]
    public partial class LuisModel : LuisDialog<object>
    {
        [NonSerialized]
        private PretendBackendService _backEndService;
        [NonSerialized]
        private TelemetryClient _telemetry;

        private ResumptionCookie _resumptionCookie;

        public LuisModel()
        {
            _backEndService = new PretendBackendService();
        }

        protected async override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            try
            {
                var activity = (Activity) await item;
                _resumptionCookie = new ResumptionCookie(activity);
                await base.MessageReceived(context, item);
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex);
            }
           
        }

        [LuisIntent("FindTalkPerson")]
        public async Task PersonFinder(IDialogContext context, LuisResult result)
        {
            EntityRecommendation recommendationFirst = null;
            EntityRecommendation recommendationLast = null;

            result.TryFindEntity("FirstName", out recommendationFirst);
            result.TryFindEntity("LastName", out recommendationLast);

            if (recommendationFirst != null || recommendationLast != null)
            {
                var entityFirstName = recommendationFirst?.Entity ?? string.Empty;
                var entityLastName = recommendationLast?.Entity ?? string.Empty;

                if (entityFirstName.Length <= 1 && entityLastName.Length <= 1)
                {
                    await context.PostAsync(
                        "Sorry, I wasn't able to find anything for that speaker. Try entering their full name.");
                    return;
                }

                var session = await _backEndService.GetByPresenter(entityFirstName, entityLastName);

                if (session == null)
                {
                    await context.PostAsync(
                        "Sorry, I wasn't able to find anything for that speaker. They might not be presenting at Ignite this year.");
                }
                else
                {
                    var replyActivity = await CreateSessionCardReply();
                    AddSessionCardToReply(replyActivity, session, true);
                    await context.PostAsync(replyActivity);
                    context.Wait(MessageReceived);
                }
            }
            else
            {
                await context.PostAsync(
                    "Sorry, I didn't catch the speaker's name. Could you try again. For example, When is Scott Guthrie presenting?");
            }
        }

        [LuisIntent("")]
        public async Task NoItent(IDialogContext context, LuisResult result)
        {

        }
    }
}