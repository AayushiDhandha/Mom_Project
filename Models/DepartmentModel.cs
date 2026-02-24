using System.ComponentModel.DataAnnotations;

namespace Mom_Project.Models
{
    public class DepartmentModel
    {

        [Key]
      
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters")] 
        public string DepartmentName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Created {  get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }

    }
}
