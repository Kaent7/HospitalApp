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
using System.Windows.Shapes;
using HospitalApp.Models;
namespace HospitalApp.Views
{
    public partial class AddDoctorWindow : Window
    {
        public AddDoctorWindow()
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
                    // Загружаем специализации и кабинеты
                    cmbSpecialization.ItemsSource = db.Specializations.OrderBy(s => s.Title).ToList();
                    cmbCabinet.ItemsSource = db.Cabinets.OrderBy(c => c.RoomNumber).ToList();

                    // Загружаем только тех пользователей, которые еще НЕ являются врачами
                    // (Чтобы у одного аккаунта не было двух карточек врача)
                    var existingDoctorUserIds = db.Doctors.Select(d => d.UserId).ToList();
                    cmbUser.ItemsSource = db.Users
                        .Where(u => !existingDoctorUserIds.Contains(u.Id))
                        .OrderBy(u => u.Login)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки справочников: " + ex.Message);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // 1. Валидация
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО врача!");
                return;
            }

            if (cmbSpecialization.SelectedValue == null ||
                cmbCabinet.SelectedValue == null ||
                cmbUser.SelectedValue == null)
            {
                MessageBox.Show("Заполните все выпадающие списки!");
                return;
            }

            // 2. Сохранение
            try
            {
                using (var db = new HospitalDBEntities())
                {
                    var newDoctor = new Doctors
                    {
                        FullName = txtFullName.Text.Trim(),
                        SpecializationId = (int)cmbSpecialization.SelectedValue,
                        CabinetId = (int)cmbCabinet.SelectedValue,
                        UserId = (int)cmbUser.SelectedValue
                    };

                    db.Doctors.Add(newDoctor);
                    db.SaveChanges();

                    MessageBox.Show("Врач успешно добавлен в штат!");
                    this.DialogResult = true; // Это закроет окно и обновит таблицу в DoctorsPage
                }
            }
            catch (Exception ex)
            {
                // Если возникла ошибка валидации БД
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
                MessageBox.Show($"Ошибка сохранения: {ex.Message}\n{innerMsg}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}