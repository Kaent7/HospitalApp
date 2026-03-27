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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity; 

namespace HospitalApp.Pages
{
    public partial class AppointmentsPage : Page
    {
        public AppointmentsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new HospitalDBEntities())
            {
                // .Include нужен, чтобы подгрузить связанные таблицы (Пациента и Врача)
                // На слабом ПК это быстрее, чем делать много мелких запросов
                var list = db.Appointments
                             .Include(a => a.Patients)
                             .Include(a => a.Doctors)
                             .ToList();
                dgAppointments.ItemsSource = list;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (var db = new HospitalDBEntities())
            {
                string search = txtSearch.Text.ToLower();
                var filtered = db.Appointments
                                 .Include(a => a.Patients)
                                 .Include(a => a.Doctors)
                                 .Where(a => a.Patients.FullName.ToLower().Contains(search))
                                 .ToList();
                dgAppointments.ItemsSource = filtered;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new Views.AddAppointmentWindow();
            addWin.Owner = Window.GetWindow(this); // Чтобы окно было по центру главного
            if (addWin.ShowDialog() == true)
            {
                LoadData(); // Обновляем таблицу после сохранения
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgAppointments.SelectedItem as Appointments;
            if (selected == null)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            if (MessageBox.Show("Удалить запись?", "Вопрос", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var db = new HospitalDBEntities())
                {
                    var toDelete = db.Appointments.Find(selected.Id);
                    db.Appointments.Remove(toDelete);
                    db.SaveChanges();
                    LoadData();
                }
            }
        }
    }
}
