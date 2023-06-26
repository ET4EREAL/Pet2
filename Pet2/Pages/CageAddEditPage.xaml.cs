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
    /// Логика взаимодействия для CageAddEditPage.xaml
    /// </summary>
    public partial class CageAddEditPage : Page
    {
        // Текущий вольер
        private Cage currnetCage = new Cage();
        public CageAddEditPage(Cage selectedCage)
        {
            InitializeComponent();
            // Переход на редактирование
            if (selectedCage != null)
            {
                currnetCage = selectedCage;
                addbutton.Content = "Редактировать";
            }


            // Получение контекста данных текущего продукта
            DataContext = currnetCage;
        }

        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            // Ошибки
            StringBuilder errors = new StringBuilder();

            // Проверки
            if (int.TryParse(currnetCage.Number, out var number))
            {
                if (string.IsNullOrWhiteSpace(currnetCage.Number))
                    errors.AppendLine("Укажите номер вольера");
            }
            else
            {
                errors.AppendLine("Укажите номер вольера");
            }

            // Если ошибки есть, то вывод каждой с новой строки
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Добавление данных
            if (currnetCage.ID == 0)
                App.db.Cage.Add(currnetCage);

            try
            {
                // сохранение изменений (при добавлении или редактировании данных)
                App.db.SaveChanges();
                MessageBox.Show("Информация сохранена!");
                // Переход на страницу товаров
                ProjectManager.MainFrame.Navigate(new Pages.CagePage());
            }
            // Ошибка
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
