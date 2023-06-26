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
    /// Логика взаимодействия для ClientViewPage.xaml
    /// </summary>
    public partial class ClientViewPage : Page
    {
        //public static PetHotel_ЗалялетдиновEntities db1 = new PetHotel_ЗалялетдиновEntities();
        public ClientViewPage()
        {
            InitializeComponent();
            var allclient = App.db.Client.ToList();
            ClientView.ItemsSource = allclient.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new ClientAddEditPage((sender as Button).DataContext as Client));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBox = MessageBox.Show("Вы уверены, что хотите удалить клиента","Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBox == MessageBoxResult.Yes)
            {
                if (((sender as Button).DataContext as Client).Service.Count() != 0)
                {
                    MessageBox.Show("Вы не можете удалить данного клиента");
                    return;
                }
                App.db.Client.Remove((sender as Button).DataContext as Client);
                App.db.SaveChanges();
                ProjectManager.MainFrame.Navigate(new ClientViewPage());
            }
        }

        private void petView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new ClientAddEditPage(null));
        }

        private void clientbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searching();
        }

        void searching()
        {
            try
            {
                var list = App.db.Client.Where(a => a.FullName.ToLower().Contains(endsbox.Text) || a.Phone.ToLower().Contains(endsbox.Text)).ToList();
                if (string.IsNullOrEmpty(endsbox.Text)) list = App.db.Client.ToList();
                ClientView.ItemsSource = list;
                // Если животных не обнаружено сообщение о том, что результатов не найдено
                if (!list.Any())
                {
                    ClientView.Visibility = Visibility.Hidden;
                    notfound.Visibility = Visibility.Visible;
                }
                else
                {
                    ClientView.Visibility = Visibility.Visible;
                    notfound.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                ClientView.ItemsSource = App.db.Client.ToList();
            }
        }
    }
}
