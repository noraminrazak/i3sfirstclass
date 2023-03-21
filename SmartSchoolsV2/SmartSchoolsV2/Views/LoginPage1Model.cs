using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Views
{
    public class LoginPage1Model : BasePageModel
    {
        public void Reload()
        {
            // ImageUrl = Helpers.GetRandomImageUrl();
            ImageUrl = @"https://staging.i-3s.com.my/Images/i3sLogo.png";
        }

        public string ImageUrl { get; set; }
    }
}
