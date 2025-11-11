using CashMachineApp.Services;
using CashMachineNamespace;
using System.Windows.Controls;

namespace CashMachineApp.ViewModels
{
    public class WithdrawMoneyPageViewModel : ViewModelBase
    {
        private readonly CashMachine _cashMachine;
        private readonly INavigationService _navigationService;
        private string _sumMoney = string.Empty;
        private bool _isSmallMoney = false;

        public string SumMoney
        {
            get => _sumMoney;
            set => SetProperty(ref _sumMoney, value);
        }

        public bool IsSmallMoney
        {
            get => _isSmallMoney;
            set => SetProperty(ref _isSmallMoney, value);
        }

        public WithdrawMoneyPageViewModel(CashMachine cashMachine, INavigationService navigationService)
        {
            _cashMachine = cashMachine;
            _navigationService = navigationService;
        }

        public RelayCommand OkCommand => new RelayCommand(ExecuteOkCommand);
        public RelayCommand CancelCommand => new RelayCommand(ExecuteCancelCommand);

        private void ExecuteOkCommand()
        {
            if (string.IsNullOrWhiteSpace(SumMoney) || !uint.TryParse(SumMoney, out uint sum))
                return;

            bool smallMoney = IsSmallMoney;
            Dictionary<DenominationBanknotes, uint> result = new Dictionary<DenominationBanknotes, uint>();
            bool res = _cashMachine.WithdrawMoney(ref sum, ref result, smallMoney);
            
            if (!res)
            {
                var resultPage = new ResultPage();
                resultPage.DataContext = new ResultPageViewModel(sum, PageType.WithdrawMoneyResultPage, _navigationService);
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

