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
            using (var db = new HospitalDBEntities())
            {
                dgPatients.ItemsSource = db.Patients.ToList();
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (var db = new HospitalDBEntities())
            {
                string search = txtSearch.Text.ToLower();
                dgPatients.ItemsSource = db.Patients
                    .Where(p => p.FullName.ToLower().Contains(search))
                    .ToList();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Сюда потом привяжем окно добавления пациента
            MessageBox.Show("Функция в разработке");
        }
    }
}
