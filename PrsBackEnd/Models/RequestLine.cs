using Microsoft.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrsBackEnd.Models
{
    public class RequestLine
    {
        [Key]
        public int Id { get; set; }

        public Request Request { get; set; }

        [JsonIgnore]
        public int RequestId { get; set; }

        //public User User { get; set; }

        //[JsonIgnore]
        //public int UserId { get; set; }

        public Product Product { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;



        
    }
}
