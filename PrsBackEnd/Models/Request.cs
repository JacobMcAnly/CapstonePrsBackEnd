
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrsBackEnd.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [StringLength(80)]
        public string Description { get; set; }

        [StringLength(80)]
        public string Justification { get; set; }

        [StringLength(80)]
        public string? RejectionReason { get; set; }

        [StringLength(20)]
        public string DeliveryMode { get; set; } = "Pickup";

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        public DateTime DateNeeded { get; set; } // Java return date not dateTime

        [StringLength(10)]
        public string Status { get; set; } = "New";

        [Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; }

        
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; } //data type user w/ variable 'user'

        //public List<RequestLine> RequestLines { get; set; }
    }
}
