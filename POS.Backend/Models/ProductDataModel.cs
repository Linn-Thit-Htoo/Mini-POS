using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Backend.Models
{
    [Table("Tbl_Product")]
    public class ProductDataModel
    {
        [Key]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; } // float column in table
        public DateTime CreatedDate { get; set; }
    }
}
