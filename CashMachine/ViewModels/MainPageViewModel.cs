using CashMachineApp.Services;
using CashMachineNamespace;

namespace CashMachineApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly CashMachine _cashMachine;
        private readonly INavigationService _navigationService;

        public MainPageViewModel(CashMachine cashMachine, INavigationService navigationService)
        {
            _cashMachine = cashMachine;
            _navigationService = navigationService;
        }

        public RelayCommand NavigateToPutMoneyCommand => new RelayCommand(NavigateToPutMoney);
        public RelayCommand NavigateToWithdrawMoneyCommand => new RelayCommand(NavigateToWithdrawMoney);
        public RelayCommand NavigateToShowStateCommand => new RelayCommand(NavigateToShowState);

        private void NavigateToPutMoney()
        {
            var page = new PutMoneyPage();
            page.DataContext = new PutMoneyPageViewModel(_cashMachine, _navigationService);
            _navigationService.NavigateTo(page);
        }

        private void NavigateToWithdrawMoney()
        {
            var page = new WithdrawMoneyPage();
            page.DataContext = new WithdrawMoneyPageViewModel(_cashMachine, _navigationService);
            _navigationService.NavigateTo(page);
        }

        private void NavigateToShowState()
        {
            var page = new ShowStatePage();
            page.DataContext = new ShowStatePageViewModel(_cashMachine, _navigationService);
            _navigationService.NavigateTo(page);
        }
    }
}

