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

       // [JsonIgnore]
        [ForeignKey(nameof(RequestId))]
        public Request? Request { get; set; }

        public int RequestId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;



        
    }
}
