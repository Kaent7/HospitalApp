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
        public RegisterWindow()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                using (var db = new HospitalDBEntities())
                {
                    // Загружаем роли для выпадающего списка
                    var roles = db.Roles.ToList();
                    cmbRoles.ItemsSource = roles;

                    if (roles.Count > 0)
                        cmbRoles.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки ролей: " + ex.Message);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // 1. Извлекаем данные
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            // 2. Валидация
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми!", "Внимание");
                return;
            }

            // ИСПРАВЛЕНО: Проверка длины пароля
            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов!", "Безопасность");
                return;
            }

            if (cmbRoles.SelectedValue == null)
            {
                MessageBox.Show("Выберите роль пользователя!");
                return;
            }

            try
            {
                using (var db = new HospitalDBEntities())
                {
                    // 3. Проверка уникальности логина
                    if (db.Users.Any(u => u.Login == login))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!");
                        return;
                    }

                    // 4. Создание нового объекта
                    var newUser = new Users
                    {
                        Login = login,
                        Password = password, // В реальных проектах тут должен быть Хэш!
                        RoleId = (int)cmbRoles.SelectedValue
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show("Регистрация завершена успешно!", "Успех");

                    // Возврат на окно входа
                    LoginWindow loginWin = new LoginWindow();
                    loginWin.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        private void ToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWin = new LoginWindow();
            loginWin.Show();
            this.Close();
        }
    }
}