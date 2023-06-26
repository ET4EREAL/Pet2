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

namespace Pet2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
            // Получение данных из бд в дата грид (таблицу)
            DGridClient.ItemsSource = App.db.Client.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу добавления данных
            ProjectManager.MainFrame.Navigate(new ClientAddEditPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу редактирования данных
            ProjectManager.MainFrame.Navigate(new ClientAddEditPage((sender as Button).DataContext as Client));
        }
    }
}
