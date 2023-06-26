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
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace Pet2.Pages
{
    /// <summary>
    /// Логика взаимодействия для PetAddEditPage.xaml
    /// </summary>
    public partial class PetAddEditPage : Page
    {
        // Текущее животное
        private Pet currentPet = new Pet();
        public PetAddEditPage(Pet selectedPet)
        {
            InitializeComponent();
            // Переход на редактирование
            if (selectedPet != null)
            {
                currentPet = selectedPet;
                addbutton.Content = "Редактировать";
                if (currentPet.PetType == "Кошка") pettypeComboBox.SelectedIndex = 0;
                else pettypeComboBox.SelectedIndex = 1;
                pettypeComboBox.IsEnabled = false;
                ClientComboBox.IsEnabled = false;
                
            }

            // Получение контекста данных текущего животного
            DataContext = currentPet;
            // Получение данных от клиенте в ComboBox
            ClientComboBox.ItemsSource = App.db.Client.ToList();
        }

        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            // Ошибки
            StringBuilder errors = new StringBuilder();

            currentPet.PetType = pettypeComboBox.Text;


            // Проверки
            if (string.IsNullOrWhiteSpace(currentPet.Nickname))
                errors.AppendLine("Укажите кличку животного");
            else
            {
                Regex regex = new Regex("^[a-zA-Zа-яА-Я ]+$");
                bool isValiddd = regex.IsMatch(currentPet.Nickname);
                if (!isValiddd)
                {
                    errors.AppendLine("Введёна неверная кличка животного");
                }
            }
            if (string.IsNullOrWhiteSpace(currentPet.PetType))
                errors.AppendLine("Выберите вид животного");
            if (currentPet.Client == null)
                errors.AppendLine("Выберите владельца животного");


            // Если ошибки есть, то вывод каждой с новой строки
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Добавление данных
            if (currentPet.ID == 0)
                App.db.Pet.Add(currentPet);

            try
            {
                // сохранение изменений (при добавлении или редактировании данных)
                App.db.SaveChanges();
                MessageBox.Show("Информация сохранена!");
                // Переход на страницу товаров
                ProjectManager.MainFrame.Navigate(new Pages.PetViewPage());
            }
            // Ошибка
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void addImg_Click(object sender, RoutedEventArgs e)
        {
            // Выбранный путь
            string selectedPath = "";
            // Открытие проводника
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                // Полученение пути
                img.Source = new BitmapImage(new Uri(ofd.FileName));
                // Присваивание переменной пути
                selectedPath = ofd.FileName;

                // Генерация строчки + расширение
                string newFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(selectedPath);

                //MessageBox.Show(newFileName);

                // Директория (путь до проекта)
                string appPath = AppDomain.CurrentDomain.BaseDirectory;

                // Директория + название папки, где хранятся фотки + название фотки
                string newFilePath = System.IO.Path.Combine(appPath, "Животные", newFileName);

                // Если фотка есть в папке происходит добавление фотки из папки в бд
                if (selectedPath == newFilePath)
                {
                    currentPet.Photo = "Животные\\" + newFileName;
                }
                else
                {
                    // Если фотка, которая уже есть в папке, добавляется из других мест: ошибка, фотка уже есть в проекте
                    if (File.Exists(newFilePath))
                    {
                        MessageBox.Show("Такая фотка уже есть в проекте! \n Выберите её в папке проекта");
                        img.Source = null;
                    }
                    // Добавление новой фотки в папку проекта и в бд 
                    else
                    {

                        // Создание экземпляра
                        FileInfo fileInf = new FileInfo(selectedPath);

                        // Копирование фотки в папку проекта
                        fileInf.CopyTo(newFilePath, true);

                        // путь фотки нужный для бд

                        // запись этого пути в бд
                        currentPet.Photo = "Животные\\" + newFileName;

                        MessageBox.Show(selectedPath);

                        MessageBox.Show(fileInf.Name);
                    }
                }
            }
        }
    }
}
