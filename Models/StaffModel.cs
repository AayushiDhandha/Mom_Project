using System.ComponentModel.DataAnnotations;

namespace Mom_Project.Models
{
    public class StaffModel
    {
        [Key]
        public int StaffID { get; set; }

        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(50)]
        public string StaffName { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [StringLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid mobile number.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string EmailAddress { get; set; }

        [StringLength(250)]
        public string? Remarks { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }
    }
}
