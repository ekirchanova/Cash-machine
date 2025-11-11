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
        private static uint MaxAmountBanknotesEveryDenomination{get;set;}
        private static SortedDictionary<DenominationBanknotes, uint> denominationBanknotes;

        static CashMachine()
        {
            MaxAmountBanknotesEveryDenomination = 1000;
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
                if (_banknotes.Count > 0)
                {
                    foreach (var (_, currentAmount) in _banknotes)
                    {
                        CurrentAmountBanknotes += currentAmount;
                    }
                }
            }
        }

        public ulong CurrentAmountOfMoney { get => CalculateSumMoney(Banknotes); }
        public CashMachine() 
        {
            CurrentAmountBanknotes = 0;
            _banknotes = new Dictionary<DenominationBanknotes, uint>();
        }
        public CashMachine(Dictionary<DenominationBanknotes, uint> banknotes)
        {
            _banknotes = banknotes;
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
            List<DenominationBanknotes> needDelete = new List<DenominationBanknotes>();
            bool successPut = true;
            
            foreach (var (denomination, currentAmount) in banknotes)
            {
                uint curAmountCurDenomination = Banknotes.ContainsKey(denomination) ? Banknotes[denomination] : 0; 
                if (curAmountCurDenomination + currentAmount <= MaxAmountBanknotesEveryDenomination)
                {
                    AddMoney(denomination, currentAmount);
                    needDelete.Add(denomination);
                    CurrentAmountBanknotes += currentAmount;
                }
                else
                {
                    uint addingAmount = MaxAmountBanknotesEveryDenomination - curAmountCurDenomination;
                    AddMoney(denomination, addingAmount);
                    banknotes[denomination] -= addingAmount;
                    CurrentAmountBanknotes += addingAmount;
                    successPut =  false;
                    break;
                }
            }

            foreach (var denomination in needDelete)
            {
                banknotes.Remove(denomination);
            }
            
            
            return successPut;
        }
        
        public bool WithdrawMoney(ref uint sum, ref Dictionary<DenominationBanknotes, uint> result, bool isSmallMoney = true)
        {
            if (sum == 0) return true;
            if (Banknotes.Count == 0) return false;
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

        public static ulong CalculateSumMoney(Dictionary<DenominationBanknotes, uint> money)
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
