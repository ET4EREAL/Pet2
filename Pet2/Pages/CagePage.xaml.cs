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
    /// Логика взаимодействия для CagePage.xaml
    /// </summary>
    public partial class CagePage : Page
    {
        public CagePage()
        {
            InitializeComponent();
            DGridCage.ItemsSource = App.db.Cage.ToList();
            startsAtDatePicker.DisplayDateStart = DateTime.Now;
            endsAtDatePicker.DisplayDateStart = DateTime.Now;
            startsAtDatePicker.SelectedDate = DateTime.Now;
            endsAtDatePicker.SelectedDate = DateTime.Now;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new CageAddEditPage(null));
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBox = MessageBox.Show("Вы уверены, что хотите удалить вольер", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question); if (messageBox == MessageBoxResult.Yes)
                {
                    if (((sender as Button).DataContext as Cage).Service.Count() != 0)
                    {
                        MessageBox.Show("Вы не можете удалить данный вольер");
                        return;
                    }
                    App.db.Cage.Remove((sender as Button).DataContext as Cage); App.db.SaveChanges();
                    ProjectManager.MainFrame.Navigate(new CagePage());
                }
            }
            catch
            {
                MessageBox.Show("Вы не можете удалить данный вольер");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ProjectManager.MainFrame.Navigate(new CageAddEditPage((sender as Button).DataContext as Cage));
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime start = startsAtDatePicker.SelectedDate.Value.Date;
                DateTime end = endsAtDatePicker.SelectedDate.Value.Date;
                int cagecount = 0;
                var cagesList = new List<Cage>();
                foreach (var cage in App.db.Cage)
                {
                    bool f = true;
                    foreach (var service in cage.Service)
                    {
                        if (App.DateRangeIntersects(start, end, service.StartsAt, service.EndsAt))
                        {
                            f = false;
                            break;
                        }
                    }
                    if (f) cagecount++;
                }
                CountCage.Text = "Свободно " + cagecount.ToString() + " вольеров";
            }
            catch
            {
                CountCage.Text = "Свободно вольеров";
            }
        }
    }
}
