using MvvmHelpers;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace SmartSchoolsV2.Models
{
    public class ChatMessage
    {
        [JsonProperty("message_id")]
        public int MessageId { get; set; } = 0;

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; } = "";

        [JsonProperty("message_type_id")]
        public int MessageTypeId { get; set; } = 0;

        [JsonProperty("message")]
        public string Message { get; set; } = "";

        [JsonProperty("photo_url")]
        public string PhotoUrl { get; set; } = "";

        [JsonProperty("receiver_id")]
        public int ReceiverId { get; set; } = 0;

        [JsonProperty("receiver_name")]
        public string ReceiverName { get; set; } = "";

        [JsonProperty("receiver_photo_url")]
        public string ReceiverPhotoUrl { get; set; } = "";

        [JsonProperty("sender_id")]
        public int SenderId { get; set; } = 0;

        [JsonProperty("sender_name")]
        public string SenderName { get; set; } = "";

        [JsonProperty("sender_photo_url")]
        public string SenderPhotoUrl { get; set; } = "";


        [JsonProperty("sent_at")]
        public DateTime SentAt { get; set; }


        [JsonIgnore]
        public bool IsYourMessage { get; set; }
    }
}
