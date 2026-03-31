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
using HospitalApp.Views;
using System.Data.Entity;

namespace HospitalApp.Pages
{
    public partial class DoctorsPage : Page
    {
        public DoctorsPage()
        {
            InitializeComponent();
            LoadDoctors();
            CheckAccessRights();
        }

        /// <summary>
        /// Проверка прав доступа: кнопка добавления видна только Администратору (RoleId == 1)
        /// </summary>
        private void CheckAccessRights()
        {
            // Берем текущего пользователя из статической переменной окна входа
            var user = LoginWindow.CurrentUser;

            if (user != null && user.RoleId == 1)
            {
                btnAddDoctor.Visibility = Visibility.Visible;
            }
            else
            {
                btnAddDoctor.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Загрузка списка врачей из базы данных
        /// </summary>
        private void LoadDoctors()
        {
            try
            {
                using (var db = new HospitalDBEntities()) // Ваше имя контекста
                {
                    // Загружаем врачей вместе со связанными объектами Специализация и Кабинет
                    var doctorsList = db.Doctors
                        .Include(d => d.Specializations)
                        .Include(d => d.Cabinets)
                        .OrderBy(d => d.FullName)
                        .ToList();

                    dgDoctors.ItemsSource = doctorsList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке списка врачей: " + ex.Message);
            }
        }

        private void btnAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно добавления врача
            var addWindow = new AddDoctorWindow();

            // Если окно закрылось с результатом True (сохранение прошло)
            if (addWindow.ShowDialog() == true)
            {
                LoadDoctors(); // Обновляем таблицу
            }
        }
    }
}