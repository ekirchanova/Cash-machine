using CashMachineApp.Services;
using CashMachineNamespace;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace CashMachineApp.ViewModels
{
    public class BanknoteInputViewModel : ViewModelBase
    {
        private string _amount = string.Empty;
        public DenominationBanknotes Denomination { get; set; }
        public string DenominationName { get; set; } = string.Empty;

        public string Amount
        {
            get => _amount;
            set
            {
                if (SetProperty(ref _amount, value))
                {
                    OnPropertyChanged(nameof(HasValidAmount));
                }
            }
        }

        public bool HasValidAmount => !string.IsNullOrWhiteSpace(Amount) && uint.TryParse(Amount, out _);

        public uint GetAmountValue()
        {
            return uint.TryParse(Amount, out var value) ? value : 0;
        }
    }

    public class PutMoneyPageViewModel : ViewModelBase
    {
        private readonly CashMachine _cashMachine;
        private readonly INavigationService _navigationService;
        private ObservableCollection<BanknoteInputViewModel> _banknoteInputs;

        public ObservableCollection<BanknoteInputViewModel> BanknoteInputs
        {
            get => _banknoteInputs;
            set => SetProperty(ref _banknoteInputs, value);
        }

        public PutMoneyPageViewModel(CashMachine cashMachine, INavigationService navigationService)
        {
            _cashMachine = cashMachine;
            _navigationService = navigationService;
            InitializeBanknoteInputs();
        }

        private void InitializeBanknoteInputs()
        {
            _banknoteInputs = new ObservableCollection<BanknoteInputViewModel>
            {
                new BanknoteInputViewModel { Denomination = DenominationBanknotes.FiveRubles, DenominationName = "5 rubles" },
                new BanknoteInputViewModel { Denomination = DenominationBanknotes.TenRubles, DenominationName = "10 rubles" },
                new BanknoteInputViewModel { Denomination = DenominationBanknotes.FiftyRubles, DenominationName = "50 rubles" },
                new BanknoteInputViewModel { Denomination = DenominationBanknotes.OneHundredRubles, DenominationName = "100 rubles" },
                new BanknoteInputViewModel { Denomination = DenominationBanknotes.FiveHundredRubles, DenominationName = "500 rubles" },
                new BanknoteInputViewModel { Denomination = DenominationBanknotes.OneThousandRubles, DenominationName = "1000 rubles" },
                new BanknoteInputViewModel { Denomination = DenominationBanknotes.TwoThousandRubles, DenominationName = "2000 rubles" },
                new BanknoteInputViewModel { Denomination = DenominationBanknotes.FiveThousandRubles, DenominationName = "5000 rubles" }
            };
            OnPropertyChanged(nameof(BanknoteInputs));
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
                uint remain = CashMachine.CalculateSumMoney(banknotes);
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

