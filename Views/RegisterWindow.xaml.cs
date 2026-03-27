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
using HospitalApp.Models;

namespace HospitalApp.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow() => InitializeComponent();

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // ... проверки на пустые поля ...

            using (var db = new HospitalDBEntities())
            {
                // 1. Проверяем логин
                if (db.Users.Any(u => u.Login == txtLogin.Text))
                {
                    MessageBox.Show("Логин занят!");
                    return;
                }

                // 2. Создаем пользователя и ЯВНО указываем ID роли
                var newUser = new Users
                {
                    Login = txtLogin.Text,
                    Password = txtPassword.Password,
                    RoleId = 1 // Укажи здесь ID роли, который точно есть в твоей таблице Roles (обычно 1 или 2)
                };

                try
                {
                    db.Users.Add(newUser);
                    db.SaveChanges(); // Теперь ошибки быть не должно
                    MessageBox.Show("Успех!");
                    new LoginWindow().Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка БД: " + ex.Message);
                }
            }
        }

        private void ToLogin_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}