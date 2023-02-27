using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrsBackEnd.Models
{
    public class RequestLine
    {
        [Key]
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Productid { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;


       // [JsonIgnore]
        //public Request Request { get; set; }

        //public Product Product { get; set; }
    }
}
