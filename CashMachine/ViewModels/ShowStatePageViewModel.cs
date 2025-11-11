using CashMachineApp.Services;
using CashMachineNamespace;

namespace CashMachineApp.ViewModels
{
    public class ShowStatePageViewModel : ViewModelBase
    {
        private readonly CashMachine _cashMachine;
        private readonly INavigationService _navigationService;

        public ulong AmountOfMoney => _cashMachine.CurrentAmountOfMoney;
        public uint AmountOfBanknotes => _cashMachine.CurrentAmountBanknotes;

        public ShowStatePageViewModel(CashMachine cashMachine, INavigationService navigationService)
        {
            _cashMachine = cashMachine;
            _navigationService = navigationService;
            InitializeBanknoteInputs();
            SetBanknotesAmount();
        }

        private void SetBanknotesAmount()
        {
            var banknotes = _cashMachine.Banknotes;
            foreach (var banknoteInputModel in BanknoteInputs)
            {
                var curDenomination = banknoteInputModel.Denomination;
                uint curAmount = banknotes.ContainsKey(curDenomination)? banknotes[curDenomination] : 0;
                banknoteInputModel.AmountInt = curAmount;
            }
        }

        public RelayCommand OkCommand => new RelayCommand(ExecuteOkCommand);

        private void ExecuteOkCommand()
        {
            _navigationService.GoBack();
        }
    }
}

