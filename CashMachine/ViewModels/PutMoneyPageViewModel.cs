using CashMachineApp.Services;
using CashMachineNamespace;

namespace CashMachineApp.ViewModels
{ 
    public class PutMoneyPageViewModel : ViewModelBase
    {
        private readonly CashMachine _cashMachine;
        private readonly INavigationService _navigationService;

        public PutMoneyPageViewModel(CashMachine cashMachine, INavigationService navigationService)
        {
            _cashMachine = cashMachine;
            _navigationService = navigationService;
            InitializeBanknoteInputs();
        }

        public RelayCommand OkCommand => new RelayCommand(ExecuteOkCommand);
        public RelayCommand CancelCommand => new RelayCommand(ExecuteCancelCommand);

        private void ExecuteOkCommand()
        {
            var banknotes = new Dictionary<DenominationBanknotes, uint>();
            
            foreach (var input in BanknoteInputs)
            {
                if (input.HasValidAmount && input.GetAmountValue() > 0)
                {
                    banknotes[input.Denomination] = input.GetAmountValue();
                }
            }

            bool result = _cashMachine.PutMoney(ref banknotes);
            if (!result)
            {
                ulong remain = CashMachine.CalculateSumMoney(banknotes);
                var resultPage = new ResultPage();
                resultPage.DataContext = new ResultPageViewModel(remain, PageType.PutMoneyResultPage, _navigationService);
                _navigationService.NavigateTo(resultPage);
            }
            else
            {
                _navigationService.GoBack();
            }
        }

        private void ExecuteCancelCommand()
        {
            _navigationService.GoBack();
        }
    }
}

