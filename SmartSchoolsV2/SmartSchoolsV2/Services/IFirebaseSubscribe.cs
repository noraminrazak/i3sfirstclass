using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.Services
{
    public interface IFirebaseSubscribe
    {
        void SubscribeToTopic(string channel);
        void UnsubscribeFromTopic(string channel);
    }
}
