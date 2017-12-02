using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RajasthanTourismAssistance.Model
{
    public class SubCategory
    {
        public int subCategoryId { get; set; }
        public string subCategoryName { get; set; }
        public int categoryId { get; set; }
        public string imageUrl { get; set; }
    }
}