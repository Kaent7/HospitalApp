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
            using (var db = new HospitalDBEntities())
            {
                cbPatients.ItemsSource = db.Patients.ToList();
                cbDoctors.ItemsSource = db.Doctors.ToList();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate == null || cbPatients.SelectedItem == null || cbDoctors.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            using (var db = new HospitalDBEntities())
            {
                var newApp = new Appointments
                {
                    AppointmentDateTime = dpDate.SelectedDate.Value,
                    PatientId = (cbPatients.SelectedItem as Patients).Id,
                    DoctorId = (cbDoctors.SelectedItem as Doctors).Id,
                    Status = txtStatus.Text
                };

                db.Appointments.Add(newApp);
                db.SaveChanges();
            }
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
