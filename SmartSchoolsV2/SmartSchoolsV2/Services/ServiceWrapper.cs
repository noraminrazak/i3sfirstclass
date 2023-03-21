using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartSchoolsV2.Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        Rijndael rijn = new Rijndael();
        public string requestUrl = Settings.requestUrl;
        public string salt = "a778c74fec934fdc993108beb821c8b5a8c2ff55";

        #region User
        public async Task<string> PostUserInitLogin(string username)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username)
                    });
                    var response = await client.PostAsync(requestUrl + "/api/v2/user/init-login?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserLastLogin(string username)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username)
                    });
                    var response = await client.PostAsync(requestUrl + "/api/v2/user/last-login?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserAuthLogin(string username, string password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("grant_type", "password")
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/auth-login", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    LoginProperty value = JsonConvert.DeserializeObject<LoginProperty>(result);

                    string token = value.access_token;
                    string last_login = value.last_login;
                    ResponseProperty prop = new ResponseProperty();
                    prop.Success = true;
                    prop.Code = last_login;
                    prop.Message = token;

                    return JsonConvert.SerializeObject(prop);
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserRoleAccount(string username)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/role-account", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserUpdateDeviceToken(int profile_id, string device_token, int device_platform_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("device_token", device_token),
                        new KeyValuePair<string, string>("device_platform_id", device_platform_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/update-device-token", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserChangePassword(int profile_id, string username, string password, string new_password, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("new_password", new_password),
                        new KeyValuePair<string, string>("update_by", update_by),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/change-password?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserCreatePassword(string mobile_number, string username, string password, string new_password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("mobile_number", mobile_number),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("confirm_password", new_password)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/create-password?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserChangePin(string username, string new_pin, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("new_pin", new_pin),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/change-pin?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserForgotPassword(string username, string email)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("email", email)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/forgot-password?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserRegister(string full_name, int nationality_id, int card_type_id,
            string identity_number, string date_of_birth, string mobile_number, string email, string address,
            string postcode, string city, int state_id, int country_id, string mother_maiden_name, string occupation,
            string employer_name, string password, int marketing_flag, int user_role_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("full_name", full_name),
                        new KeyValuePair<string, string>("nationality_id", nationality_id.ToString()),
                        new KeyValuePair<string, string>("card_type_id", card_type_id.ToString()),
                        new KeyValuePair<string, string>("identity_number", identity_number),
                        new KeyValuePair<string, string>("date_of_birth", date_of_birth),
                        new KeyValuePair<string, string>("mobile_number", mobile_number),
                        new KeyValuePair<string, string>("email", email),
                        new KeyValuePair<string, string>("address", address),
                        new KeyValuePair<string, string>("postcode", postcode),
                        new KeyValuePair<string, string>("city", city),
                        new KeyValuePair<string, string>("state_id", state_id.ToString()),
                        new KeyValuePair<string, string>("country_id", country_id.ToString()),
                        new KeyValuePair<string, string>("mother_maiden_name", mother_maiden_name),
                        new KeyValuePair<string, string>("occupation", occupation),
                        new KeyValuePair<string, string>("employer_name", employer_name),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("marketing_flag", marketing_flag.ToString()),
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/account-register?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserVerifyAccount(string mobile_number, string username, string otp)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("mobile_number", mobile_number),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("otp", otp)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/verify-account?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserResubmitKYC(string username)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/resubmit-kyc?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserProfile(int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/profile", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostUserUpdateProfile(int profile_id, string full_name, int user_race_id, int card_type_id, string nric,
            string date_of_birth, string mobile_number, string email, string address, string postcode, string city, int state_id, string state_name, 
            int country_id, string mother_maiden_name, string occupation, string employer_name,string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("full_name", full_name),
                        new KeyValuePair<string, string>("user_race_id", user_race_id.ToString()),
                        new KeyValuePair<string, string>("card_type_id", card_type_id.ToString()),
                        new KeyValuePair<string, string>("nric", nric),
                        new KeyValuePair<string, string>("date_of_birth", date_of_birth),
                        new KeyValuePair<string, string>("mobile_number", mobile_number),
                        new KeyValuePair<string, string>("email", email),
                        new KeyValuePair<string, string>("address", address),
                        new KeyValuePair<string, string>("postcode", postcode),
                        new KeyValuePair<string, string>("city", city),
                        new KeyValuePair<string, string>("state_id", state_id.ToString()),
                        new KeyValuePair<string, string>("state_name", state_name),
                        new KeyValuePair<string, string>("country_id", country_id.ToString()),
                        new KeyValuePair<string, string>("mother_maiden_name", mother_maiden_name),
                        new KeyValuePair<string, string>("occupation", occupation),
                        new KeyValuePair<string, string>("employer_name", employer_name),
                        new KeyValuePair<string, string>("update_by", update_by),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/update-user-info?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserUpdatePhoto(int profile_id, string file_name, string photo_base64, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("profile_id", profile_id);
                    obj.Add("file_name", file_name);
                    obj.Add("photo_base64", photo_base64);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/update-photo", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserRemovePhoto(int profile_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("profile_id", profile_id);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/remove-photo", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserUploadImage(string identity_number, int image_type_id, string file_name, string photo_base64, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("identity_number", identity_number);
                    obj.Add("image_type_id", image_type_id);
                    obj.Add("file_name", file_name);
                    obj.Add("photo_base64", photo_base64);
                    obj.Add("create_by", create_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/upload-image?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserNotify(int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("profile_id", profile_id);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/notify", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserUpdateNotify(int notify_id, int profile_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("notify_id", notify_id);
                    obj.Add("profile_id", profile_id);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/update-notify", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserDeleteNotify(int notify_id, int profile_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("notify_id", notify_id);
                    obj.Add("profile_id", profile_id);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/delete-notify", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostUserDeleteAccount(string username)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("username", username);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/user/delete-account?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Staff
        public async Task<string> PostStaffProfile(int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/profile", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffClassRelationship(int school_id, int staff_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/class-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffClubRelationship(int school_id, int staff_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/club-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostStaffCreatePost(int school_id, int post_group_id, int class_id, int club_id, int staff_id, string post_message,
            string file_name, string photo_base64, DateTime start_at, DateTime end_at, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("school_id", school_id);
                    obj.Add("post_group_id", post_group_id);
                    obj.Add("class_id", class_id);
                    obj.Add("club_id", club_id);
                    obj.Add("staff_id", staff_id);
                    obj.Add("post_message", post_message);
                    obj.Add("file_name", file_name);
                    obj.Add("photo_base64", photo_base64);
                    obj.Add("start_at", start_at);
                    obj.Add("end_at", end_at);
                    obj.Add("create_by", create_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/create-post?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffUpdatePost(int post_id, int school_id, int post_group_id, int class_id, int club_id, int staff_id, string post_message,
            string file_name, string photo_base64, DateTime start_at, DateTime end_at, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();
                    obj.Add("post_id", post_id);
                    obj.Add("school_id", school_id);
                    obj.Add("post_group_id", post_group_id);
                    obj.Add("class_id", class_id);
                    obj.Add("club_id", club_id);
                    obj.Add("staff_id", staff_id);
                    obj.Add("post_message", post_message);
                    obj.Add("file_name", file_name);
                    obj.Add("photo_base64", photo_base64);
                    obj.Add("start_at", start_at);
                    obj.Add("end_at", end_at);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/update-post?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffRemovePostPhoto(int post_id, int school_id, int post_group_id, int class_id, int club_id, int staff_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();
                    obj.Add("post_id", post_id);
                    obj.Add("school_id", school_id);
                    obj.Add("post_group_id", post_group_id);
                    obj.Add("class_id", class_id);
                    obj.Add("club_id", club_id);
                    obj.Add("staff_id", staff_id);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/remove-post-photo?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffRemovePost(int post_id, int school_id, int post_group_id, int class_id, int club_id, int staff_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();
                    obj.Add("post_id", post_id);
                    obj.Add("school_id", school_id);
                    obj.Add("post_group_id", post_group_id);
                    obj.Add("class_id", class_id);
                    obj.Add("club_id", club_id);
                    obj.Add("staff_id", staff_id);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/remove-post?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffJoinClass(int class_id, int staff_id, string class_teacher_flag, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                        new KeyValuePair<string, string>("class_teacher_flag", class_teacher_flag),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/join-class?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffEnrollStudentClass(int school_id, int student_id, int class_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("student_id", student_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/enroll-student-class?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffRemoveStudentClass(int school_id, int student_id, int class_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("student_id", student_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/remove-student-class?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffRemoveClubMember(int relationship_id, int club_id, int profile_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("relationship_id", relationship_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/remove-club-member?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffJoinClub(int staff_id, int club_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/join-club?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffHandoverClub(int school_id, int club_id, int current_staff_id, int new_staff_id, int new_profile_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("current_staff_id", current_staff_id.ToString()),
                        new KeyValuePair<string, string>("new_staff_id", new_staff_id.ToString()),
                        new KeyValuePair<string, string>("new_profile_id", new_profile_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/handover-club?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffCreateClub(string club_name, int school_id, int staff_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("club_name", club_name),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/create-club?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffAddClubMember(int club_id, int profile_id, int user_role_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/add-club-member?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffAddClassStudent(int school_id, int class_id, int student_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("student_id", student_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/add-class-student?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffLeaveClass(int relationship_id, int class_id, int staff_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("relationship_id", relationship_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/leave-class?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffUpdateStudentAttendance(int report_id, int school_id, int class_id, int student_id, int attendance_id, int reason_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("report_id", report_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("student_id", student_id.ToString()),
                        new KeyValuePair<string, string>("attendance_id", attendance_id.ToString()),
                        new KeyValuePair<string, string>("reason_id", reason_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/update-student-attendance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffUpdateStaffAttendance(int report_id, int school_id, int staff_id, int attendance_id, int reason_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("report_id", report_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                        new KeyValuePair<string, string>("attendance_id", attendance_id.ToString()),
                        new KeyValuePair<string, string>("reason_id", reason_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/update-staff-attendance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffUpdateClubAttendance(int report_id, int school_id, int club_id, int profile_id, int attendance_id, int reason_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("report_id", report_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("attendance_id", attendance_id.ToString()),
                        new KeyValuePair<string, string>("reason_id", reason_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/update-club-attendance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffCreateClubAttendance(int school_id, int club_id, string entry_date, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/create-club-attendance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffSearchStudent(int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/search-student?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffSearchStudentWallet(int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/search-student-wallet?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffSearchParent(int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/search-parent?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffSearchStaff(int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/search-staff?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffSearchMerchant(int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/search-merchant?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffMonthlyAttendance(int school_id, int staff_id, string entry_month)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                         new KeyValuePair<string, string>("entry_month", entry_month)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/monthly-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffDailyAttendance(int school_id, int staff_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                         new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/daily-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffOutingRequestMonthGroup(int outing_status_id, int school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("outing_status_id", outing_status_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/outing-request-month-group", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffOutingRequestMonth(int outing_status_id, int school_id, DateTime outing_month)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("outing_status_id", outing_status_id.ToString()),
                         new KeyValuePair<string, string>("outing_month", outing_month.ToString("MM-yyyy"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/outing-request-month", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffOutingRequest(int outing_status_id, int school_id, DateTime outing_month, int outing_type_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("outing_status_id", outing_status_id.ToString()),
                         new KeyValuePair<string, string>("outing_month", outing_month.ToString("MM-yyyy")),
                         new KeyValuePair<string, string>("outing_type_id", outing_type_id.ToString()),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/outing-request", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffApproveOutingRequest(int outing_id, int student_id, int school_id, int approve_by_id, string approve_comment,string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("outing_id", outing_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("approve_by_id", approve_by_id.ToString()),
                         new KeyValuePair<string, string>("approve_comment", approve_comment),
                         new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/approve-outing-request", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffRejectOutingRequest(int outing_id, int student_id, int school_id, int approve_by_id, string approve_comment, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("outing_id", outing_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("approve_by_id", approve_by_id.ToString()),
                         new KeyValuePair<string, string>("approve_comment", approve_comment),
                         new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/reject-outing-request", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffShift(int school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/shift", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffRemoveStaff(int school_id, int staff_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/remove-staff?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStaffUpdateStaffShift(int school_id, int staff_id, int shift_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("staff_id", staff_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("shift_id", shift_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/staff/update-staff-shift?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Parent
        public async Task<string> PostParentProfile(int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/profile", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostParentSchoolRelationship(int parent_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("parent_id", parent_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/school-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostParentStudentRelationship(int parent_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("parent_id", parent_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/student-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostParentCreateStudentRelationship(int parent_id, int student_id, int school_id, int class_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("parent_id", parent_id.ToString()),
                        new KeyValuePair<string, string>("student_id", student_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/add-virtual-balance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostParentRemoveStudentRelationship(int parent_id, int student_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("parent_id", parent_id.ToString()),
                        new KeyValuePair<string, string>("student_id", student_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/remove-student-relationship?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostParentSearchStudent(int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/search-student?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostParentJoinClub(int parent_id, int club_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("parent_id", parent_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/join-club?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostParentClubRelationship(int parent_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("parent_id", parent_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/club-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostParentSearchStaff(int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/search-staff?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostParentSearchStaffWallet(int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/parent/search-staff-wallet?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Student
        public async Task<string> PostStudentProfile(int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/profile", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostStudentJoinClub(int student_id, int club_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("student_id", student_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/join-club?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostStudentClubRelationship(int school_id, int student_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/club-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentMonthlyAttendanceSummary(int school_id, int class_id, int student_id, string entry_month)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("class_id", class_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("entry_month", entry_month)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/monthly-attendance-summary", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentMonthlyAttendance(int school_id, int class_id, int student_id, string entry_month)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("class_id", class_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("entry_month", entry_month)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/monthly-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostStudentDailyAttendance(int school_id, int class_id, int student_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("class_id", class_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/daily-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentOutingRequestMonthGroup(int student_id, int school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/outing-request-month-group", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentOutingRequestMonth(int student_id, int school_id, DateTime outing_month)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("outing_month", outing_month.ToString("MM-yyyy"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/outing-request-month", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentSaveOutingRequest(int student_id, int school_id, int outing_type_id, DateTime check_out_date,
            DateTime check_in_date, string outing_reason, int request_by_id, int request_by_user_role_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("outing_type_id", outing_type_id.ToString()),
                         new KeyValuePair<string, string>("check_out_date", check_out_date.ToString("yyyy/MM/dd HH:mm:ss")),
                         new KeyValuePair<string, string>("check_in_date", check_in_date.ToString("yyyy/MM/dd HH:mm:ss")),
                         new KeyValuePair<string, string>("outing_reason", outing_reason),
                         new KeyValuePair<string, string>("request_by_id", request_by_id.ToString()),
                         new KeyValuePair<string, string>("request_by_user_role_id", request_by_user_role_id.ToString()),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/save-outing-request", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentSubmitOutingRequest(int outing_id, int student_id, int school_id, int outing_type_id, DateTime check_out_date,
            DateTime check_in_date, string outing_reason, int request_by_id, int request_by_user_role_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("outing_id", outing_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("outing_type_id", outing_type_id.ToString()),
                         new KeyValuePair<string, string>("check_out_date", check_out_date.ToString("yyyy/MM/dd HH:mm:ss")),
                         new KeyValuePair<string, string>("check_in_date", check_in_date.ToString("yyyy/MM/dd HH:mm:ss")),
                         new KeyValuePair<string, string>("outing_reason", outing_reason),
                         new KeyValuePair<string, string>("request_by_id", request_by_id.ToString()),
                         new KeyValuePair<string, string>("request_by_user_role_id", request_by_user_role_id.ToString()),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/submit-outing-request", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentUpdateOutingRequest(int outing_id, int student_id, int school_id, int outing_type_id, DateTime check_out_date,
            DateTime check_in_date, string outing_reason, int request_by_id, int request_by_user_role_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("outing_id", outing_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("outing_type_id", outing_type_id.ToString()),
                         new KeyValuePair<string, string>("check_out_date", check_out_date.ToString("yyyy/MM/dd HH:mm:ss")),
                         new KeyValuePair<string, string>("check_in_date", check_in_date.ToString("yyyy/MM/dd HH:mm:ss")),
                         new KeyValuePair<string, string>("outing_reason", outing_reason),
                         new KeyValuePair<string, string>("request_by_id", request_by_id.ToString()),
                         new KeyValuePair<string, string>("request_by_user_role_id", request_by_user_role_id.ToString()),
                         new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/update-outing-request", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentCancelOutingRequest(int outing_id, int student_id, int school_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("outing_id", outing_id.ToString()),
                         new KeyValuePair<string, string>("student_id", student_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/cancel-outing-request", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostStudentParentRelationship(int student_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("student_id", student_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/student/parent-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Merchant
        public async Task<string> PostMerchantProfile(int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/profile", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostMerchantSchoolRelationship(int merchant_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/school-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostMerchantJoinClub(int merchant_id, int club_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/join-club", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostMerchantClubRelationship(int merchant_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/club-relationship", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantTerminal(int school_id, int merchant_id, string receipt_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("receipt_date", receipt_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/terminal", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantTerminalReceipt(int school_id, int merchant_id, int terminal_id, string receipt_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("terminal_id", terminal_id.ToString()),
                         new KeyValuePair<string, string>("receipt_date", receipt_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/terminal-receipt", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantTerminalReceiptDetail(int rcpt_id, string wallet_number, string reference_number)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("rcpt_id", rcpt_id.ToString()),
                         new KeyValuePair<string, string>("wallet_number", wallet_number),
                         new KeyValuePair<string, string>("reference_number", reference_number)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/terminal-receipt-detail", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantProductCategory(int school_id, int merchant_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/product-category", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantCreateProductCategory(int school_id, int merchant_id, string category_name, string category_description, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("category_name", category_name),
                        new KeyValuePair<string, string>("category_description", category_description),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/create-product-category?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantUpdateProductCategory(int school_id, int merchant_id, int category_id,string category_name, string category_description, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("category_id", category_id.ToString()),
                        new KeyValuePair<string, string>("category_name", category_name),
                        new KeyValuePair<string, string>("category_description", category_description),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/update-product-category?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantDeleteProductCategory(int school_id, int merchant_id, int category_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("category_id", category_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/delete-product-category?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantProductDetail(int school_id, int merchant_id, int category_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("category_id", category_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/product-detail", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantCreateProductDetail(int merchant_id, int category_id, string product_name, string product_sku, string photo_base64, string file_name,
            decimal unit_price, decimal cost_price, decimal discount_price, string product_description, string product_weight, string product_ingredient, string special_flag, 
            string available_day,string text_color, string background_color, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("merchant_id", merchant_id);
                    obj.Add("category_id", category_id);
                    obj.Add("product_name", product_name);
                    obj.Add("product_sku", product_sku);
                    obj.Add("photo_base64", photo_base64);
                    obj.Add("file_name", file_name);
                    obj.Add("unit_price", unit_price);
                    obj.Add("cost_price", cost_price);
                    obj.Add("discount_price", discount_price);
                    obj.Add("product_description", product_description);
                    obj.Add("product_weight", product_weight);
                    obj.Add("product_ingredient", product_ingredient);
                    obj.Add("special_flag", special_flag);
                    obj.Add("available_day", available_day);
                    obj.Add("text_color", text_color);
                    obj.Add("background_color", background_color);
                    obj.Add("create_by", create_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/create-product-detail?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantUpdateProductDetail(int product_id, int merchant_id, int category_id, string product_name, string product_sku, string photo_base64, string file_name,
            decimal unit_price, decimal cost_price, decimal discount_price, string product_description, string product_weight, string product_ingredient, string special_flag, string available_day, 
            string text_color, string background_color, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);


                    JObject obj = new JObject();
                    obj.Add("product_id", product_id);
                    obj.Add("merchant_id", merchant_id);
                    obj.Add("category_id", category_id);
                    obj.Add("product_name", product_name);
                    obj.Add("product_sku", product_sku);
                    obj.Add("photo_base64", photo_base64);
                    obj.Add("file_name", file_name);
                    obj.Add("unit_price", unit_price);
                    obj.Add("cost_price", cost_price);
                    obj.Add("discount_price", discount_price);
                    obj.Add("product_description", product_description);
                    obj.Add("product_weight", product_weight);
                    obj.Add("product_ingredient", product_ingredient);
                    obj.Add("special_flag", special_flag);
                    obj.Add("available_day", available_day);
                    obj.Add("text_color", text_color);
                    obj.Add("background_color", background_color);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/update-product-detail?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantRemoveProductPhoto(int product_id, int merchant_id, int category_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);


                    JObject obj = new JObject();
                    obj.Add("product_id", product_id);
                    obj.Add("merchant_id", merchant_id);
                    obj.Add("category_id", category_id);
                    obj.Add("update_by", update_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/remove-product-photo?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantDeleteProductDetail(int product_id, int merchant_id, int category_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("category_id", category_id.ToString()),
                        new KeyValuePair<string, string>("product_id", product_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/delete-product-detail?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantProductNutrition(int school_id, int merchant_id, int product_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("product_id", product_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/product-nutrition-info", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostMerchantCreateProductNutrition(int product_id, string nutrition_name, string per_serving, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("product_id", product_id.ToString()),
                         new KeyValuePair<string, string>("nutrition_name", nutrition_name.ToString()),
                         new KeyValuePair<string, string>("per_serving", per_serving),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/create-product-nutrition", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostMerchantUpdateProductNutrition(int info_id, int product_id, string nutrition_name, string per_serving, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("info_id", info_id.ToString()),
                        new KeyValuePair<string, string>("product_id", product_id.ToString()),
                         new KeyValuePair<string, string>("nutrition_name", nutrition_name.ToString()),
                         new KeyValuePair<string, string>("per_serving", per_serving),
                         new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/update-product-nutrition", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostMerchantDeleteProductNutrition(int info_id, int product_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("info_id", info_id.ToString()),
                        new KeyValuePair<string, string>("product_id", product_id.ToString()),
                         new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/delete-product-nutrition", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostMerchantOrderHistory(int school_id, int merchant_id, DateTime pickup_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/order-history", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostMerchantOrderHistoryGroup(int school_id, int merchant_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/order-history-group", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantSettlementGroup(int merchant_id, int school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/settlement-group", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantSettlement(int merchant_id, int school_id, DateTime receipt_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("receipt_date", receipt_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/settlement", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantStudentOrderHistory(int merchant_id, int school_id, int class_id, DateTime pickup_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("class_id", class_id.ToString()),
                         new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/student-order-history", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostMerchantStudentOrderHistoryDetail(int school_id, int class_id, DateTime pickup_date, int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("class_id", class_id.ToString()),
                         new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd")),
                         new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/student-order-history-detail", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostMerchantProductOrderHistory(int merchant_id, int school_id, int class_id, DateTime pickup_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/product-order-history", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostMerchantProductOrderHistoryDetail(int merchant_id, int school_id, int class_id, DateTime pickup_date, int product_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("class_id", class_id.ToString()),
                         new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd")),
                         new KeyValuePair<string, string>("product_id", product_id.ToString()),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/product-order-history-detail", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantProductOrderHistoryStaff(int order_id, int school_id, DateTime pickup_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("order_id", order_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/product-order-history-staff", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantSales(int merchant_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/sales", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantSalesMethod(int merchant_id, DateTime receipt_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("receipt_date", receipt_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/sales-method", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostMerchantUpdateOrderStatus(int school_id, int merchant_id, string order_id, int order_status_id, string update_by, DateTime pickup_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("order_id", order_id),
                         new KeyValuePair<string, string>("order_status_id", order_status_id.ToString()),
                         new KeyValuePair<string, string>("update_by", update_by),
                         new KeyValuePair<string, string>("order_date", pickup_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/merchant/update-order-status?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Lookup
        public async Task<string> PostLookupPlatformVersion(string platform)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("platform", platform)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/platform-version", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostLookupState()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/state", null);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostLookupCity(int state_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("state_id", state_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/city", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostLookupCountry()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/country", null);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostLookupAttendance()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/attendance", null);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostLookupReasonForAbsent()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/reason-for-absent", null);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostLookupCardType()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/card-types", null);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostLookupProblemType()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/problem-types", null);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostLookupUserRace()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/user-race", null);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostLookupOccupation()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(requestUrl + "/api/v2/lookup/occupation", null);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region School
        public async Task<string> PostSchoolPost(int[] school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

                    for (int i = 0; i < school_id.Length; i++)
                    {
                        keyValues.Add(new KeyValuePair<string, string>("school_id[]", school_id[i].ToString()));
                    }

                    var content = new FormUrlEncodedContent(keyValues.Select(s =>
                        new KeyValuePair<string, string>(s.Key, s.Value)
                    ));

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/post?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClassPost(int school_id, int class_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/class-post?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClubPost(int school_id, int club_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/club-post?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClubMember(int school_id, int club_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/club-member?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolInfo(int school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/info", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostSearchSchool(int state_id, string school_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("state_id", state_id.ToString()),
                        new KeyValuePair<string, string>("school_name", school_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/search?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostSchoolClass(int school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/class", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClub(int []school_id, int create_by_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);


                    List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

                    for (int i = 0; i < school_id.Length; i++)
                    {
                        keyValues.Add(new KeyValuePair<string, string>("school_id[]", school_id[i].ToString()));
                    }
                    keyValues.Add(new KeyValuePair<string, string>("create_by_id", create_by_id.ToString()));

                    var content = new FormUrlEncodedContent(keyValues.Select(s =>
                        new KeyValuePair<string, string>(s.Key, s.Value)
                    ));

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/club", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostSchoolClassStudent(int school_id, int class_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/class-student?limit=0&offset=0", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolStaff(int school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/staff", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostSchoolMerchant(int school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/merchant", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostSchoolClassDailyAttendanceSummary(int school_id, int class_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/class-daily-attendance-summary", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClubDailyAttendanceSummary(int school_id, int club_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/club-daily-attendance-summary", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClubMemberMonthlyAttendance(int school_id, int club_id, int profile_id, string entry_month)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("entry_month", entry_month)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/club-member-monthly-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClubMemberDailyAttendance(int school_id, int club_id, int profile_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("club_id", club_id.ToString()),
                         new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                         new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/club-member-daily-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolStaffDailyAttendanceSummary(int school_id, int shift_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("shift_id", shift_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/staff-daily-attendance-summary", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClassDailyAttendancePercentage(int school_id, int class_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/class-daily-attendance-percentage", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClubDailyAttendancePercentage(int school_id, int club_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/club-daily-attendance-percentage", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolStaffDailyAttendancePercentage(int school_id, int shift_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("shift_id", shift_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/staff-daily-attendance-percentage", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClassDailyAttendance(int school_id, int class_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/class-daily-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolClubDailyAttendance(int school_id, int club_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/club-daily-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostSchoolStaffDailyAttendance(int school_id, int shift_id, string entry_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("shift_id", shift_id.ToString()),
                        new KeyValuePair<string, string>("entry_date", entry_date)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/school/staff-daily-attendance", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Wallet
        public async Task<string> PostWalletTransactionHistory(string wallet_number)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_number", wallet_number)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/wallet/transaction-history", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostWalletTransactionReference(int transaction_id, string reference_number)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("transaction_id", transaction_id.ToString()),
                        new KeyValuePair<string, string>("reference_number", reference_number)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/wallet/transaction-reference", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostWalletTransactionDetail(string wallet_number, string reference_number)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_number", wallet_number),
                        new KeyValuePair<string, string>("reference_number", reference_number)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/wallet/transaction-detail", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostWalletTransactionMaster(string wallet_number, string reference_number)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_number", wallet_number),
                        new KeyValuePair<string, string>("reference_number", reference_number)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/wallet/transaction-master", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostWalletTopup(int wallet_id, string wallet_number, decimal topup_amount, string topup_date, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("wallet_number", wallet_number),
                        new KeyValuePair<string, string>("topup_amount", topup_amount.ToString()),
                        new KeyValuePair<string, string>("topup_date", topup_date),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/wallet/topup?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountTopup(int wallet_id, string wallet_number, decimal topup_amount, string topup_date, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("wallet_number", wallet_number),
                        new KeyValuePair<string, string>("topup_amount", topup_amount.ToString()),
                        new KeyValuePair<string, string>("topup_date", topup_date),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/topup?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostWalletTransfer(int wallet_id, string wallet_number, int recipient_id, string recipient_wallet, decimal transfer_amount, string transfer_date, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;
                    // Get IP Address
                    string ip_address = DependencyService.Get<IIPAddressManager>().GetIPAddress();

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("wallet_number", wallet_number),
                        new KeyValuePair<string, string>("recipient_id", recipient_id.ToString()),
                        new KeyValuePair<string, string>("recipient_wallet", recipient_wallet),
                        new KeyValuePair<string, string>("transfer_amount", transfer_amount.ToString()),
                        new KeyValuePair<string, string>("transfer_date", transfer_date),
                        new KeyValuePair<string, string>("ip_address", ip_address),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/transfer?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountCardInfo(string username)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/card-info?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountTopupStatus(string username, string reference_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("reference_id", reference_id),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/topup-status?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountRegisterCheck(string username, int user_role_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/register-check?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        #endregion

        #region Card
        public async Task<string> PostCardDailyLimit(int card_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("card_id", card_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/card/daily-limit", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostCardUpdateDailyLimit(int card_id, int school_id, decimal daily_limit, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("card_id", card_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("daily_limit", daily_limit.ToString("F")),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/card/update-daily-limit?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostCardUpdateStatusBlacklist(int card_id, int school_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("card_id", card_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/card/update-status-blacklist?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostCardReplacement(int user_role_id,int school_id, int old_card_id,int old_card_status_id, string new_card_number, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("old_card_id", old_card_id.ToString()),
                        new KeyValuePair<string, string>("old_card_status_id", old_card_status_id.ToString()),
                        new KeyValuePair<string, string>("new_card_number", new_card_number),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/card/replacement?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostCardSearchAssignment(int user_role_id, int school_id, string search_name)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("search_name", search_name)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/card/search-assignment", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region General
        public async Task<string> PostGeneralLeaveClub(int relationship_id, int profile_id, int club_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("relationship_id", relationship_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("club_id", club_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/general/leave-club?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Purchase

        public async Task<string> PostPurchaseProductCategory(int school_id, int merchant_id, string special_flag)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("special_flag", special_flag)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/product-category", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseProductDetail(int school_id, int merchant_id, int category_id, string special_flag, string available_day)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("category_id", category_id.ToString()),
                         new KeyValuePair<string, string>("special_flag", special_flag),
                         new KeyValuePair<string, string>("available_day", available_day)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/product-detail", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseInsertCart(int profile_id, int wallet_id, int merchant_id, int school_id, int user_role_id, int recipient_id,
            int recipient_role_id, DateTime pickup_date, TimeSpan pickup_time, int service_method_id, string delivery_location, int product_id, int product_qty, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    DateTime dt = pickup_date.Add(pickup_time);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString()),
                        new KeyValuePair<string, string>("recipient_id", recipient_id.ToString()),
                        new KeyValuePair<string, string>("recipient_role_id", recipient_role_id.ToString()),
                        new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("pickup_time", dt.ToString("HH:mm:ss")),
                        new KeyValuePair<string, string>("service_method_id", service_method_id.ToString()),
                        new KeyValuePair<string, string>("delivery_location", delivery_location),
                        new KeyValuePair<string, string>("product_id", product_id.ToString()),
                        new KeyValuePair<string, string>("product_qty", product_qty.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/insert-cart", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseUpdateCart(int cart_id, int profile_id, int wallet_id, int merchant_id, int school_id, int recipient_id,
            DateTime pickup_date, int product_id, int product_qty, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("cart_id", cart_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("recipient_id", recipient_id.ToString()),
                        new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, string>("product_id", product_id.ToString()),
                        new KeyValuePair<string, string>("product_qty", product_qty.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/update-cart", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostPurchaseDeleteCart(int cart_id, int profile_id, int recipient_id, int product_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("cart_id", cart_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("recipient_id", recipient_id.ToString()),
                        new KeyValuePair<string, string>("product_id", product_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/delete-cart", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        
        public async Task<string> PostPurchaseCartCount(int profile_id, int wallet_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/cart-count", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseCartTotal(int profile_id, int wallet_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/cart-total", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseOrderHistoryTotal(string wallet_number, string reference_number)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_number", wallet_number),
                        new KeyValuePair<string, string>("reference_number", reference_number)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/order-history-total", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseCart(int profile_id, int wallet_id, int order_status_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("order_status_id", order_status_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/cart", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseCartGroupDate(int profile_id, int wallet_id, int order_status_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("order_status_id", order_status_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/cart-pickup-date-group", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseCartPickupDate(int profile_id, int wallet_id, int order_status_id, DateTime pickup_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("order_status_id", order_status_id.ToString()),
                        new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/cart-pickup-date", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchasePlaceOrder(int profile_id, int wallet_id, int order_status_id, int user_role_id, decimal sub_total_amount, decimal tax_amount, decimal total_amount, int payment_method_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("order_status_id", order_status_id.ToString()),
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString()),
                        new KeyValuePair<string, string>("sub_total_amount", sub_total_amount.ToString()),
                        new KeyValuePair<string, string>("tax_amount", tax_amount.ToString()),
                        new KeyValuePair<string, string>("total_amount", total_amount.ToString()),
                        new KeyValuePair<string, string>("payment_method_id", payment_method_id.ToString()),            
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/mpay/purchase?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseOrderHistory(int profile_id, int wallet_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/order-history", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseDetailOrderHistory(int order_id, int wallet_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("order_id", order_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/detail-order-history", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseOrderHistoryDetailGroupDate(string wallet_number, string reference_number)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_number", wallet_number),
                        new KeyValuePair<string, string>("reference_number", reference_number)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/detail-order-history-date-group", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseOrderHistoryDetailDate(string wallet_number, string reference_number, DateTime pickup_date)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("wallet_number", wallet_number),
                        new KeyValuePair<string, string>("reference_number", reference_number),
                        new KeyValuePair<string, string>("pickup_date", pickup_date.ToString("yyyy-MM-dd"))
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/detail-order-history-date", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseRemoveOrderMaster(int order_id, int profile_id, int wallet_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("order_id", order_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/remove-order?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostPurchaseContinueOrderMaster(int order_id, int profile_id, int wallet_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("order_id", order_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_id", wallet_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/purchase/continue-order?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Contact
        public async Task<string> PostContactClassTeacher(int parent_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("parent_id", parent_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/contact/class-teacher", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostContactStudentClass(int parent_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("parent_id", parent_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/contact/student-class", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostContactStudentParentStaffMerchant(int school_id, int[] class_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

                    keyValues.Add(new KeyValuePair<string, string>("school_id", school_id.ToString()));

                    for (int i = 0; i < class_id.Length; i++)
                    {
                        keyValues.Add(new KeyValuePair<string, string>("class_id[]", class_id[i].ToString()));
                    }

                    var content = new FormUrlEncodedContent(keyValues.Select(s =>
                        new KeyValuePair<string, string>(s.Key, s.Value)
                    ));

                    var response = await client.PostAsync(requestUrl + "/api/v2/contact/student-parent-staff-merchant", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostContactSchoolStaffMerchant(int school_id, int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/contact/school-staff-merchant", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostContactSchoolStaff(int[] school_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

                    for (int i = 0; i < school_id.Length; i++)
                    {
                        keyValues.Add(new KeyValuePair<string, string>("school_id[]", school_id[i].ToString()));
                    }

                    var content = new FormUrlEncodedContent(keyValues.Select(s =>
                        new KeyValuePair<string, string>(s.Key, s.Value)
                    ));

                    var response = await client.PostAsync(requestUrl + "/api/v2/contact/school-staff", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Report
        public async Task<string> PostReportClassAttendance(int school_id, int class_id, DateTime entry_month, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("class_id", class_id.ToString()),
                         new KeyValuePair<string, string>("entry_month", entry_month.ToString("yyyy-MM-dd")),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/report/class-attendance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        public async Task<string> PostReportStaffAttendance(int school_id, int shift_id, DateTime entry_month, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("shift_id", shift_id.ToString()),
                         new KeyValuePair<string, string>("entry_month", entry_month.ToString("yyyy-MM-dd")),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/report/staff-attendance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostReportClassOrder(int merchant_id, int school_id, DateTime pickup_date, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("order_date", pickup_date.ToString("yyyy-MM-dd")),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/report/class-daily-order?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostReportStaffOrder(int merchant_id, int school_id, DateTime pickup_date, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("order_date", pickup_date.ToString("yyyy-MM-dd")),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/report/staff-daily-order?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostReportProductOrder(int merchant_id, int school_id, DateTime pickup_date, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("order_date", pickup_date.ToString("yyyy-MM-dd")),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/report/product-daily-order?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostReportStudentOrder(int merchant_id, int school_id, int class_id, DateTime pickup_date, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("merchant_id", merchant_id.ToString()),
                         new KeyValuePair<string, string>("school_id", school_id.ToString()),
                         new KeyValuePair<string, string>("class_id", class_id.ToString()),
                         new KeyValuePair<string, string>("order_date", pickup_date.ToString("yyyy-MM-dd")),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/report/student-daily-order?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostReportCustomerFeedback(int support_category_id, int problem_type_id, int priority_type_id, string ticket_subject, string ticket_desc, int ticket_status_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("support_category_id", support_category_id.ToString()),
                         new KeyValuePair<string, string>("problem_type_id", problem_type_id.ToString()),
                         new KeyValuePair<string, string>("priority_type_id", priority_type_id.ToString()),
                         new KeyValuePair<string, string>("ticket_subject", ticket_subject),
                         new KeyValuePair<string, string>("ticket_desc", ticket_desc),
                         new KeyValuePair<string, string>("ticket_status_id", ticket_status_id.ToString()),
                         new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/report/customer-feedback?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostReportUploadAttachment(int ticket_id, string file_name, string photo_base64, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    JObject obj = new JObject();

                    obj.Add("ticket_id", ticket_id);
                    obj.Add("file_name", file_name);
                    obj.Add("photo_base64", photo_base64);
                    obj.Add("create_by", create_by);

                    var jsonStr = JsonConvert.SerializeObject(obj); //JSON encoded
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUrl + "/api/v2/report/upload-attachment?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Chat
        public async Task<string> PostChatHistory(int profile_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/chat/history", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostChatUserStatus(string channel_id, int profile_id, int status_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    //string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("channel_id", channel_id),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("status_id", status_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/chat/user-status?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostChatJoinChannel(string channel_id, int profile_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    //string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("channel_id", channel_id),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/chat/join-channel?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostChatLeaveChannel(string channel_id, int profile_id, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    //string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("channel_id", channel_id),
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("update_by", update_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/chat/leave-channel?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostChatChannelUser(string channel_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    //string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("channel_id", channel_id)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/chat/channel-user", content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostChatSend(string channel_id, int channel_type_id, string channel_name, int sender_id, 
            int receiver_id, int message_type_id, string message, string photo_base64, string file_name, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    //string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("channel_id", channel_id),
                        new KeyValuePair<string, string>("channel_type_id", channel_type_id.ToString()),
                        new KeyValuePair<string, string>("channel_name", channel_name),
                        new KeyValuePair<string, string>("sender_id", sender_id.ToString()),
                        new KeyValuePair<string, string>("receiver_id", receiver_id.ToString()),
                        new KeyValuePair<string, string>("message_type_id", message_type_id.ToString()),
                        new KeyValuePair<string, string>("message", message),
                        new KeyValuePair<string, string>("photo_base64", photo_base64),
                        new KeyValuePair<string, string>("file_name", file_name),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/chat/send?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

        #region Account
        public async Task<string> PostAccountRegister(int user_role_id, string full_name, int card_type_id, int nationality_id,
            string date_of_birth, string identity_number, string mobile_number, string email, string password,int marketing_flag)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString()),
                        new KeyValuePair<string, string>("full_name", full_name),
                        new KeyValuePair<string, string>("id_type", card_type_id.ToString()),
                        new KeyValuePair<string, string>("nationality_id", nationality_id.ToString()),
                        new KeyValuePair<string, string>("date_of_birth", date_of_birth),
                        new KeyValuePair<string, string>("id_number", identity_number),
                        new KeyValuePair<string, string>("mobile_number", mobile_number),
                        new KeyValuePair<string, string>("email", email),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("marketing_flag", marketing_flag.ToString()),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/register?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        //public async Task<string> PostAccountVerify(string account_id, string otp)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Initialization  
        //            string authorization = Settings.accessToken;

        //            // Setting Authorization.  
        //            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

        //            var content = new FormUrlEncodedContent(new[]
        //            {
        //                new KeyValuePair<string, string>("account_id", account_id.ToString()),
        //                new KeyValuePair<string, string>("otp", otp),
        //            });

        //            var response = await client.PostAsync(requestUrl + "/api/v2/account/verify?culture=" + Settings.cultureInfo, content);
        //            string result = await response.Content.ReadAsStringAsync();
        //            if (response.StatusCode != HttpStatusCode.OK)
        //            {
        //                return result = ResponseMessage(response.StatusCode);
        //            }
        //            return result;
        //        }
        //        catch (WebException ex)
        //        {
        //            if (ex.Response != null)
        //            {
        //                string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
        //                throw new System.Exception($"response :{responseContent}", ex);
        //            }
        //            throw;
        //        }
        //    }
        //}

        public async Task<string> PostAccountVerify(string mobile_number, string username, string otp)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("mobile_number", mobile_number),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("otp", otp)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/verify?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountStatus(int profile_id, string wallet_number)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("wallet_number", wallet_number)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/status?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountAddVirtualBalance(int parent_id, int student_id, int school_id, int class_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("parent_id", parent_id.ToString()),
                        new KeyValuePair<string, string>("student_id", student_id.ToString()),
                        new KeyValuePair<string, string>("school_id", school_id.ToString()),
                        new KeyValuePair<string, string>("class_id", class_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/add-virtual-balance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountUpdateVirtualBalance(int parent_profile_id, int student_profile_id, string create_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("parent_profile_id", parent_profile_id.ToString()),
                        new KeyValuePair<string, string>("student_profile_id", student_profile_id.ToString()),
                        new KeyValuePair<string, string>("create_by", create_by)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/update-virtual-balance?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountResubmitKYC(string username)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/resubmit-kyc?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountVerifyKYC(string username)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("username", username)
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/verify-kyc?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountUpdateProfile(int profile_id, string full_name, string nric,
            string address, string postcode, string city, int state_id, int country_id,
            string mother_maiden_name, string occupation, string employer_name, string update_by)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("full_name", full_name),
                        new KeyValuePair<string, string>("nric", nric),
                        new KeyValuePair<string, string>("address", address),
                        new KeyValuePair<string, string>("postcode", postcode),
                        new KeyValuePair<string, string>("city", city),
                        new KeyValuePair<string, string>("state_id", state_id.ToString()),
                        new KeyValuePair<string, string>("country_id", country_id.ToString()),
                        new KeyValuePair<string, string>("mother_maiden_name", mother_maiden_name),
                        new KeyValuePair<string, string>("occupation", occupation),
                        new KeyValuePair<string, string>("employer_name", employer_name),
                        new KeyValuePair<string, string>("update_by", update_by),
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/update-profile?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }

        public async Task<string> PostAccountInfo(int profile_id, int user_role_id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Initialization  
                    string authorization = Settings.accessToken;

                    // Setting Authorization.  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("profile_id", profile_id.ToString()),
                        new KeyValuePair<string, string>("user_role_id", user_role_id.ToString())
                    });

                    var response = await client.PostAsync(requestUrl + "/api/v2/account/info?culture=" + Settings.cultureInfo, content);
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return result = ResponseMessage(response.StatusCode);
                    }
                    return result;
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new System.Exception($"response :{responseContent}", ex);
                    }
                    throw;
                }
            }
        }
        #endregion

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
                    GetLoginPage();
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

        public static Page GetLoginPage()
        {
            App.Current.MainPage = new NavigationPage(new LoginPage1())
            {
                BarBackgroundColor = Color.FromHex("#5E625A"),
                BarTextColor = Color.FromHex("#FFD612")
            };
            return App.Current.MainPage;
        }
    }
}
