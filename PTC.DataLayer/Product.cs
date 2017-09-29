namespace PTC.DataLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public int ProductId { get; set; }
        
        public string ProductName { get; set; }

        public DateTime IntroductionDate { get; set; }
        
        public string Url { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}
