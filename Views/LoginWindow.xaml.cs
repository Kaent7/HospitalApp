using HospitalApp.Models;
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
using System.Windows.Shapes;
using HospitalApp.Infrastructure;

namespace HospitalApp.Views
{
    public partial class LoginWindow : Window
    {
        // Статическое свойство, чтобы из любой страницы (например, HomePage) 
        // можно было написать: LoginWindow.CurrentUser.Login
        public static Users CurrentUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password; // В PasswordBox используем .Password, а не .Text

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new HospitalDBEntities())
                {
                    // Ищем пользователя в таблице Users
                    // Важно: проверь, чтобы в БД поле называлось Password, а не PasswordHash или как-то иначе
                    var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                    if (user != null)
                    {
                        CurrentUser = user; // Сохраняем сессию

                        // Переходим в главное окно
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close(); // Закрываем окно логина
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден или пароль неверен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критическая ошибка при подключении к БД: " + ex.Message, "Ошибка");
            }
        }

        private void ToRegister_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно регистрации
            RegisterWindow regWin = new RegisterWindow();
            regWin.Show();
            this.Close();
        }
    }
}