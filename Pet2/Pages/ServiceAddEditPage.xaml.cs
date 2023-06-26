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
    /// Логика взаимодействия для ServiceAddEditPage.xaml
    /// </summary>
    public partial class ServiceAddEditPage : Page
    {
        List<Cage> cages = new List<Cage>();
        // Текущий продукт
        private Service currentService = new Service();
       // private Reservation currentReservation = new Reservation();
        public ServiceAddEditPage(Service selectedService)
        {
            InitializeComponent();
            // Переход на редактирование
            if (selectedService != null)
            {
                currentService = selectedService;
                addbutton.Content = "Редактировать";
                PetComboBox.ItemsSource = App.db.Pet.Where(r => r.ClientID == currentService.ClientID).ToList();
                startsAtDatePicker.IsEnabled = false;
                CageComboBox.IsEnabled = false;
                PhoneComboBox.IsEnabled = false;
            }
            else
            {
                PetComboBox.IsEnabled = false;
                currentService.StartsAt = DateTime.Now;
                currentService.EndsAt = DateTime.Now;
                startsAtDatePicker.DisplayDateStart = DateTime.Now;
                endsAtDatePicker.DisplayDateStart = DateTime.Now;
            }
            // Получение контекста данных текущего продукта
            DataContext = currentService;
            CageComboBox.ItemsSource = App.db.Cage.ToList();
            PhoneComboBox.ItemsSource = App.db.Client.ToList();
            currentService.CreatedAt = DateTime.Now;

        }

        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            // Ошибки
            StringBuilder errors = new StringBuilder();

            // Проверки
            if (currentService.Client == null)
                errors.AppendLine("Укажите кличку животного");
            if (currentService.Cage == null)
                errors.AppendLine("Выберите вольер");
            if (currentService.StartsAt == default)
                errors.AppendLine("Выберите дату заселения");
            if (currentService.EndsAt == default)
                errors.AppendLine("Выберите дату выселения");
            if (currentService.Duration == 0)
                errors.AppendLine("Укажите продолжительность услуги");
            if (currentService.CreatedAt == null)
                errors.AppendLine("Выберите дату создания заявки");
            if (currentService.StartsAt >= currentService.EndsAt)
                errors.AppendLine("Дата заселения не может быть выше даты выселения");
      
            // Если ошибки есть, то вывод каждой с новой строки
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Добавление данных
            if (currentService.ID == 0)
                App.db.Service.Add(currentService);

            try
            {
                // сохранение изменений (при добавлении или редактировании данных)
                App.db.SaveChanges(); //MainWindow.context.SaveChanges();
                MessageBox.Show("Информация сохранена!");
                // Переход на страницу товаров
                ProjectManager.MainFrame.Navigate(new Pages.ServicePage());
            }
            // Ошибка
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ReservationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void CageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void CageComboBox_DropDownOpened(object sender, EventArgs e)
        {
            var cagesList = new List<Cage>();
            foreach (var cage in App.db.Cage)
            {
                bool f = true;
                foreach (var service in cage.Service)
                {
                    if (App.DateRangeIntersects(currentService.StartsAt, currentService.EndsAt, service.StartsAt, service.EndsAt))
                    {
                        f = false;
                        break;
                    }
                }
                if (f) cagesList.Add(cage);
            }
            if (!cagesList.Any())
            {
                MessageBox.Show("На выбранную дату и время не найдено ни одного вольера");
            }
            CageComboBox.ItemsSource = cagesList;
        }

        private void CageComboBox_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void endsAtDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime start = startsAtDatePicker.SelectedDate.Value.Date;
            DateTime end = endsAtDatePicker.SelectedDate.Value.Date;
            var prod = end.Subtract(start);
            durationTextBox.Text = prod.TotalDays.ToString();
          
        }

        private void startsAtDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        { try
            {
                if (endsAtDatePicker.SelectedDate.Value < startsAtDatePicker.SelectedDate.Value)
                {
                    currentService.EndsAt = startsAtDatePicker.SelectedDate.Value;
                    endsAtDatePicker.SelectedDate = startsAtDatePicker.SelectedDate.Value;
                }
                endsAtDatePicker.DisplayDateStart = startsAtDatePicker.SelectedDate.Value.Date;
            }
            catch { }
        }

        private void PhoneComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PhoneComboBox_DropDownOpened(object sender, EventArgs e)
        {

        }

        private void PhoneComboBox_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                PetComboBox.IsEnabled = true;
                var t = App.db.Client.Where(s => s.FullName == PhoneComboBox.Text).First();
                PetComboBox.ItemsSource = App.db.Pet.Where(r => r.ClientID == t.ID).ToList();
            }
            catch
            {
                PetComboBox.IsEnabled = false;
            }
        }

        private void PetComboBox_DropDownOpened(object sender, EventArgs e)
        {

        }
    }
}

