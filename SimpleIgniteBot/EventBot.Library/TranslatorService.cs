using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using DXAU.CognitiveServices.Translator;
using Microsoft.ApplicationInsights;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace EventBot.SupportLibrary.Services
{
    public class TranslatorService
    {
        readonly CognitiveServicesTranslator _translator = new CognitiveServicesTranslator();

        TelemetryClient _logService = new TelemetryClient();



        public string GetLanguage(IDialogContext context)
        {
            context.UserData.TryGetValue<string>("LanguageCode", out string code);
            return code;
        }

        public async Task<string> GetLanguage(Activity activity)
        {
            var sc = activity.GetStateClient();
            var data = await sc.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var code = data.GetProperty<string>("LanguageCode");
            return code;
        }

        public async Task SetLanguage(Activity activity, string languageCode)
        {
            var sc = activity.GetStateClient();
            var data = await sc.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

            if (string.IsNullOrWhiteSpace(languageCode))
            {
                data.RemoveProperty("LanguageCode");
            }
            else
            {
                data.SetProperty("LanguageCode", languageCode);
            }

            await sc.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, data);

            _logService.TrackTrace("LangaugeSet", new Dictionary<string, string> { { "LanguageCode", languageCode } });
        }

        public void SetLanguage(IDialogContext context, string languageCode)
        {

            if (string.IsNullOrWhiteSpace(languageCode))
            {
                context.UserData.RemoveValue("LanguageCode");
            }
            else
            {
                context.UserData.SetValue("LanguageCode", languageCode);
            }
            _logService.TrackTrace("LangaugeSet", new Dictionary<string, string> { { "LanguageCode", languageCode } });
        }

        private string _subsKey => ConfigurationManager.AppSettings["AzureTranslateKey"];

        public async Task<string> Translate(string text, string sourceLanguage, string destinationLanguage)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var languageResult = await _translator.Translate(_subsKey, text, sourceLanguage, destinationLanguage);
            if (string.IsNullOrWhiteSpace(languageResult))
            {
                _logService.TrackTrace("LanguageTranslationFail",

                    new Dictionary<string, string>
                    {
                        {"Text", text}
                    });
            }
            else
            {
                _logService.TrackTrace("LanguageTranslationSuccess",

                    new Dictionary<string, string>
                    {
                        {"Text", text},
                        {"TranslatedText", text }
                    });
            }

            return languageResult;
        }

        public async Task<string> Translate(string text, string sourceLanguage)
        {
            return await Translate(text, sourceLanguage, "en");
        }

        public async Task<string> TranslateBack(Activity activity, string text)
        {
            var targetLanguage = await GetLanguage(activity);

            if (string.IsNullOrWhiteSpace(targetLanguage))
            {
                return text;
            }

            return await Translate(text, "en", targetLanguage);
        }

        public async Task<string> Detect(string text)
        {
            var languageResult = await _translator.Detect(_subsKey, text);
            if (string.IsNullOrWhiteSpace(languageResult))
            {
                _logService.TrackTrace("LanguageDetectionFail",

                    new Dictionary<string, string>
                    {
                        {"Text", text}
                    });
            }
            else
            {
                _logService.TrackTrace("LanguageDetectionSuccess",

                    new Dictionary<string, string>
                    {
                        {"Text", text},
                        {"DetectedLanguage", text }
                    });
            }

            return languageResult;
        }
    }
}
