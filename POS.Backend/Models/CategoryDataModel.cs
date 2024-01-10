using System.ComponentModel.DataAnnotations;

namespace POS.Backend.Models
{
    public class CategoryDataModel
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
