using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalApp.Models;

namespace HospitalApp.Infrastructure
{
    public static class AuthService
    {
        // Здесь будет храниться объект пользователя, который ввел верный пароль
        public static Users CurrentUser { get; set; }

        // Удобные проверки для интерфейса
        public static bool IsAdmin => CurrentUser?.RoleId == 1; // 1 - это ID Админа в нашей БД
        public static bool IsDoctor => CurrentUser?.RoleId == 2; // 2 - ID Врача

        // Метод для выхода из системы
        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}