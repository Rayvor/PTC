using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTC.DataLayer;
using PTC.DataLayer.EntityClasses;
using System.Collections.Specialized;

namespace PTC.ViewModelLayer
{
    public class ProductViewModel
    {
        public List<Product> DataCollection { get; set; }

        public string Message { get; set; }

        public ProductSearch SearchEntity { get; set; }

        public string EventAction { get; set; }

        public void Init()
        {
            DataCollection = new List<Product>();
            SearchEntity = new ProductSearch();
            Message = string.Empty;
            EventAction = string.Empty;
        }

        public ProductViewModel() : base()
        {
            Init();
        }

        public void Publish(Exception ex, string message)
        {
            Publish(ex, message, null);
        }

        public void Publish(Exception ex, string message,
            NameValueCollection nvc)
        {
            //Update view model propetions
            Message = message;
            //TODO: Publish exception here
        }

        protected void BuildCollection()
        {
            PTCData db = null;
            try
            {
                db = new PTCData();
                DataCollection = db.Products.ToList();

                if (DataCollection != null && DataCollection.Count > 0)
                {
                    if (!string.IsNullOrEmpty(SearchEntity.ProductName))
                    {
                        //DataCollection = DataCollection.FindAll(
                        //    p => p.ProductName.
                        //    StartsWith(SearchEntity.ProductName,
                        //        StringComparison.InvariantCultureIgnoreCase));

                        DataCollection = DataCollection.FindAll(
                            p => p.ProductName.IndexOf(
                                SearchEntity.ProductName, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                }
            }
            catch(Exception ex)
            {
                Publish(ex, "Error while loading products.");
            }
        }

        public void HandleRequest()
        {
            // Make sure we have a valid event command
            EventAction = (EventAction == null ? "" :
                            EventAction.ToLower());

            switch (EventAction)
            {
                case "search":
                    break;
                case "resetsearch":
                    SearchEntity = new ProductSearch();
                    break;
            }

            BuildCollection();

            if (DataCollection.Count == 0)
                Message = "No Product Data Found.";
        }
    }
}
