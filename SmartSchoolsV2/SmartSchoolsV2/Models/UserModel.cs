using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Models
{
    public class UserModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string UrlPhoto { get; set; } = "";

        public UserModel()
        {

        }

        public UserModel(string id, string name, string photo)
        {
            Id = id;
            Name = name;
            UrlPhoto = photo;
        }
    }
}
