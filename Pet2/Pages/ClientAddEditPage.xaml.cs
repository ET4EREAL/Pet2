using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ClientAddEditPage.xaml
    /// </summary>
    public partial class ClientAddEditPage : Page
    {
        // Текущий клиент
        private Client currentClient = new Client();
        public ClientAddEditPage(Client selectedClient)
        {
            InitializeComponent();
            // Переход на редактирование
            if (selectedClient != null)
            {
                currentClient = selectedClient;
                addbutton.Content = "Редактировать";
            }


            // Получение контекста данных текущего клиента
            DataContext = currentClient;
        }

        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            // Ошибки
            StringBuilder errors = new StringBuilder();



            // Проверки
            if (string.IsNullOrWhiteSpace(currentClient.FullName))
                errors.AppendLine("Укажите ФИО клиента");
            else
            {
                Regex regex = new Regex("^[a-zA-Zа-яА-Я ]+$");
                bool isValiddd = regex.IsMatch(currentClient.FullName);
                if (!isValiddd)
                {
                    errors.AppendLine("Введёно неверное ФИО клиента");
                }
            }
            if (string.IsNullOrWhiteSpace(currentClient.Phone))
                errors.AppendLine("Укажите телефон клиента");
            else
            {
                Regex regex = new Regex("^(7|\\+7)[0-9]{10}$");
                bool isValid = regex.IsMatch(currentClient.Phone);
                if (!isValid)
                {
                    errors.AppendLine("Введён неверный номер телефона");
                }
            }
            if (string.IsNullOrWhiteSpace(currentClient.E_mail))
                errors.AppendLine("Укажите E-mail клиента");
            else
            {
                Regex regex = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
                bool isValidd = regex.IsMatch(currentClient.E_mail);
                if (!isValidd)
                {
                    errors.AppendLine("Введён неверный E-mail");
                }
            }
            if (string.IsNullOrWhiteSpace(currentClient.Adress))
                errors.AppendLine("Укажите адрес клиента");

            // Если ошибки есть, то вывод каждой с новой строки
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Добавление данных
            if (currentClient.ID == 0)
                App.db.Client.Add(currentClient);

            try
            {
                // сохранение изменений (при добавлении или редактировании данных)
                App.db.SaveChanges();
                MessageBox.Show("Информация сохранена!");
                // Переход на страницу товаров
                ProjectManager.MainFrame.Navigate(new Pages.ClientViewPage());
            }
            // Ошибка
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
