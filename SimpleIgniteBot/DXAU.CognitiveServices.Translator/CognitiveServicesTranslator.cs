using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DXAU.CognitiveServices.Translator.MicrosoftTranslator;

namespace DXAU.CognitiveServices.Translator
{
    public class CognitiveServicesTranslator
    {
        private static List<KeyCacheItem> _cache = new List<KeyCacheItem>() ;

        readonly LanguageServiceClient _translatorService = new LanguageServiceClient();

        string _checkCache(string subsKey)
        {
            var item = _cache.FirstOrDefault(_ => _.SubscriptionKey == subsKey && _.Expires > DateTime.Now);
            return item?.Bearer;
        }

        void _setCache(string subsKey, string bearer)
        {
            var item = _cache.FirstOrDefault(_ => _.SubscriptionKey == subsKey && _.Expires <= DateTime.Now);
            if (item != null)
            {
                _cache.Remove(item);
            }

            _cache.Add(new KeyCacheItem
            {
                Bearer = bearer,
                Expires = DateTime.Now.AddMinutes(8),
                SubscriptionKey = subsKey
            });
        }

        async Task<string> _init(string subsKey)
        {
            var bearer = _checkCache(subsKey);

            if (bearer == null)
            {
                var authTokenSource = new AzureAuthToken(subsKey);

                try
                {
                    bearer = await authTokenSource.GetAccessTokenAsync();
                    _setCache(subsKey, bearer);
                }

                catch (HttpRequestException)
                {
                    switch (authTokenSource.RequestStatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            Console.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                            break;
                        case HttpStatusCode.Forbidden:
                            Console.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                            break;
                    }
                    throw;
                }
            }

            return bearer;
        }

        public async Task<string> Detect(string subsKey, string detectString)
        {
            var bearer = await _init(subsKey);
            var languages = await _translatorService.DetectAsync(bearer, detectString);
            return languages;
        }

        public async Task<string> Translate(string subsKey, string translateString, string sourceLanguage, string destinationLanguage)
        {
            var bearer = await _init(subsKey);
            var result = await _translatorService.TranslateAsync(bearer, translateString, sourceLanguage, destinationLanguage,
                "text/plain", "general", string.Empty);
            return result;
        }
    }
}
