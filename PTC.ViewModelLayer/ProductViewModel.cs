using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTC.DataLayer;
using PTC.DataLayer.EntityClasses;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace PTC.ViewModelLayer
{
    public class ProductViewModel
    {
        public List<Product> DataCollection { get; set; }

        public List<string> Messages { get; set; }

        public string Message { get; set; }

        public ProductSearch SearchEntity { get; set; }

        public string EventAction { get; set; }

        public string EventArgument { get; set; }

        public PDSAPageModeEnum PageMode { get; set; }

        public Product Entity { get; set; }

        public bool IsValid { get; set; }
        
        public void Init()
        {
            DataCollection = new List<Product>();
            Messages = new List<string>();
            SearchEntity = new ProductSearch();
            Entity = new Product();
            Message = string.Empty;
            EventAction = string.Empty;
            EventArgument = string.Empty;
            PageMode = PDSAPageModeEnum.List;
            IsValid = true;
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

            Message = string.Empty;

            switch (EventAction)
            {
                case "add":
                    IsValid = true;
                    PageMode = PDSAPageModeEnum.Add;
                    break;
                case "edit":
                    IsValid = true;
                    PageMode = PDSAPageModeEnum.Edit;
                    GetEntity();
                    break;
                case "search":
                    PageMode = PDSAPageModeEnum.List;
                    break;
                case "resetsearch":
                    PageMode = PDSAPageModeEnum.List;
                    SearchEntity = new ProductSearch();
                    break;
                case "cancel":
                    PageMode = PDSAPageModeEnum.List;
                    break;
                case "save":
                    Save();
                    break;
                case "delete":
                    Delete();
                    break;
            }

            if (PageMode == PDSAPageModeEnum.List)
            {
                BuildCollection();

                if (DataCollection.Count == 0)
                    Message = "No Product Data Found.";
            }
        }

        protected virtual void GetEntity()
        {
            PTCData db = null;
            try
            {
                db = new PTCData();
                // Get the entity
                if (!string.IsNullOrEmpty(EventArgument))
                {
                    Entity =
                          db.Products.Find(Convert.ToInt32(EventArgument));
                }
            }
            catch(Exception ex)
            {
                Publish(ex, "Error Retrieving Product With ID=" +
                    EventArgument);
            }
        }

        protected void Insert()
        {
            PTCData db = null;
            try
            {
                db = new PTCData();
                // Do editing here
                db.Products.Add(Entity);
                db.SaveChanges();
                PageMode = PDSAPageModeEnum.List;
            }
            catch (DbEntityValidationException ex)
            {
                IsValid = false;
                ValidationErrorsToMessages(ex);
            }
            catch(Exception ex)
            {
                Publish(ex, "Error Inserting New Product: '" +
                    Entity.ProductName + "'");
            }
        }

        protected void Update()
        {
            PTCData db = null;
            try
            {
                db = new PTCData();
                // Do editing here
                db.Entry(Entity).State = EntityState.Modified;
                db.SaveChanges();
                PageMode = PDSAPageModeEnum.List;
            }
            catch (DbEntityValidationException ex)
            {
                IsValid = false;
                ValidationErrorsToMessages(ex);
            }
            catch(Exception ex)
            {
                Publish(ex, "Error Updating Product With ID=" +
                    Entity.ProductId.ToString());
            }
        }

        protected void Save()
        {
            IsValid = true;
            if (PageMode == PDSAPageModeEnum.Add)
                Insert();
            else
                Update();
        }

        public virtual void Delete()
        {
            PTCData db = null;
            try
            {
                db = new PTCData();
                if (!string.IsNullOrEmpty(EventArgument))
                {
                    Entity = db.Products.Find(Convert.ToInt32(EventArgument));
                    db.Products.Remove(Entity);
                    db.SaveChanges();
                    PageMode = PDSAPageModeEnum.List;
                }
            }
            catch (Exception ex)
            {
                Publish(ex, "Error Deleting Product With ID=" +
                    Entity.ProductName);
            }
        }

        protected void ValidationErrorsToMessages(DbEntityValidationException ex)
        {
            foreach (DbEntityValidationResult result in ex.EntityValidationErrors)
            {
                foreach (DbValidationError item in result.ValidationErrors)
                    Messages.Add(item.ErrorMessage);
            }
        }
    }
}
