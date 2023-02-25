using System.ComponentModel.DataAnnotations;

namespace PrsBackEnd.Models
{
    public class RequestLine
    {
        [Key]
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Productid { get; set; }
        public int Quantity { get; set; }
    }
}
