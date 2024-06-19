using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Data.Common;
using System.Web.UI.WebControls;

namespace SQLEditor
{
    class Program
    {
        public static string connectionString;

        public static bool detectedConfig = true;
        public static bool errorConnectionWithConfig = false;

        public static string login;
        public static string password;
        public static string nameTable;

        [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
#pragma warning disable
        static void Main(string[] args)
        {
            Console.WriteLine("Не выносите в отдельные таблицы такие поля, как - Surname, Firstname, Patronymic, Lastname, Phone, Email, id\n");
            authUser();

            using (SqlConnection Con = new SqlConnection(connectionString))
            {
                try
                {
                    Console.Title = "Получение списка таблиц";
                    Con.Open();

                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM sys.tables", Con);

                    int tableCount = (int)command.ExecuteScalar();

                    string[] arrayTName = new string[tableCount];

                    command = new SqlCommand("SELECT name FROM sys.tables", Con);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\nСписок таблиц в базе данных:");
                        int i = 0;
                        while (reader.Read())
                        {

                            string tableName = reader.GetString(0);
                            if (tableName != "sysdiagrams")
                            {
                                arrayTName[i] = tableName;
                                i++;
                                Console.WriteLine(tableName);
                            }
                        }
                    }

                    checkTable(arrayTName);

                    Con.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("\nОшибка при открытии подключения: " + ex.Message);
                    authUser();
                }
            }

            Console.ReadLine();
        }

#pragma warning disable
        public static void checkTable(string[] tableName)
        {

            using (SqlConnection Con = new SqlConnection(connectionString))
            {
                Con.Open();
                for (int i = 0; i < tableName.Length; i++)
                {
                    if (tableName[i] == null)
                    {
                        break;
                    }
                    Console.WriteLine($"\n-Анализирую таблицу - {tableName[i]}");
                    Console.Title = $"\nАнализирую таблицу - {tableName[i]}";

                    SqlCommand command = new SqlCommand($"SELECT count(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName[i]}' AND TABLE_CATALOG = '{nameTable}'", Con);

                    int columnsCount = (int)command.ExecuteScalar();

                    string[] columnsName = new string[columnsCount];

                    command = new SqlCommand($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName[i]}' AND TABLE_CATALOG = '{nameTable}'", Con);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int index = 0;
                        while (reader.Read())
                        {
                            string cName = reader.GetString(0);
                            columnsName[index] = cName;
                            index++;
                        }
                    }

                    command = new SqlCommand($"SELECT COUNT(*) FROM [{tableName[i]}]", Con);
                    int countId = (int)command.ExecuteScalar();

                    for (int j = 0; j < columnsCount; j++)
                    {
                        Console.Write($"--Анализирую столбец - {columnsName[j]}");
                        Console.Title = $"\nАнализирую таблицу - {tableName[i]}, столбец - {columnsName[j]}";

                        command = new SqlCommand($"SELECT COUNT(DISTINCT([{columnsName[j]}])) FROM [{tableName[i]}] WHERE [{columnsName[j]}] IS NOT NULL", Con);
                        int countDistinct = (int)command.ExecuteScalar();

                        if (countDistinct < countId)
                        {
                            Console.Write($"\n\nПредлагаю вынести столбец - {columnsName[j]} из таблицы - {tableName[i]} в отдельную таблицу (Y / N) ");
                            string answer = Console.ReadLine();
                            if (answer == "Y" || answer == "y" || answer == "н" || answer == "Н")
                            {
                                string[] distinctParametres = new string[countDistinct];

                                command = new SqlCommand($"select distinct [{columnsName[j]}] from [{tableName[i]}]  WHERE [{columnsName[j]}] IS NOT NULL", Con);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    int index = 0;
                                    while (reader.Read())
                                    {

                                        string cName = reader.GetString(0);
                                        distinctParametres[index] = cName;
                                        index++;
                                    }
                                }

                                command = new SqlCommand($@"CREATE TABLE [{columnsName[j]}]([ID] [int] IDENTITY(1,1) NOT NULL,[Title] [nvarchar](255) NOT NULL, CONSTRAINT [PK_{columnsName[j]}] PRIMARY KEY CLUSTERED ([ID] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY]", Con);
                                command.ExecuteScalar();

                                for (int k = 0; k < distinctParametres.Length; k++)
                                {
                                    command = new SqlCommand($"INSERT INTO [{columnsName[j]}]([Title]) VALUES('{distinctParametres[k]}')", Con);
                                    command.ExecuteScalar();
                                }

                                command = new SqlCommand($@"UPDATE [{tableName[i]}] SET [{tableName[i]}].[{columnsName[j]}] = [{columnsName[j]}].ID From [{columnsName[j]}] Where [{tableName[i]}].[{columnsName[j]}] = [{columnsName[j]}].Title", Con);
                                command.ExecuteScalar();

                                command = new SqlCommand($@"ALTER TABLE [{tableName[i]}] ALTER COLUMN [{columnsName[j]}] int;",Con);
                                command.ExecuteScalar();
                                command = new SqlCommand($@"ALTER TABLE [{tableName[i]}]  WITH CHECK ADD  CONSTRAINT [FK_{tableName[i]}_{columnsName[j]}] FOREIGN KEY([{columnsName[j]}]) REFERENCES [{columnsName[j]}] ([ID]) ON UPDATE CASCADE ON DELETE CASCADE", Con);
                                command.ExecuteScalar();


                                Console.Write($"Успешно.\n");
                            }
                        }
                        else
                        {
                            Console.Write($". Не обнаружено повторяющихся элементов в выбранном столбце.\n");
                        }
                    }
                }
            }

            Console.WriteLine("\nАнализ окончен. База данных приведена к 3 нормальной форме.");
            Console.Title = $"\nБаза данных приведена к 3 нормальной форме.";

        }
        #pragma warning disable
        public static void checkConfig()
        {
            Console.Title = "Проверка Config файла";
            string filePath = "config.txt";
            string fileContents;
            if (File.Exists(filePath))
            {
                try
                {
                    fileContents = File.ReadAllText(filePath);
                    string pattern = @"login:(.*?);password:(.*?);";

                    Match match = Regex.Match(fileContents, pattern);

                    if (match.Success)
                    {
                        string loginValue = match.Groups[1].Value;
                        string passwordValue = match.Groups[2].Value;

                        if (loginValue == string.Empty || passwordValue == string.Empty)
                        {
                            Console.WriteLine("\nConfig файл был изменен или повреждён");
                            detectedConfig = false;
                        }
                        else
                        {
                            login = loginValue;
                            password = passwordValue;
                            detectedConfig = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nConfig файл был изменен или повреждён");
                        detectedConfig = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nConfig файл был изменен или повреждён - ", ex.Message);
                    detectedConfig = false;
                }
            }
            else
            {
                Console.WriteLine("Config не был обнаружен, он создастся автоматически после авторизации.\n");
                detectedConfig = false;
            }
        }
#pragma warning disable
        public static void authUser()
        {
            Console.Title = "Авторизация";
            checkConfig();
            Console.Title = "Авторизация";
            if (!detectedConfig || errorConnectionWithConfig)
            {
                Console.Write("Введите логин - ");
                login = Console.ReadLine();
                Console.Write("Введите пароль - ");
                password = Console.ReadLine();
            }
            else
            {
                Console.Write($"Продолжить под {login}? (Y / N) ");
                string answer = Console.ReadLine();
                if (answer != "Y" && answer != "y" && answer != "н" && answer != "Н")
                {
                    File.Delete("config.txt");
                    Console.WriteLine("\nConfig файл был удалён.");
                    detectedConfig = false;
                    authUser();
                }
            }
            Console.Write("Введите название базы данных - ");
            nameTable = Console.ReadLine();

            string pattern = @"20\.102k-(\d{1,2})";
            Match match = Regex.Match(login, pattern);

            if (!match.Success)
            {
                Console.WriteLine("Недопустимый логин. Попробуйте ещё раз.\n");
                authUser();
                return;
            }
            try
            {
                connectionString =  $@"Data Source=DESKTOP-4HS179K\KKSKFI;Initial Catalog={nameTable};User ID={login};Password={password}";
                using (SqlConnection Con = new SqlConnection(connectionString))
                {
                    try
                    {
                        Con.Open();

                        if (Con.State == ConnectionState.Open)
                        {
                            Console.WriteLine("\nПодключение успешно открыто.");
                            string filePath = "config.txt";
                            File.Delete("config.txt");
                            string text = $"login:{login};password:{password};";
                            File.AppendAllText(filePath, text);
                        }
                        else
                        {
                            Console.WriteLine("\nНе удалось открыть подключение.");
                            authUser();
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("\nОшибка при открытии подключения: " + ex.Message);
                        authUser();
                    }
                    Con.Close();
                }
            }
            catch
            {
                Console.WriteLine("\nНеверный логин или пароль. Попробуйте ещё раз.\n");
                authUser();
            }
        }
    }
}