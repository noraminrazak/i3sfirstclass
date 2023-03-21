using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Models
{
    public class PushMessage
    {
        [PrimaryKey]
        public int id { get; set; }
        public string from { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string attach_url { get; set; }
        public string link_url { get; set; }
        public DateTime receive_at { get; set; }
        public bool is_read { get; set; }
    }
}
