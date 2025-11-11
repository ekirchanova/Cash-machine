using System.Windows.Controls;

namespace CashMachineApp
{
    public enum PageType
    {
        PutMoneyResultPage,
        WithdrawMoneyResultPage
    }

    public partial class ResultPage : Page
    {
        public ResultPage()
        {
            InitializeComponent();
        }
    }
}
