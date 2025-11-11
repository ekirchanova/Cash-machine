using CashMachineApp.Services;
using CashMachineNamespace;
using System.Windows.Controls;

namespace CashMachineApp.ViewModels
{
    public class ShowStatePageViewModel : ViewModelBase
    {
        private readonly CashMachine _cashMachine;
        private readonly INavigationService _navigationService;

        public uint AmountOfMoney => _cashMachine.CurrentAmountOfMoney;
        public uint AmountOfBanknotes => _cashMachine.CurrentAmountBanknotes;

        public ShowStatePageViewModel(CashMachine cashMachine, INavigationService navigationService)
        {
            _cashMachine = cashMachine;
            _navigationService = navigationService;
        }

        public RelayCommand OkCommand => new RelayCommand(ExecuteOkCommand);

        private void ExecuteOkCommand()
        {
            _navigationService.GoBack();
        }
    }
}

