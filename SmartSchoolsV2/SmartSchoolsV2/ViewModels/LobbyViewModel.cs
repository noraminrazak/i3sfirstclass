using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartSchoolsV2.ViewModels
{
    public class LobbyViewModel : ViewModelBase
    {
        public List<string> Rooms { get; }
        public LobbyViewModel()
        {
            Rooms = ChatService.GetRooms();
        }

        public string UserName
        {
            get => Settings.fullName;
            set
            {
                if (value == UserName)
                    return;
                Settings.fullName = value;
                //OnPropertyChanged();
            }
        }

        public async Task GoToGroupChat(INavigation navigation, string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                return;

            if (string.IsNullOrWhiteSpace(UserName))
                return;

            Settings.Group = group;
            await navigation.PushModalAsync(new XamChatNavigationPage(new GroupChatPage()));
        }

    }
}
