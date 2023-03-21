using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.DataServices;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using System;
using System.IO;
using System.Net;

namespace SmartSchoolsV2.Services
{
    public class ChatService : IChatService
    {
        public string requestUrl = Settings.requestUrl;
        private readonly IRequestProvider _requestProvider;
        public ChatService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public event EventHandler NewMessageReceived;
        public void OnMessageReceived(PushNotification message)
        {
            EventHandler eh = NewMessageReceived;
            if (eh != null)
                eh(this, new BodyEventArgs(message));
        }

        //public void SendMessage(ChatMessage message)
        //{
        //    string url = "https://fcm.googleapis.com/fcm/send";

        //    var data = new MessageModel(message);
        //    _requestProvider.PostAsync(url, data);
        //}

        public async void SendMessage(ChatMessage message)
        {
            //using (var client = new HttpClient())
            //{
            //    try
            //    {
            //        JObject obj = new JObject();

            //        obj.Add("message_from", message.MessageFrom);
            //        obj.Add("message_to", message.MessageTo);
            //        obj.Add("push_type", message.PushType);
            //        obj.Add("channel_id", message.ChannelId);
            //        obj.Add("channel_name", message.ChannelName);
            //        obj.Add("channel_type_id", message.ChannelTypeId);
            //        obj.Add("channel_photo_url", message.ChannelUrlPhoto);
            //        obj.Add("full_name", message.FullName);
            //        obj.Add("message", message.Text);
            //        obj.Add("time_message", message.TextTime.ToString("yyyy/MM/dd HH:mm:ss"));
            //        obj.Add("photo_url", message.UrlPhoto);
            //        obj.Add("attach_url", message.AttachImg);
            //        obj.Add("first_id", message.FirstId);
            //        obj.Add("first_name", message.FirstName);
            //        obj.Add("second_id", message.SecondId);
            //        obj.Add("second_name", message.SecondName);
            //        obj.Add("node_path", message.NodePath);

            //        var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
            //        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

            //        var response = await client.PostAsync(requestUrl + "/api/v2/chat/send", content);
            //        string result = await response.Content.ReadAsStringAsync();
            //        if (response.StatusCode != HttpStatusCode.OK)
            //        {
            //            result = ResponseMessage(response.StatusCode);
            //        }
            //        //return result;
            //    }
            //    catch (WebException ex)
            //    {
            //        if (ex.Response != null)
            //        {
            //            string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            //            throw new System.Exception($"response :{responseContent}", ex);
            //        }
            //        throw;
            //    }
            //}
        }

        public string ResponseMessage(HttpStatusCode StatusCode)
        {
            string message = string.Empty;
            ResponseProperty prop = new ResponseProperty();
            prop.Success = false;
            if (StatusCode == HttpStatusCode.BadRequest)
            {
                prop.Message = AppResources.BadRequestText;
            }
            else if (StatusCode == HttpStatusCode.Unauthorized)
            {
                bool isLogin = Settings.isLogin;

                if (isLogin)
                {
                    Settings.isLogin = false;
                    Settings.profileId = 0;
                    Settings.userRoleId = 0;
                    Settings.parentId = 0;
                    Settings.staffId = 0;
                    Settings.merchantId = 0;
                    Settings.staffTypeId = 0;
                    Settings.schoolId = 0;
                    App.Current.MainPage.DisplayAlert(AppResources.SessionExpiredText, AppResources.SessionExpiredMessageText, "OK");
                }
                //prop.Message = "401 - Unauthorized";
            }
            else if (StatusCode == HttpStatusCode.Forbidden)
            {
                prop.Message = AppResources.ForbiddenText;
            }
            else if (StatusCode == HttpStatusCode.NotFound)
            {
                prop.Message = AppResources.NotFoundText;
            }
            else if (StatusCode == HttpStatusCode.RequestTimeout)
            {
                prop.Message = AppResources.RequestTimeoutText;
            }
            else if (StatusCode == HttpStatusCode.InternalServerError)
            {
                prop.Message = AppResources.InternalServerErrorText;
            }
            else if (StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                prop.Message = AppResources.ServiceUnavailableText;
            }
            return JsonConvert.SerializeObject(prop);
        }

    }
    public class BodyEventArgs : EventArgs
    {
        public PushNotification Message { get; set; }

        public BodyEventArgs(PushNotification message)
        {
            Message = message;
        }
    }
}
