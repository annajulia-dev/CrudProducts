using System.ComponentModel.DataAnnotations;

namespace ProductsCrud.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
