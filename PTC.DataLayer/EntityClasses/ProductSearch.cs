using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTC.DataLayer.EntityClasses
{
    public class ProductSearch
    {
        public ProductSearch() : base()
        {
            Init();
        }

        public void Init()
        {
            // Initialize all search variables
            ProductName = string.Empty;
        }

        public string ProductName { get; set; }
    }
}
