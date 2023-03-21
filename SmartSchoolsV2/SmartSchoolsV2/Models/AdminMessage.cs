using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Models
{
    public class PushNotification
    {
        [JsonProperty("body")]
        public string Body { get; set; } = "";

        [JsonProperty("title")]
        public string Title { get; set; } = "";

    }
}
