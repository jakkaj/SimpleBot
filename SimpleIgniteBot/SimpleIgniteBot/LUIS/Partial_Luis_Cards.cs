using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Humanizer;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleIgniteBot.Services.Entities;

namespace SimpleIgniteBot.Bot
{
    public partial class LuisModel : LuisDialog<object>
    {
        public async Task<Activity> CreateSessionCardReply()
        {
            Activity originalActivity = (Activity)_resumptionCookie.GetMessage();

            Activity replyToConversation = originalActivity.CreateReply("Here are the sessions: ");
            replyToConversation.Recipient = originalActivity.From;
            replyToConversation.Type = "message";
            //replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            replyToConversation.Attachments = new List<Attachment>();

            return replyToConversation;
        }

        public void AddSessionCardToReply(Activity activity, Session session, bool addButton)
        {
            if (session == null || activity == null)
            {
                return;
            }

            var presenters = string.Join(" and ", session.Presenters.Select(_ => _.FullName));

            if (session.DateTime_Start != null)
            {
                AddSessionCardToReply(activity,
                    $"{_getSessionTimeString(session)}",
                    $"{session.Code}",
                    $"{session.Name} by {presenters}",
                    session.Room,
                    addButton);
            }
        }

        public void AddSessionCardToReply(Activity activity, string timeslotDisplayText, string sessionCode, string sessionTitle, string sessionRoom, bool AddButton)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://ignitebot.azurewebsites.net/Images/ignite.png"));

            List<CardAction> cardButtons = new List<CardAction>();

            if (!string.IsNullOrWhiteSpace(sessionCode) && _supportsButtons(activity))
            {
                CardAction plButton;

                if (AddButton)
                {
                    plButton = new CardAction()
                    {
                        Value = $"Add {sessionCode} to my schedule",
                        Type = "postBack",
                        Title = "Add to schedule"
                    };
                }
                else
                {
                    plButton = new CardAction()
                    {
                        Value = $"Remove {sessionCode} from my schedule",
                        Type = "postBack",
                        Title = "Remove from schedule"
                    };
                }
                cardButtons.Add(plButton);


                CardAction infoButton = new CardAction()
                {
                    Value = $"Tell me more about {sessionCode}",
                    Type = "postBack",
                    Title = "More information"
                };
                cardButtons.Add(infoButton);
            }




            HeroCard plCard = new HeroCard()
            {
                Title = $"{timeslotDisplayText} {sessionCode} {sessionRoom}",
                Subtitle = sessionTitle,
                Images = cardImages,
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();
            activity.Attachments.Add(plAttachment);
        }

        bool _supportsButtons(Activity activity)
        {
            switch (activity.ChannelId)
            {
                // These channels support cards (new skype clients now out)
                case "slack":
                case "facebook":
                case "telegram":
                case "skype":
                case "emulator":
                    return true;
                default:
                    return false;
            }
        }

        string _getSessionTimeString(Session session)
        {
            var startTime = session.DateTime_Start.ToShortTimeString();
            var day = session.DateTime_Start.DayOfWeek.ToString();
            var timeAway = session.DateTime_Start.Subtract(DateTime.Now);
            var inOne = timeAway.TotalMilliseconds < 0 ? "" : "in ";
            var inTwo = timeAway.TotalMilliseconds < 0 ? " ago" : "";
            var time = $"{startTime} {day} - ({inOne}{timeAway.Humanize(2)}{inTwo})";

            return time;
        }


    }
}