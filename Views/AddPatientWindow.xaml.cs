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
    public partial class AddPatientWindow : Window
    {
        public AddPatientWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Валидация ФИО
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Пожалуйста, введите ФИО пациента!");
                return;
            }

            // 2. Валидация Даты (лучше не давать сохранять сегодняшнюю дату по ошибке)
            if (dpBirthDate.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату рождения!");
                return;
            }

            using (var db = new HospitalDBEntities())
            {
                try
                {
                    // Создаем объект. 
                    // ВНИМАНИЕ: Если класс в моделях называется Patient (без s), убери s здесь.
                    var newPatient = new Patients
                    {
                        FullName = txtFullName.Text.Trim(),
                        BirthDate = dpBirthDate.SelectedDate.Value,
                        Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? "Не указан" : txtPhone.Text.Trim(),

                        // По твоей схеме БД PolicyNumber обязателен. 
                        // Даем заглушку, если в будущем не добавишь TextBox для него.
                        PolicyNumber = "00000000"
                    };

                    db.Patients.Add(newPatient);
                    db.SaveChanges();

                    MessageBox.Show("Пациент успешно добавлен!");

                    // Установка DialogResult автоматически закроет окно и вернет 'true' в вызывающее окно
                    this.DialogResult = true;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    // Вывод конкретных ошибок полей базы (например, если PolicyNumber слишком длинный)
                    var errorMessages = dbEx.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => $"Поле: {x.PropertyName}; Ошибка: {x.ErrorMessage}");

                    string fullErrorMessage = string.Join("\n", errorMessages);
                    MessageBox.Show("Ошибка валидации данных в БД:\n" + fullErrorMessage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Критическая ошибка при сохранении: " + ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}