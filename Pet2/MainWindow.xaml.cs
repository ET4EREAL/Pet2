using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pet2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ProjectManager.MainFrame = mainFrame;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void Service(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new Pages.ServicePage());
        }


        private void ClosingMainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result =  MessageBox.Show("Вы действительно хотети закрыть программу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }

        private void Loading(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new Pages.ServicePage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new Pages.PetViewPage());
        }

        private void Reservation(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new Pages.ClientViewPage());
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        bool _maximized = false;
        double _prevWidth = 0;
        double _prevHeight = 0;
        double _prevTop = 0;
        double _prefLeft = 0;
        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            maxOrMin();
        }

        void maxOrMin()
        {
            if (_maximized)
            {
                Top = _prevTop;
                Left = _prefLeft;
                Width = _prevWidth;
                Height = _prevHeight;
                _maximized = false;
                return;
            }
            _prevTop = Top;
            _prefLeft = Left;
            _prevWidth = Width;
            _prevHeight = Height;
            Top = 0;
            Left = 0;
            Height = SystemParameters.WorkArea.Height;
            Width = SystemParameters.WorkArea.Width;
            _maximized = true;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new Pages.CagePage());
        }
    }
}
