using System.ComponentModel.DataAnnotations;

namespace Mom_Project.Models
{
    public class MeetingVenueModel
    {
        [Key]
 
        public int MeetingVenueID { get; set; }

        [Required(ErrorMessage = "Meeting venue is required.")]
        [StringLength(100)]
        public string MeetingVenueName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }
    }
}
