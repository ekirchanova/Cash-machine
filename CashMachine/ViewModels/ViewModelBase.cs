using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using CashMachineNamespace;


namespace CashMachineApp.ViewModels
{
    public class BanknoteInputViewModel : ViewModelBase
    {
        private string _amount = string.Empty;
        public DenominationBanknotes Denomination { get; set; }
        public string DenominationName { get; set; } = string.Empty;

        public uint AmountInt
        {
            set
            {
                _amount = value.ToString();
            }
        }
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
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private ObservableCollection<BanknoteInputViewModel> _banknoteInputs;

        public ObservableCollection<BanknoteInputViewModel> BanknoteInputs
        {
            get => _banknoteInputs;
            set => SetProperty(ref _banknoteInputs, value);
        }

        protected void InitializeBanknoteInputs()
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
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

