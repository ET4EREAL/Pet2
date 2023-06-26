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
    /// Логика взаимодействия для PetPage.xaml
    /// </summary>
    public partial class PetPage : Page
    {
        public PetPage()
        {
            InitializeComponent();
            // Получение данных из бд в дата грид (таблицу)
            DGridPet.ItemsSource = App.db.Pet.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу добавления данных
            ProjectManager.MainFrame.Navigate(new PetAddEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Множественный выбор (можно сразу выбрать несколько элементов и их удалить)
            var petForRemoving = DGridPet.SelectedItems.Cast<Pet>().ToList();

            // Сообщение предупреждение об кол-ве удаления элементов
            if (MessageBox.Show($"Вы действительно хотите удалить следующие {petForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаление данных (сколько строк необходимо удалить)
                    App.db.Pet.RemoveRange(petForRemoving);
                    // Сохранение данных
                    App.db.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    // Получение данных после удаления
                    DGridPet.ItemsSource = App.db.Pet.ToList();
                }
                // Ошибка
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }



        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу редактирования данных
            ProjectManager.MainFrame.Navigate(new PetAddEditPage((sender as Button).DataContext as Pet));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Если страница видима, то происходит обновление данных
            if (Visibility == Visibility.Visible)
            {
                App.db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridPet.ItemsSource = App.db.Pet.ToList();
            }
        }
    }
}
