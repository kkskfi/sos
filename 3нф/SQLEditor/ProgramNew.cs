using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLEditor
{
    class Program
    {
        public static string con = @"Data Source=10.137.203.94,1433\SQLEXPRESS;Initial Catalog=20.102k-14-PR8;User ID=20.102k-14;Password=SnnmCK12345";
        static void Main(string[] args)
        {
            string column = "Compoud";
            string table = "Cloth";
            string table2 = "Compoud";
            int count = 0;
            using (SqlConnection Con = new SqlConnection(con))
            {
                Con.Open();
                string query = $@"select count(distinct {column}) from {table}";
                SqlCommand command = new SqlCommand(query, Con);
                count = int.Parse(command.ExecuteScalar().ToString());
                string[] array = new string[count];
                int j = 0;
                query = $@"select distinct {column} from {table}";
                command = new SqlCommand(query, Con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        array[j] = reader[0].ToString();
                        j++;
                    }
                }
                query = $@"delete from {table} where {column} is null";
                command = new SqlCommand(query, Con);
                command.ExecuteScalar();
                for (int i = 0; i < array.Length; i++)
                {
                     query = $@"INSERT INTO {table2}([Title]) VALUES('{array[i]}')";
                     command = new SqlCommand(query, Con);
                    command.ExecuteScalar();
                }
                for (int i = 0; i < array.Length; i++)
                {
                     query = $@"UPDATE {table} SET {table}.{column} = {table2}.ID From {table2} Where {table}.{column} = {table2}.Title";
                     command = new SqlCommand(query, Con);
                    command.ExecuteScalar();
                }
                Con.Close();
            }
        }
    }
}
