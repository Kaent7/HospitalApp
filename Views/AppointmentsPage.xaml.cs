using HospitalApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO; // Для экспорта в TXT
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalApp.Pages
{
    public partial class AppointmentsPage : Page
    {
        public AppointmentsPage()
        {
            InitializeComponent();
            LoadData();
        }

        // --- ЛОГИКА ЗАГРУЗКИ И ФИЛЬТРАЦИИ ---
        public void LoadData()
        {
            try
            {
                using (var db = new HospitalDBEntities())
                {
                    var currentUser = Views.LoginWindow.CurrentUser;
                    if (currentUser == null) return;

                    // Базовый запрос (аналог твоего SQL JOIN)
                    var query = db.Appointments
                        .Include(a => a.Patients)
                        .Include(a => a.Doctors)
                        .AsQueryable();

                    // ТВОЯ ЛОГИКА: Если вошел врач (RoleId = 2), фильтруем по его UserId
                    if (currentUser.RoleId == 2)
                    {
                        query = query.Where(a => a.Doctors.UserId == currentUser.Id);

                        // Скрываем кнопку удаления для врачей (обычно им нельзя удалять записи)
                        btnDelete.Visibility = Visibility.Collapsed;
                    }

                    // Поиск по ФИО пациента (если в поиске что-то введено)
                    string search = txtSearch.Text.ToLower();
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(a => a.Patients.FullName.ToLower().Contains(search));
                    }

                    dgAppointments.ItemsSource = query.OrderByDescending(a => a.AppointmentDateTime).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        // --- ОБРАБОТЧИКИ КНОПОК ---

        // Поиск (срабатывает при каждом символе)
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData();
        }

        // Новая запись
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new Views.AddAppointmentWindow(); // Убедись, что путь к окну верный
            if (addWin.ShowDialog() == true)
            {
                LoadData();
            }
        }

        // Удалить запись
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgAppointments.SelectedItem as Appointments;
            if (selected == null)
            {
                MessageBox.Show("Выберите запись для удаления");
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var db = new HospitalDBEntities())
                {
                    var item = db.Appointments.Find(selected.Id);
                    if (item != null)
                    {
                        db.Appointments.Remove(item);
                        db.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        // Экспорт в отчет (TXT)
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = dgAppointments.ItemsSource as List<Appointments>;
                if (data == null || data.Count == 0) return;

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "Report_Appointments.txt");

                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"ОТЧЕТ ПО ПРИЕМАМ ОТ {DateTime.Now:dd.MM.yyyy}");
                    sw.WriteLine("------------------------------------------");
                    foreach (var item in data)
                    {
                        sw.WriteLine($"{item.AppointmentDateTime:dd.MM HH:mm} | Пациент: {item.Patients.FullName} | Статус: {item.Status}");
                    }
                }
                MessageBox.Show($"Отчет сохранен на рабочий стол!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка экспорта: " + ex.Message);
            }
        }
    }
}