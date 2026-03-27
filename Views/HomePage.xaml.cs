using HospitalApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            UpdateDashboard();
        }

        private void UpdateDashboard()
        {
            try
            {
                using (var db = new HospitalDBEntities())
                {
                    DateTime now = DateTime.Now;
                    DateTime today = DateTime.Today;

                    // 1. Сбор общей статистики
                    // Считаем всех пациентов и врачей
                    txtTotalPatients.Text = db.Patients.Count().ToString();
                    txtTotalDoctors.Text = db.Doctors.Count().ToString();

                    // Считаем количество записей именно на сегодня
                    int countToday = db.Appointments
                        .Count(a => DbFunctions.TruncateTime(a.AppointmentDateTime) == today);
                    txtTodayApp.Text = countToday.ToString();

                    // 2. Загрузка ближайших приемов (начиная с текущего времени и до конца дня)
                    var upcoming = db.Appointments
                        .Include("Patients")
                        .Include("Doctors")
                        .Where(a => DbFunctions.TruncateTime(a.AppointmentDateTime) == today
                                    && a.AppointmentDateTime >= now)
                        .OrderBy(a => a.AppointmentDateTime)
                        .Take(10) // Показываем только первые 10 актуальных
                        .ToList();

                    dgUpcoming.ItemsSource = upcoming;
                }
            }
            catch (Exception ex)
            {
                // Если база пустая или ошибка подключения
                System.Windows.MessageBox.Show("Ошибка обновления данных: " + ex.Message);
            }
        }
    }
}