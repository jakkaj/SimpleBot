using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimpleIgniteBot.QnAMaker
{

    public class QnaMakerResult
    {
        /// <summary>
        /// The top answer found in the QnA Maker Service.
        /// </summary>
        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        /// <summary>
        /// The score in range [0, 100] corresponding to the top answer found in the QnA Maker Service.
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }

}
