using System.ComponentModel.DataAnnotations;

namespace Mom_Project.Models
{
    public class MeetingTypeModel
    {
        [Key]
        public int MeetingTypeID { get; set; }

        [Required(ErrorMessage = "Meeting type is required.")]
        [StringLength(100)]
        public string MeetingTypeName { get; set; }

        [Required(ErrorMessage = "Remarks is required.")]
        [StringLength(100)]
        public string Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }
    }
}
