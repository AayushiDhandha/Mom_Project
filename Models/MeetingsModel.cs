using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Mom_Project.Models
{
    public class MeetingsModel
    {

        [Key]
    
        public int MeetingID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime MeetingDate { get; set; }
        public int MeetingVenueID { get; set; }

        public int MeetingTypeID { get; set; }

        public int DepartmentID { get; set; }

        [StringLength(250)]
        public string? MeetingDescription { get; set; }

        [StringLength(250)]
        public string? DocumentPath { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }

        public bool? IsCancelled { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime? CancellationDateTime { get; set; }

        [StringLength(250)]
        public String? CancellationReason { get; set; }

        
    }
}
