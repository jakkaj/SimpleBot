using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;

namespace SimpleIgniteBot.QnAMaker
{
    
    public class QnaMakerKb
    {
        private readonly Uri qnaBaseUri = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");

        private readonly string KbId = ConfigurationManager.AppSettings["QnaKnowledgeBaseId"];
        private readonly string SubscriptionKey = ConfigurationManager.AppSettings["QnaSubscriptionKey"];

        private readonly TelemetryClient _telemetry;


        public QnaMakerKb()
        {
            _telemetry = new TelemetryClient();
        }

        // Sample HTTP Request:
        // POST /knowledgebases/{KbId}/generateAnswer
        // Host: https://westus.api.cognitive.microsoft.com/qnamaker/v1.0
        // Ocp-Apim-Subscription-Key: {SubscriptionKey}
        // Content-Type: application/json
        // {"question":"hi"}
        public async Task<QnaMakerResult> SearchFaqAsync(string question)
        {
            var responseString = string.Empty;

            var uri = new UriBuilder($"{qnaBaseUri}/knowledgebases/{this.KbId}/generateAnswer").Uri;

            var postBody = $"{{\"question\": \"{question}\"}}";

            //Send the POST request
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Ocp-Apim-Subscription-Key", this.SubscriptionKey);

                try
                {
                    responseString = await client.UploadStringTaskAsync(uri, postBody);

                }
                catch (WebException we)
                {
                    throw;
                }
            }

            var result = ConvertResponseFromJson(responseString);

            if (result == null || result.Score <= 0)
            {
                _telemetry.TrackTrace("QandAQuestionAnswerFound", new Dictionary<string, string>
            {
                {"Question", question }
            });
                return null;
            }

            _telemetry.TrackTrace("QandAQuestionAnswerFound", new Dictionary<string, string>
            {
                {"Question", question },
                {"Answer", result.Answer }
            });

            return result;
        }

        private QnaMakerResult ConvertResponseFromJson(string responseString)
        {
            QnaMakerResult response;
            try
            {
                response = JsonConvert.DeserializeObject<QnaMakerResult>(responseString);
            }
            catch
            {
                throw new Exception("Unable to deserialize QnA Maker response string.");
            }

            return response;
        }



    }
}