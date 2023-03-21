using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Class
{
    public class Connection
    {
        public bool IsConnected()
        {
            if (CrossConnectivity.Current.IsConnected)
                return true;

            return false;
        }
    }
}
