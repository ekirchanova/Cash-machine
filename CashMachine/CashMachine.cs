namespace CashMachineNamespace
{
    public enum DenominationBanknotes
    {
        FiveRubles,
        TenRubles,
        FiftyRubles,
        OneHundredRubles,
        FiveHundredRubles,
        OneThousandRubles,
        TwoThousandRubles,
        FiveThousandRubles
    }

    public partial class CashMachine
    {
        private static uint MaxAmountBankNotes {get;set;}
        private static SortedDictionary<DenominationBanknotes, uint> denominationBanknotes;

        static CashMachine()
        {
            MaxAmountBankNotes = 1000;
            denominationBanknotes = new SortedDictionary<DenominationBanknotes, uint>(Comparer<DenominationBanknotes>.Create((x, y) => y.CompareTo(x))) {
                {DenominationBanknotes.FiveRubles, 5 },
                {DenominationBanknotes.TenRubles, 10},
                {DenominationBanknotes.FiftyRubles, 50},
                {DenominationBanknotes.OneHundredRubles, 100},
                {DenominationBanknotes.FiveHundredRubles, 500},
                {DenominationBanknotes.OneThousandRubles, 1000},
                {DenominationBanknotes.TwoThousandRubles, 2000},
                {DenominationBanknotes.FiveThousandRubles, 5000}
            };
        }
        public uint CurrentAmountBanknotes { get; private set; }
        private Dictionary<DenominationBanknotes, uint> _banknotes;
        public Dictionary<DenominationBanknotes, uint> Banknotes
        {
            get => _banknotes;
            set
            {
                _banknotes = value;
                CurrentAmountBanknotes = 0;
                if (_banknotes != null)
                {
                    foreach (var (_, currentAmount) in _banknotes)
                    {
                        CurrentAmountBanknotes += currentAmount;
                    }
                }
            }
        }

        public uint CurrentAmountOfMoney { get => CalculateSumMoney(Banknotes); }
        public CashMachine() 
        {
            CurrentAmountBanknotes = 0;
            _banknotes = new Dictionary<DenominationBanknotes, uint>();
        }
        public CashMachine(Dictionary<DenominationBanknotes, uint> banknotes)
        {
            Banknotes = banknotes;
        }

        private void AddMoney(DenominationBanknotes denomination, uint amount)
        {
            if (Banknotes.ContainsKey(denomination))
                Banknotes[denomination] += amount;
            else
                Banknotes[denomination] = amount;
        }

        public bool PutMoney(ref  Dictionary<DenominationBanknotes, uint> banknotes)
        {
            if (banknotes.Count == 0) return true;
            foreach (var (denomination, currentAmount) in banknotes)
            {
                if (CurrentAmountBanknotes + currentAmount <= MaxAmountBankNotes)
                {
                    AddMoney(denomination, currentAmount);
                    banknotes.Remove(denomination);
                    CurrentAmountBanknotes += currentAmount;
                }
                else
                {
                    uint addingAmount = MaxAmountBankNotes - CurrentAmountBanknotes;

                    AddMoney(denomination, addingAmount);
                    banknotes[denomination] -= addingAmount;
                    CurrentAmountBanknotes = MaxAmountBankNotes;
                    return false;
                }
            }
            return true;
        }
        
        public bool WithdrawMoney(ref uint sum, ref Dictionary<DenominationBanknotes, uint> result, bool isSmallMoney = true)
        {
            if (sum == 0) return true;
            if (Banknotes.Count == 0) return false; ;
            var curDenominationBanknotes = !isSmallMoney ? denominationBanknotes.ToList() : denominationBanknotes.ToList().AsEnumerable().Reverse().ToList();
            foreach (var(denomination, currentDenomination) in curDenominationBanknotes)
            {
                if (!Banknotes.ContainsKey(denomination))
                    continue;
                uint wholePart = sum / currentDenomination;
                if (wholePart == 0)
                    continue;

                uint curAmount = Banknotes[denomination];
                if (curAmount < wholePart)
                    wholePart = curAmount;
                result[denomination] = wholePart;
                Banknotes[denomination] -= wholePart;
                CurrentAmountBanknotes -= wholePart;
                sum -= wholePart * currentDenomination;
                
            }
            return sum == 0;
        }

        public static uint CalculateSumMoney(Dictionary<DenominationBanknotes, uint> money)
        {
            uint sum = 0;
            foreach (var (denomination, currentAmount) in money)
            {
                sum += currentAmount * denominationBanknotes[denomination];
            }
            return sum;
        }

    }
}
