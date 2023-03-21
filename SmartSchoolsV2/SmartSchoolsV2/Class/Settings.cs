using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace SmartSchoolsV2.Class
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string nodePath
        {
            get => AppSettings.GetValueOrDefault(nameof(nodePath), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(nodePath), value);
        }
        public static string topic
        {
            get => AppSettings.GetValueOrDefault(nameof(topic), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(topic), value);
        }

        public static string channelTopName
        {
            get => AppSettings.GetValueOrDefault(nameof(channelTopName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(channelTopName), value);
        }
        public static string channelBtmName
        {
            get => AppSettings.GetValueOrDefault(nameof(channelBtmName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(channelBtmName), value);
        }
        public static string channelPhotoUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(channelPhotoUrl), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(channelPhotoUrl), value);
        }

        public static int channelTypeId
        {
            get => AppSettings.GetValueOrDefault(nameof(channelTypeId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(channelTypeId), value);
        }

        public static string channelId
        {
            get => AppSettings.GetValueOrDefault(nameof(channelId), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(channelId), value);
        }
        public static int firstId
        {
            get => AppSettings.GetValueOrDefault(nameof(firstId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(firstId), value);
        }
        public static int secondId
        {
            get => AppSettings.GetValueOrDefault(nameof(secondId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(secondId), value);
        }

        public static string requestUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(requestUrl), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(requestUrl), value);
        }

        public static string androidPackageName
        {
            get => AppSettings.GetValueOrDefault(nameof(androidPackageName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(androidPackageName), value);
        }
        public static string currentVersion
        {
            get => AppSettings.GetValueOrDefault(nameof(currentVersion), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(currentVersion), value);
        }

        public static string currentBuild
        {
            get => AppSettings.GetValueOrDefault(nameof(currentBuild), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(currentBuild), value);
        }
        public static string devicePlatform
        {
            get => AppSettings.GetValueOrDefault(nameof(devicePlatform), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(devicePlatform), value);
        }

        public static string cartMode
        {
            get => AppSettings.GetValueOrDefault(nameof(cartMode), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(cartMode), value);
        }

        public static string iOSApplicationId
        {
            get => AppSettings.GetValueOrDefault(nameof(iOSApplicationId), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(iOSApplicationId), value);
        }

        public static string uwpProductId
        {
            get => AppSettings.GetValueOrDefault(nameof(uwpProductId), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(uwpProductId), value);
        }
        public static string cultureInfo
        {
            get => AppSettings.GetValueOrDefault(nameof(cultureInfo), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(cultureInfo), value);
        }
        public static bool isLogin
        {
            get => AppSettings.GetValueOrDefault(nameof(isLogin), false);

            set => AppSettings.AddOrUpdateValue(nameof(isLogin), value);
        }
        public static bool isFirstTime
        {
            get => AppSettings.GetValueOrDefault(nameof(isFirstTime), true);

            set => AppSettings.AddOrUpdateValue(nameof(isFirstTime), value);
        }
        public static int profileId
        {
            get => AppSettings.GetValueOrDefault(nameof(profileId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(profileId), value);
        }
        public static string userName
        {
            get => AppSettings.GetValueOrDefault(nameof(userName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(userName), value);

        }

        public static int rememberMe
        {
            get => AppSettings.GetValueOrDefault(nameof(rememberMe), 0);

            set => AppSettings.AddOrUpdateValue(nameof(rememberMe), value);
        }

        public static string deviceToken
        {
            get => AppSettings.GetValueOrDefault(nameof(deviceToken), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(deviceToken), value);

        }

        public static string accessToken
        {
            get => AppSettings.GetValueOrDefault(nameof(accessToken), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(accessToken), value);
        }

        public static string lastLogin
        {
            get => AppSettings.GetValueOrDefault(nameof(lastLogin), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(lastLogin), value);
        }

        public static string topupRefId
        {
            get => AppSettings.GetValueOrDefault(nameof(topupRefId), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(topupRefId), value);
        }
        public static string fullName
        {
            get => AppSettings.GetValueOrDefault(nameof(fullName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(fullName), value);

        }

        public static string photoUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(photoUrl), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(photoUrl), value);

        }
        public static int userRoleId
        {
            get => AppSettings.GetValueOrDefault(nameof(userRoleId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(userRoleId), value);
        }
        public static int parentId
        {
            get => AppSettings.GetValueOrDefault(nameof(parentId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(parentId), value);
        }
        public static int staffId
        {
            get => AppSettings.GetValueOrDefault(nameof(staffId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(staffId), value);
        }
        public static int staffTypeId
        {
            get => AppSettings.GetValueOrDefault(nameof(staffTypeId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(staffTypeId), value);
        }
        public static int merchantId
        {
            get => AppSettings.GetValueOrDefault(nameof(merchantId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(merchantId), value);
        }

        public static int merchantTypeId
        {
            get => AppSettings.GetValueOrDefault(nameof(merchantTypeId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(merchantTypeId), value);
        }
        public static int merchantSchoolId
        {
            get => AppSettings.GetValueOrDefault(nameof(merchantSchoolId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(merchantSchoolId), value);
        }

        public static string merchantSchoolName
        {
            get => AppSettings.GetValueOrDefault(nameof(merchantSchoolName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(merchantSchoolName), value);
        }
        public static int schoolId
        {
            get => AppSettings.GetValueOrDefault(nameof(schoolId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(schoolId), value);
        }

        public static string schoolName
        {
            get => AppSettings.GetValueOrDefault(nameof(schoolName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(schoolName), value);
        }
        public static string schoolCode
        {
            get => AppSettings.GetValueOrDefault(nameof(schoolCode), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(schoolCode), value);
        }
        public static int selectedSchoolId
        {
            get => AppSettings.GetValueOrDefault(nameof(selectedSchoolId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(selectedSchoolId), value);
        }
        public static string selectedSchoolName
        {
            get => AppSettings.GetValueOrDefault(nameof(selectedSchoolName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(selectedSchoolName), value);
        }

        public static int cityId
        {
            get => AppSettings.GetValueOrDefault(nameof(cityId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(cityId), value);
        }

        public static string cityName
        {
            get => AppSettings.GetValueOrDefault(nameof(cityName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(cityName), value);
        }
        public static int selectedClubId
        {
            get => AppSettings.GetValueOrDefault(nameof(selectedClubId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(selectedClubId), value);
        }

        public static int selectedClassId
        {
            get => AppSettings.GetValueOrDefault(nameof(selectedClassId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(selectedClassId), value);
        }

        public static string selectedClubName
        {
            get => AppSettings.GetValueOrDefault(nameof(selectedClubName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(selectedClubName), value);
        }

        public static string selectedClassName
        {
            get => AppSettings.GetValueOrDefault(nameof(selectedClassName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(selectedClassName), value);
        }

        public static int createByStaffId
        {
            get => AppSettings.GetValueOrDefault(nameof(createByStaffId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(createByStaffId), value);
        }
        public static bool studentClub
        {
            get => AppSettings.GetValueOrDefault(nameof(studentClub), false);

            set => AppSettings.AddOrUpdateValue(nameof(studentClub), value);
        }
        public static int classId
        {
            get => AppSettings.GetValueOrDefault(nameof(classId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(classId), value);
        }

        public static int clubId
        {
            get => AppSettings.GetValueOrDefault(nameof(clubId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(clubId), value);
        }
        public static string clubName
        {
            get => AppSettings.GetValueOrDefault(nameof(clubName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(clubName), value);
        }

        public static string clubCreator
        {
            get => AppSettings.GetValueOrDefault(nameof(clubCreator), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(clubCreator), value);
        }

        public static int assignProfileId
        {
            get => AppSettings.GetValueOrDefault(nameof(assignProfileId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(assignProfileId), value);
        }
        public static int selectMerchantId
        {
            get => AppSettings.GetValueOrDefault(nameof(selectMerchantId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(selectMerchantId), value);
        }

        public static int selectMerchantTypeId
        {
            get => AppSettings.GetValueOrDefault(nameof(selectMerchantTypeId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(selectMerchantTypeId), value);
        }

        public static string selectCompanyName
        {
            get => AppSettings.GetValueOrDefault(nameof(selectCompanyName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(selectCompanyName), value);
        }
        public static string selectMerchantType
        {
            get => AppSettings.GetValueOrDefault(nameof(selectMerchantType), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(selectMerchantType), value);
        }
        public static int assignCardId
        {
            get => AppSettings.GetValueOrDefault(nameof(assignCardId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(assignCardId), value);
        }
        public static string assignCardNumber
        {
            get => AppSettings.GetValueOrDefault(nameof(assignCardNumber), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(assignCardNumber), value);
        }

        public static int assignCardStatusId
        {
            get => AppSettings.GetValueOrDefault(nameof(assignCardStatusId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(assignCardStatusId), value);
        }

        public static string assignCardStatus
        {
            get => AppSettings.GetValueOrDefault(nameof(assignCardStatus), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(assignCardStatus), value);
        }

        public static string assignPhotoUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(assignPhotoUrl), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(assignPhotoUrl), value);
        }

        public static string assignFullName
        {
            get => AppSettings.GetValueOrDefault(nameof(assignFullName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(assignFullName), value);
        }

        public static string className
        {
            get => AppSettings.GetValueOrDefault(nameof(className), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(className), value);
        }

        public static string sessionCode
        {
            get => AppSettings.GetValueOrDefault(nameof(sessionCode), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(sessionCode), value);
        }

        public static int studentId
        {
            get => AppSettings.GetValueOrDefault(nameof(studentId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(studentId), value);
        }

        public static int studentSchoolId
        {
            get => AppSettings.GetValueOrDefault(nameof(studentSchoolId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(studentSchoolId), value);
        }
        public static string studentSchoolName
        {
            get => AppSettings.GetValueOrDefault(nameof(studentSchoolName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(studentSchoolName), value);
        }
        public static int studentClassId
        {
            get => AppSettings.GetValueOrDefault(nameof(studentClassId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(studentClassId), value);
        }
        public static string studentClassName
        {
            get => AppSettings.GetValueOrDefault(nameof(studentClassName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(studentClassName), value);
        }
        public static int studentProfileId
        {
            get => AppSettings.GetValueOrDefault(nameof(studentProfileId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(studentProfileId), value);
        }
        public static string studentFullName
        {
            get => AppSettings.GetValueOrDefault(nameof(studentFullName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(studentFullName), value);
        }
        public static string studentPhotoUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(studentPhotoUrl), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(studentPhotoUrl), value);
        }
        public static int studentClubId
        {
            get => AppSettings.GetValueOrDefault(nameof(studentClubId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(studentClubId), value);
        }
        public static int studentWalletId
        {
            get => AppSettings.GetValueOrDefault(nameof(studentWalletId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(studentWalletId), value);
        }
        public static string studentWalletNumber
        {
            get => AppSettings.GetValueOrDefault(nameof(studentWalletNumber), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(studentWalletNumber), value);
        }
        public static int studentCardId
        {
            get => AppSettings.GetValueOrDefault(nameof(studentCardId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(studentCardId), value);
        }
        public static string studentCardNumber
        {
            get => AppSettings.GetValueOrDefault(nameof(studentCardNumber), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(studentCardNumber), value);
        }
        public static int studentCardStatusId
        {
            get => AppSettings.GetValueOrDefault(nameof(studentCardStatusId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(studentCardStatusId), value);
        }
        public static string studentCardStatus
        {
            get => AppSettings.GetValueOrDefault(nameof(studentCardStatus), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(studentCardStatus), value);
        }
        public static int walletId
        {
            get => AppSettings.GetValueOrDefault(nameof(walletId), 0);
            
            set => AppSettings.AddOrUpdateValue(nameof(walletId), value);
        }
        public static string walletNumber
        {
            get => AppSettings.GetValueOrDefault(nameof(walletNumber), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(walletNumber), value);
        }

        public static int stateId
        {
            get => AppSettings.GetValueOrDefault(nameof(stateId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(stateId), value);
        }
        public static string stateName
        {
            get => AppSettings.GetValueOrDefault(nameof(stateName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(stateName), value);
        }

        public static int countryId
        {
            get => AppSettings.GetValueOrDefault(nameof(countryId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(countryId), value);
        }
        public static string countryName
        {
            get => AppSettings.GetValueOrDefault(nameof(countryName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(countryName), value);
        }
        public static string countryCode
        {
            get => AppSettings.GetValueOrDefault(nameof(countryCode), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(countryCode), value);
        }
        public static int reasonId
        {
            get => AppSettings.GetValueOrDefault(nameof(reasonId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(reasonId), value);
        }

        public static int userRaceId
        {
            get => AppSettings.GetValueOrDefault(nameof(userRaceId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(userRaceId), value);
        }

        public static string userRace
        {
            get => AppSettings.GetValueOrDefault(nameof(userRace), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(userRace), value);
        }

        public static int cardTypeId
        {
            get => AppSettings.GetValueOrDefault(nameof(cardTypeId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(cardTypeId), value);
        }

        public static string cardType
        {
            get => AppSettings.GetValueOrDefault(nameof(cardType), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(cardType), value);
        }

        public static int shiftId
        {
            get => AppSettings.GetValueOrDefault(nameof(shiftId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(shiftId), value);
        }

        public static string shiftCode
        {
            get => AppSettings.GetValueOrDefault(nameof(shiftCode), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(shiftCode), value);
        }

        public static string textColor
        {
            get => AppSettings.GetValueOrDefault(nameof(textColor), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(textColor), value);
        }

        public static string bgColor
        {
            get => AppSettings.GetValueOrDefault(nameof(bgColor), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(bgColor), value);
        }
        public static DateTime pickupDate
        {
            get => AppSettings.GetValueOrDefault(nameof(pickupDate), DateTime.MinValue);

            set => AppSettings.AddOrUpdateValue(nameof(pickupDate), value);
        }

        public static DateTime receiptDate
        {
            get => AppSettings.GetValueOrDefault(nameof(receiptDate), DateTime.MinValue);

            set => AppSettings.AddOrUpdateValue(nameof(receiptDate), value);
        }

        public static int selectProblemTypeId
        {
            get => AppSettings.GetValueOrDefault(nameof(selectProblemTypeId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(selectProblemTypeId), value);
        }

        public static string selectProblemType
        {
            get => AppSettings.GetValueOrDefault(nameof(selectProblemType), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(selectProblemType), value);
        }

        public static int occupationId
        {
            get => AppSettings.GetValueOrDefault(nameof(occupationId), 0);

            set => AppSettings.AddOrUpdateValue(nameof(occupationId), value);
        }

        public static string occupationName
        {
            get => AppSettings.GetValueOrDefault(nameof(occupationName), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(occupationName), value);
        }

#if DEBUG
        static readonly string defaultIP = DeviceInfo.Platform == DevicePlatform.Android ? "192.168.0.160" : "localhost";
#else
        static readonly string defaultIP = "xamchatr.azurewebsites.net";
#endif
        public static bool UseHttps
        {
            get => (defaultIP != "localhost" && defaultIP != "192.168.0.160");
        }

        public static string ServerIP
        {
            get => Preferences.Get(nameof(ServerIP), defaultIP);
            set => Preferences.Set(nameof(ServerIP), value);
        }

        static Random random = new Random();
        static readonly string defaultName = $"{DeviceInfo.Platform} User {random.Next(1, 100)}";
        public static string UserName
        {
            get => Preferences.Get(nameof(UserName), defaultName);
            set => Preferences.Set(nameof(UserName), value);
        }
        public static string Group
        {
            get => Preferences.Get(nameof(Group), string.Empty);
            set => Preferences.Set(nameof(Group), value);
        }
    }
}
