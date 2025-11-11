using CashMachineApp.Services;
using CashMachineApp.ViewModels;
using CashMachineNamespace;
using System.Windows;

namespace CashMachineApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CashMachine _cashMachine;
        private readonly INavigationService _navigationService;

        public MainWindow()
        { 
            _cashMachine = new CashMachine();
            _navigationService = new NavigationService();
            
            InitializeComponent();
            
            _navigationService.NavigationFrame = MainFrame;
            
            var mainPage = new MainPage();
            mainPage.DataContext = new MainPageViewModel(_cashMachine, _navigationService);
            MainFrame.Navigate(mainPage);
        }
    }
}