using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF/*.StoreFrontMetadata*/
{
    public class EmployeeMetaData
    { 

    public int EmployeeID { get; set; }
    [Required]
    [Display(Name = "First Name")]
    [StringLength(25, ErrorMessage = "Max 25 Characters")]
    public string FirstName { get; set; }
    [Required]
    [Display(Name = "Last Name")]
    [StringLength(25, ErrorMessage = "Max 25 Characters")]
    public string LastName { get; set; }
    [Required]
    [Display(Name = "Title")]
    [StringLength(25, ErrorMessage = "Max 25 Characters")]
    public string Title { get; set; }
    public Nullable<int> DirectReportID { get; set; }
    public Nullable<int> DepartmentID { get; set; }

    }

    [MetadataType(typeof(EmployeeMetaData))]
    public partial class Employee
    {
        public string fullName
        {
            get { return FirstName + " " + LastName; }
        }
    }

    public class ProductMetaData
    {
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        [StringLength(25, ErrorMessage = "Max 25 Characters")]
        public string ProductName { get; set; }
        public Nullable<decimal> Price { get; set; }
        [Required]
        public int StockStatus { get; set; }
        [Required]
        public int ManufacturerID { get; set; }
        [Required]
        public int CatagorieID { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }

    }

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product { }
}
