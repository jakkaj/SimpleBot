using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleIgniteBot.Bot;

namespace EventBot.EditableDialogs
{
    [Serializable]
    public class EditablePromptChoice<T> : PromptDialog.PromptChoice<T>
    {
        public PromptOptions<T> PromptOptions => promptOptions;

        public EditablePromptChoice(IEnumerable<T> options, string prompt, string retry, int attempts, PromptStyle promptStyle = PromptStyle.Auto, IEnumerable<string> descriptions = null) : base(options, prompt, retry, attempts, promptStyle, descriptions)
        {
        }

        public EditablePromptChoice(PromptOptions<T> promptOptions) : base(promptOptions)
        {
        }

        protected override Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> message)
        {
            if (!(context is TranslatingDialogContext))
            {
                context = new TranslatingDialogContext(context);
            }

            return base.MessageReceivedAsync(context, message);
        }
    }

    public class EditablePromptDialog : PromptDialog
    {
        public new static void Choice<T>(IDialogContext context, ResumeAfter<T> resume, PromptOptions<T> promptOptions)
        {
            if (!(context is TranslatingDialogContext))
            {
                context = new TranslatingDialogContext(context);
            }

            var child = new EditablePromptChoice<T>(promptOptions);

            var cp = new ResumeReplacer<T>(resume);

            context.Call<T>(child, cp.ResumeReplace);
        }

        [Serializable]
        public class ResumeReplacer<T>
        {
            private readonly ResumeAfter<T> _outerResumer;

            public ResumeReplacer()
            {

            }

            public ResumeReplacer(ResumeAfter<T> outerResumer)
            {
                _outerResumer = outerResumer;
            }

            public async Task ResumeReplace(IDialogContext outerContext, IAwaitable<T> result)
            {
                if (!(outerContext is TranslatingDialogContext))
                {
                    outerContext = new TranslatingDialogContext(outerContext);
                }

                await _outerResumer(outerContext, result);
            }
        }

        public new static void Choice<T>(IDialogContext context, ResumeAfter<T> resume, IEnumerable<T> options, string prompt, string retry = null, int attempts = 3, PromptStyle promptStyle = PromptStyle.Auto, IEnumerable<string> descriptions = null)
        {
            if (!(context is TranslatingDialogContext))
            {
                context = new TranslatingDialogContext(context);
            }
            Choice(context, resume, new PromptOptions<T>(prompt, retry, attempts: attempts, options: options.ToList(), promptStyler: new PromptStyler(promptStyle), descriptions: descriptions?.ToList()));
        }
    }
}