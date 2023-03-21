using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Models.FirebaseModels
{
    public class MessageModel
    {
        [JsonProperty("to")]
        public string To { get; set; } = Settings.topic;

        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";

        [JsonProperty("notification")]
        public NotificationModel NotificationModel { get; set; }

        [JsonProperty("data")]
        public ChatMessage Message { get; set; }

        public MessageModel(ChatMessage message)
        {
            Message = message;

            NotificationModel = new NotificationModel()
            {
                Title = message.FullName,
                Body = message.Text,
                Icon = "icon_60x60.png"
            };
        }
    }


    public class NotificationModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
