using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Win32;
using System.IO;

namespace Pet2.Pages
{
    /// <summary>
    /// Логика взаимодействия для PetViewPage.xaml
    /// </summary>
    public partial class PetViewPage : Page
    {
        private static ObservableCollection<Pet> List = new ObservableCollection<Pet>();
        public PetViewPage()
        {
            InitializeComponent();
            var allpet = App.db.Pet.ToList();
            petView.ItemsSource = allpet.ToList();
        }

        private void petView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (petView.SelectedItems.Count > 0)
            {
                var item = petView.SelectedItem as Pet;

                if (item != null)
                {
                    CurrentPetPage cpp = new CurrentPetPage(item);
                    ProjectManager.MainFrame.Navigate(new CurrentPetPage(item));
                }
            }*/
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу редактирования данных
            ProjectManager.MainFrame.Navigate(new PetAddEditPage((sender as Button).DataContext as Pet));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBox = MessageBox.Show("Вы уверены, что хотите удалить животное", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBox == MessageBoxResult.Yes)
            {
                if (((sender as Button).DataContext as Pet).Service.Count() != 0)
                {
                    MessageBox.Show("Вы не можете удалить данное животное");
                    return;
                }
                var h = (sender as Button).DataContext as Pet;       
                App.db.Pet.Remove(h);
                App.db.SaveChanges();
                ProjectManager.MainFrame.Navigate(new PetViewPage());
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new PetAddEditPage(null));
        }



        private void petbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search();
        }

        void search()
        {
            try
            {
                var list = App.db.Pet.Where(a => a.Client.FullName.ToLower().Contains(endsbox.Text) || a.Client.Phone.ToLower().Contains(endsbox.Text) || a.Nickname.ToLower().Contains(endsbox.Text)).ToList();
                if (string.IsNullOrEmpty(endsbox.Text)) list = App.db.Pet.ToList();
                petView.ItemsSource = list;
                // Если животных не обнаружено сообщение о том, что результатов не найдено
                if (!list.Any())
                {
                    petView.Visibility = Visibility.Hidden;
                    notfound.Visibility = Visibility.Visible;
                    
                }
                else
                {
                    petView.Visibility = Visibility.Visible;
                    notfound.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                petView.ItemsSource = App.db.Pet.ToList();
            }
        }
    }
}
