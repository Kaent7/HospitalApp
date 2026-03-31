using HospitalApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

                    var currentUser = Views.LoginWindow.CurrentUser;
                    if (currentUser == null) return;

                    txtTotalPatients.Text = db.Patients.Count().ToString();
                    txtTotalDoctors.Text = db.Doctors.Count().ToString();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка обновления панели управления: " + ex.Message);
            }
        }
    }
}