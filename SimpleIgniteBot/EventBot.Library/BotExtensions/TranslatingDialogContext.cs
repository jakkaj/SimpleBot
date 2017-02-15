using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBot.EditableDialogs;
using EventBot.SupportLibrary.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace SimpleIgniteBot.Bot
{
    public class TranslatingDialogContext : IDialogContext
    {
        private readonly IDialogContext _original;

        private TranslatorService _translatorService;

        public TranslatingDialogContext(IDialogContext original)
        {
            _translatorService = new TranslatorService();
            _original = original;
        }

        async Task _translate(string translateString, string languageTo, Action<string> callback)
        {
            var result = await _translatorService.Translate(translateString, "en",
                languageTo);
            callback(result);
        }

        public async Task PostAsync(IMessageActivity message, CancellationToken cancellationToken = new CancellationToken())
        {
            var language = _translatorService.GetLanguage(_original);

            if (string.IsNullOrWhiteSpace(language))
            {
                await _original.PostAsync(message, cancellationToken);
                return;
            }



            var translateTasks = new List<Task>();

            if (!string.IsNullOrWhiteSpace(message.Text))
            {
                translateTasks.Add(_translate(message.Text, language, (s) => message.Text = s));
            }

            if (message.Attachments != null)
            {
                foreach (var a in message.Attachments)
                {
                    if (a.Content is HeroCard h)
                    {
                        var closure = h;
                        translateTasks.Add(_translate(h.Subtitle, language, (s) => closure.Subtitle = s));
                        translateTasks.Add(_translate(h.Text, language, (s) => closure.Text = s));
                        translateTasks.Add(_translate(h.Title, language, (s) => closure.Title = s));

                        translateTasks.AddRange(
                            from b in h.Buttons
                            let closureB = b
                            select _translate(b.Title, language,
                            (s) => closureB.Title = s));
                    }
                }
            }

            await Task.WhenAll(translateTasks);

            await _original.PostAsync(message, cancellationToken);
        }

        public void Wait<R>(ResumeAfter<R> resume)
        {
            _original.Wait<R>(resume);
        }

        public void Call<R>(IDialog<R> child, ResumeAfter<R> resume)
        {
            if (child is EditablePromptChoice<string> p)
            {
                var language = _translatorService.GetLanguage(_original);

                if (string.IsNullOrWhiteSpace(language))
                {
                    _original.Call<R>(child, resume);
                    return;
                }

                var translateTasks = new List<Task>();

                var optOld = p.PromptOptions;

                var prompt = AsyncHelpers.RunSync(() => _translatorService.Translate(optOld.Prompt, "en", language));
                var retry = AsyncHelpers.RunSync(() => _translatorService.Translate(optOld.Retry, "en", language));

                //var prompt = "";
                //var retry = "";

                //translateTasks.Add(_translate(optOld.Prompt, language, (s) => prompt = s));
                //translateTasks.Add(_translate(optOld.Retry, language, (s) => retry = s));

                //AsyncHelpers.RunSync(()=>Task.WhenAll(translateTasks));

                //Task.WhenAll(translateTasks).ConfigureAwait(true);

                var optNew = new PromptOptions<string>(prompt, retry, optOld.TooManyAttempts, optOld.Options, optOld.Attempts, optOld.PromptStyler, optOld.Descriptions);

                var newDialog = new PromptDialog.PromptChoice<string>(optNew);
                _original.Call<string>(newDialog, resume as ResumeAfter<string>);

                return;

            }
            _original.Call<R>(child, resume);
        }

        public Task Forward<R, T>(IDialog<R> child, ResumeAfter<R> resume, T item, CancellationToken token)
        {
            return _original.Forward<R, T>(child, resume, item, token);
        }

        public void Done<R>(R value)
        {
            _original.Done<R>(value);
        }

        public void Fail(Exception error)
        {
            _original.Fail(error);
        }

        public void Reset()
        {
            _original.Reset();
        }

        public IReadOnlyList<Delegate> Frames => _original.Frames;

        public Task LoadAsync(CancellationToken cancellationToken)
        {
            return _original.LoadAsync(cancellationToken);
        }

        public Task FlushAsync(CancellationToken cancellationToken)
        {
            return _original.FlushAsync(cancellationToken);
        }

        public IBotDataBag UserData => _original.UserData;
        public IBotDataBag ConversationData => _original.ConversationData;
        public IBotDataBag PrivateConversationData => _original.PrivateConversationData;


        public IMessageActivity MakeMessage()
        {
            return _original.MakeMessage();
        }

        public CancellationToken CancellationToken => _original.CancellationToken;
        public IActivity Activity => _original.Activity;
        public void Post<E>(E @event, ResumeAfter<E> resume)
        {
            _original.Post<E>(@event, resume);
        }
    }

}