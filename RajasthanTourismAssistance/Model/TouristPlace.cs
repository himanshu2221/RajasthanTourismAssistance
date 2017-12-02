using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RajasthanTourismAssistance.Model
{
    public class TouristPlace
    {

        public string place { set; get; }
        public int subCategoryId { set; get; }
        public int cityId { set; get; }
        public string description { set; get; }
        public string hyperlink { set; get; }
        public string imageUrl { get; set; }
    }
}