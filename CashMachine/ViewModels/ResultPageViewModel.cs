using CashMachineApp.Services;

namespace CashMachineApp.ViewModels
{
    public class ResultPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ulong _remain;
        private PageType _pageType;

        public ulong Remain
        {
            get => _remain;
            set => SetProperty(ref _remain, value);
        }

        public PageType PageType
        {
            get => _pageType;
            set => SetProperty(ref _pageType, value);
        }

        public string TitleText => PageType == PageType.PutMoneyResultPage 
            ? "Put money result" 
            : "Withdraw money result";

        public string RemainText => PageType == PageType.PutMoneyResultPage 
            ? "Can't input money:" 
            : "Can't get money:";

        public ResultPageViewModel(ulong remain, PageType pageType, INavigationService navigationService)
        {
            _remain = remain;
            _pageType = pageType;
            _navigationService = navigationService;
        }

        public RelayCommand OkCommand => new RelayCommand(ExecuteOkCommand);

        private void ExecuteOkCommand()
        {
            _navigationService.GoBack();
            _navigationService.GoBack();
        }
    }
}

