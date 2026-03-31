using HospitalApp.Models;
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

namespace HospitalApp.Pages
{
    public partial class PatientsPage : Page
    {
        public PatientsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var db = new HospitalDBEntities())
                {
                    // Загружаем список всех пациентов, сортируем по алфавиту
                    dgPatients.ItemsSource = db.Patients.OrderBy(p => p.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно добавления
            var addWindow = new AddPatientWindow();

            // Указываем владельца, чтобы окно было по центру
            addWindow.Owner = Window.GetWindow(this);

            if (addWindow.ShowDialog() == true)
            {
                LoadData(); // Обновляем список, если сохранение прошло успешно
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (var db = new HospitalDBEntities())
            {
                string search = txtSearch.Text.Trim().ToLower();

                // Фильтрация с защитой от null в поле FullName
                dgPatients.ItemsSource = db.Patients
                    .Where(p => p.FullName != null && p.FullName.ToLower().Contains(search))
                    .OrderBy(p => p.FullName)
                    .ToList();
            }
        }
    }
}