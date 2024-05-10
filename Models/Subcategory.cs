using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class Subcategory : BaseEntity
    {
        [StringLength(255)]
        [Required(ErrorMessage = "Subcategory cannot be empty!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Parent category cannot be empty!")]
        public int ParentCategoryID { get; set; }
        public Category ParentCategory { get; set; }
        public int? ParentSubcategoryID { get; set; } 
        public Subcategory ParentSubcategory { get; set; } 
    }

}
