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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        private Service currentService = new Service();
        public ServicePage()
        {
            InitializeComponent();
            DGridService.ItemsSource = App.db.Service.ToList();

            // Получение контекста данных текущей заявки
            DataContext = currentService;

            CountOfCage();
        }

        void CountOfCage()
        {
            // занятые вольеры по айдишнику (список)
            var t = App.db.Service.Where(a => a.Cage.ID == a.CageID).Select(d => d.Cage.Number).Distinct().ToList();
            var b = App.db.Cage.ToList();
            if (t.Count == b.Count())
            {
                CageCount.Text = "Все вольеры заняты";
            }
            else
            {
                CageCount.Text = $"Занято {t.Count} вольеров из {b.Count}";
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу добавления данных
            ProjectManager.MainFrame.Navigate(new ServiceAddEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Множественный выбор (можно сразу выбрать несколько элементов и их удалить)
            var serviceForRemoving = DGridService.SelectedItems.Cast<Service>().ToList();

            // Сообщение предупреждение об кол-ве удаления элементов
            if (MessageBox.Show($"Вы действительно хотите удалить следующие {serviceForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаление данных (сколько строк необходимо удалить)
                    App.db.Service.RemoveRange(serviceForRemoving);
                    // Сохранение данных
                    App.db.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    // Получение данных после удаления
                    DGridService.ItemsSource = App.db.Service.ToList();
                    CountOfCage();
                }
                // Ошибка
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnCancelEarlier_Click(object sender, RoutedEventArgs e)
        {
            // Сообщение предупреждение об кол-ве удаления элементов
            if (MessageBox.Show("Вы действительно хотите закончить проживание?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.db.Service.Remove((sender as Button).DataContext as Service);
                    MessageBox.Show("Проживание было успешно закончено!");
                    App.db.SaveChanges();
                    DGridService.ItemsSource = null;
                    DGridService.ItemsSource = App.db.Service.ToList();
                    CountOfCage();
                }
    
                // Ошибка
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Если страница видима, то происходит обновление данных
            if (Visibility == Visibility.Visible)
            {
                DGridService.ItemsSource = null;
                DGridService.ItemsSource = App.db.Service.ToList();
                CountOfCage();
            }
        }


        void searching()
        {
            try
            {
                var list = App.db.Service.Where(a => a.Client.FullName.ToLower().Contains(endsbox.Text) || a.Client.Phone.ToLower().Contains(endsbox.Text)).ToList();
                if (string.IsNullOrEmpty(endsbox.Text)) list = App.db.Service.OrderBy(f => f.ID).ToList();
                if (voz.IsChecked == true) list = list.OrderBy(s => s.EndsAt).ToList();
                else
                    if (yb.IsChecked == true)
                    list = list.OrderByDescending(s => s.EndsAt).ToList();
                DGridService.ItemsSource = list;
                // Если заявок не обнаружено сообщение о том, что результатов не найдено
                if (!list.Any())
                {
                    DGridService.Visibility = Visibility.Hidden;
                    notfound.Visibility = Visibility.Visible;
                   // yb.IsChecked = false;
                   // voz.IsChecked = false;
                }
                else
                {
                    DGridService.Visibility = Visibility.Visible;
                    notfound.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                DGridService.ItemsSource = App.db.Service.OrderBy(f => f.ID).ToList();
            }
        }

        private void endsbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searching();
        }

        private void voz_Checked(object sender, RoutedEventArgs e)
        {
            searching();
        }
    }
}
