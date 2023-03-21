using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Models
{
    public class Topic
    {
        [PrimaryKey]
        public string topic { get; set; }
    }
}
