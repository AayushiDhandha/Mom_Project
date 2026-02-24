using System.ComponentModel.DataAnnotations;

namespace Mom_Project.Models
{
    public class MeetingMemberModel
    {
        [Key]
   
        public int MeetingMemberID { get; set; }

        public int MeetingID { get; set; }
        public int StaffID { get; set; }

        [Required]
        public bool IsPresent { get; set; }

        [StringLength(250)]
        public string? Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; } 
    }
}
