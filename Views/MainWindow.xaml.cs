using HospitalApp.Infrastructure;
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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.HomePage());
            txtUserRole.Text = $"Вы вошли как: {Views.LoginWindow.CurrentUser.Login}";
            MainFrame.Navigate(new Pages.HomePage());
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.Name)
            {
                case "btnHome":
                    MainFrame.Navigate(new Pages.HomePage());
                    break;
                case "btnAppointments":
                    MainFrame.Navigate(new Pages.AppointmentsPage());
                    break;
                case "btnPatients":
                    MainFrame.Navigate(new Pages.PatientsPage());
                    break;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
