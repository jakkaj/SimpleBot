using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
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
    public partial class LuisModel : LuisDialog<object>, IDialog<object>
    {
        [NonSerialized]
        private PretendBackendService _backEndService;
        [NonSerialized]
        private TelemetryClient _telemetry;

        private ResumptionCookie _resumptionCookie;

        public LuisModel()
        {
            _setup();
        }


        [OnDeserialized]
        internal void _deserialized(StreamingContext context)
        {
            try
            {
                _setup();
            }
            catch (Exception e)
            {

            }
        }

        void _setup()
        {
            _backEndService = new PretendBackendService();
            _telemetry = new TelemetryClient();
        }


        async Task IDialog<object>.StartAsync(IDialogContext context)
        {
            try
            {
                var translatingContext = new TranslatingDialogContext(context);
                await base.StartAsync(translatingContext);
            }
            catch (Exception e)
            {
                _telemetry.TrackException(e);
                _telemetry.TrackTrace("Exception in EvenLuisModel.StartAsync (propagated)");
            }
        }

        protected async override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            try
            {
                if (!(context is TranslatingDialogContext))
                {
                    context = new TranslatingDialogContext(context);
                }

                var activity = (Activity) await item;
                _resumptionCookie = new ResumptionCookie(activity);
                await base.MessageReceived(context, item);
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex);
            }
        }

        [LuisIntent("WhatsOnTomorrow")]
        public async Task WhatsOnTomorrow(IDialogContext context, LuisResult result)
        {
            await WhereNext(context);
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