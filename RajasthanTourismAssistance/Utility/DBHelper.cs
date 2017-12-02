using MySql.Data.MySqlClient;
using RajasthanTourismAssistance.Model;
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
            string sql = "SELECT * FROM Category where CategoryName=@1";
            int categoryID = GetID(param, sql, "CategoryID");
            return categoryID;
        }

        public int GetSubCategoryID(string param)
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

        public List<Category> GetCategories()
        {
            String sql = "SELECT * FROM Category";
            List<Category> categoryList = new List<Category>();

            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["RajasthanTourismDB_ConnectionString"].ConnectionString;
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    //command.Parameters.AddWithValue("@1", cityName);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Category category = new Category();
                        category.categoryName = reader["CategoryName"].ToString();
                        category.imageUrl = reader["ImageUrl"].ToString();
                        categoryList.Add(category);
                    }
                    reader.Close();
                }
            }
            return categoryList;
        }


        public List<SubCategory> GetSubCategories(int categoryID)
        {
            String sql = "SELECT * FROM SubCategory where CategoryID=@1";
            List<SubCategory> subCategoryList = new List<SubCategory>();

            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["RajasthanTourismDB_ConnectionString"].ConnectionString;
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@1",categoryID);
                        MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SubCategory subCategory = new SubCategory();
                        subCategory.subCategoryName = reader["SubCategoryName"].ToString();
                        subCategory.imageUrl = reader["ImageUrl"].ToString();

                        subCategory.subCategoryId = Convert.ToInt32(reader["SubCategoryID"]);
                        subCategory.categoryId = Convert.ToInt32(reader["CategoryID"]);

                        subCategoryList.Add(subCategory);
                    }
                    reader.Close();
                }
            }
            return subCategoryList;
        }


        public List<TouristPlace> GetTouristPlaces(int subCategoryID, int cityID)
        {
            String sql = "SELECT * FROM Tourist_Place where SubCategoryID=@1 AND CityID=@2";
            List<TouristPlace> touristPlaceList = new List<TouristPlace>();

            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["RajasthanTourismDB_ConnectionString"].ConnectionString;
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@1", subCategoryID);
                    command.Parameters.AddWithValue("@2", cityID);

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        TouristPlace touristPlace = new TouristPlace();
                        touristPlace.place = reader["Place"].ToString();
                        touristPlace.imageUrl = reader["ImageUrl"].ToString();
                        touristPlace.description = reader["Description"].ToString();
                        touristPlace.hyperlink = reader["Hyperlink"].ToString();
                        touristPlaceList.Add(touristPlace);
                    }
                    reader.Close();
                }
            }
            return touristPlaceList;
        }

        //public List<Accommodation> GetHotelsPlaces(int subCategoryID, int cityID)
        //{
        //    String sql = "SELECT * FROM Accommodaton where SubCategoryID=@1 AND CityID=@2";
        //    List<Accommodation> accommodationList = new List<Accommodation>();

        //    using (MySqlConnection conn = new MySqlConnection())
        //    {
        //        conn.ConnectionString = ConfigurationManager.ConnectionStrings["RajasthanTourismDB_ConnectionString"].ConnectionString;
        //        conn.Open();
        //        using (MySqlCommand command = new MySqlCommand(sql, conn))
        //        {
        //            command.Parameters.AddWithValue("@1", subCategoryID);
        //            command.Parameters.AddWithValue("@2", cityID);

        //            MySqlDataReader reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Accommodation accommodation = new Accommodation();
        //                accommodation.Name = reader["Name"].ToString();
        //                accommodation.Name = reader["ImageUrl"].ToString();
        //                accommodation.Discription = reader["Description"].ToString();
        //                accommodation.Address = reader["Address"].ToString();
        //                accommodationList.Add(accommodation);
        //            }
        //            reader.Close();
        //        }
        //    }
        //    return accommodationList;
        //}


    }
}