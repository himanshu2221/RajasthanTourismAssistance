using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RajasthanTourismAssistance.Utility
{
    public class DBHelper
    {
        private DBHelper() { }

        public static readonly DBHelper Instance = new DBHelper();   //Singleton concept => can create only 1 object; Instance is a property

        protected int id = 0;

        public int GetCityID(string param)
        {

            string sql = "SELECT * FROM City where CityName=@1";
            int cityID = GetID(param, sql, "CityID");
            return cityID;
        }

        public int GetCategoryID(string param)
        {
            string sql = "SELECT * FROM SubCategory where SubCategoryName=@1";
            int categoryID = GetID(param, sql, "CategoryID");
            return categoryID;
        }

        public int GetSubCategory(string param)
        {
            string sql = "SELECT * FROM SubCategory where SubCategoryName=@1";
            int subCategoryID = GetID(param, sql, "SubCategoryID");
            return subCategoryID;
        }


        public int GetID(string param, string sql, string columnName)
        {
            int id = 0;
            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["RajasthanTourismDB_ConnectionString"].ConnectionString;
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@1", param);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        id = Convert.ToInt32(reader[columnName]);
                    }
                    reader.Close();
                }
            }
            return id;
        }

        




    }
}