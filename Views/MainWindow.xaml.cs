using HospitalApp.Infrastructure;
using HospitalApp.Views;
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

namespace HospitalApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Устанавливаем стартовую страницу
            MainFrame.Navigate(new Pages.HomePage());
            CheckAccessRights();
        }

        private void CheckAccessRights()
        {
            var user = LoginWindow.CurrentUser;

            if (user == null) return;

            if (user.RoleId == 1)
            {
                btnDoctors.Visibility = Visibility.Visible;
            }
            else
            {
                btnDoctors.Visibility = Visibility.Collapsed;
            }
            if (user.RoleId == 1 || user.RoleId == 3)
            {
                btnPatients.Visibility = Visibility.Visible;
            }
            else
            {
                btnPatients.Visibility = Visibility.Collapsed;
            }
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn)) return;
            var user = LoginWindow.CurrentUser;

            switch (btn.Name)
            {
                case "btnHome":
                    MainFrame.Navigate(new Pages.HomePage());
                    break;
                case "btnAppointments":
                    MainFrame.Navigate(new Pages.AppointmentsPage());
                    break;
                case "btnPatients":
                    // Проверка: только админ и регистратор
                    if (user.RoleId == 1 || user.RoleId == 3)
                        MainFrame.Navigate(new Pages.PatientsPage());
                    else
                        MessageBox.Show("Доступ к разделу пациентов ограничен.");
                    break;
                case "btnDoctors":
                    // Проверка: только админ
                    if (user.RoleId == 1)
                        MainFrame.Navigate(new Pages.DoctorsPage());
                    else
                        MessageBox.Show("У вас нет прав для управления персоналом.");
                    break;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти из системы?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Views.LoginWindow login = new Views.LoginWindow();
                login.Show();
                this.Close(); 
            }
        }
    }
}
