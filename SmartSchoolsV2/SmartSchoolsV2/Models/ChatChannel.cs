using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Models
{
    public class ChatChannel
    {
        [PrimaryKey]
        public string channel_id { get; set; }
        public string channel_name { get; set; }
        public string node_path { get; set; }
        public int message_from { get; set; }
        public int message_to { get; set; }
        public int first_id { get; set; }
        public string first_name { get; set; }
        public int second_id { get; set; }
        public string second_name { get; set; }
        public int channel_type_id { get; set; }
        public string channel_photo_url { get; set; }
        public string photo_url { get; set; }
        public string attach_url { get; set; }
        public string last_message { get; set; }
        public DateTime time_message { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }
}
