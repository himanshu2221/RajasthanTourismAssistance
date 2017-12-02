using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RajasthanTourismAssistance.Model
{
    public class Accommodation
    {
        public string Name { set; get; }
        public int SubCategoryID { get; set; }
        public int CityID { get; set; }
        public string Discription { set; get; }
        public string ImageUrl { set; get; }
        public string Address { set; get; }
        public int Contact { get; set; }
    }
}