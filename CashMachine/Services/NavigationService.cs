using System.Windows.Controls;

namespace CashMachineApp.Services
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : Page;
        void NavigateTo(Page page);
        void GoBack();
        Frame? NavigationFrame { get; set; }
    }

    public class NavigationService : INavigationService
    {
        private Frame? _navigationFrame;

        public Frame? NavigationFrame
        {
            get => _navigationFrame;
            set
            {
                _navigationFrame = value;
            }
        }

        public void NavigateTo<T>() where T : Page
        {
            if (_navigationFrame == null)
                return;

            var page = Activator.CreateInstance<T>();
            _navigationFrame.Navigate(page);
        }

        public void NavigateTo(Page page)
        {
            if (_navigationFrame == null)
                return;

            _navigationFrame.Navigate(page);
        }

        public void GoBack()
        {
            if (_navigationFrame?.CanGoBack == true)
            {
                _navigationFrame.GoBack();
            }
        }
    }
}
