using Prism.Mvvm;
using Prism.Navigation;
using SmartSchoolsV2.Services;
using Xamarin.Forms;

namespace SmartSchoolsV2.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        ChatService chatService;
        public ChatService ChatService =>
            chatService ?? (chatService = DependencyService.Resolve<ChatService>());

        IDialogService dialogService;
        public IDialogService DialogService =>
            dialogService ?? (dialogService = DependencyService.Resolve<IDialogService>());

        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        //public ViewModelBase(INavigationService navigationService)
        //{
        //    NavigationService = navigationService;
        //}

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
