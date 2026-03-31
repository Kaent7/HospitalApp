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
using HospitalApp.Models;

namespace HospitalApp.Views
{
    public partial class AddAppointmentWindow : Window
    {
        public AddAppointmentWindow()
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
                    // Загружаем данные. Убедись, что в моделях Patients и Doctors есть свойство FullName
                    cbPatients.ItemsSource = db.Patients.ToList();
                    cbDoctors.ItemsSource = db.Doctors.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке списков: " + ex.Message);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (dpDate.SelectedDate == null) { MessageBox.Show("Выберите дату!"); return; }
            if (cbPatients.SelectedItem == null) { MessageBox.Show("Выберите пациента!"); return; }
            if (cbDoctors.SelectedItem == null) { MessageBox.Show("Выберите врача!"); return; }

            using (var db = new HospitalDBEntities())
            {
                try
                {
                    // Получаем выбранные объекты
                    var selectedPatient = cbPatients.SelectedItem as Patients;
                    var selectedDoctor = cbDoctors.SelectedItem as Doctors;

                    var newApp = new Appointments
                    {
                        // Убедись, что имя свойства в БД именно AppointmentDate (по твоей схеме там AppointmentDate)
                        AppointmentDateTime = dpDate.SelectedDate.Value,
                        PatientId = selectedPatient.Id,
                        DoctorId = selectedDoctor.Id,
                        Status = txtStatus.Text.Trim()
                    };

                    db.Appointments.Add(newApp);
                    db.SaveChanges();

                    MessageBox.Show("Запись успешно создана!");
                    this.DialogResult = true;
                    this.Close();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    // Выводим детальные ошибки, если база "капризничает"
                    string msg = "Ошибка БД:\n";
                    foreach (var eve in dbEx.EntityValidationErrors)
                        foreach (var ve in eve.ValidationErrors)
                            msg += $"- {ve.PropertyName}: {ve.ErrorMessage}\n";
                    MessageBox.Show(msg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения: " + ex.Message);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}