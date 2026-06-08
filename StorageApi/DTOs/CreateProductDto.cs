
using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000)]
        public int Price { get; set; }
        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string Shelf { get; set; } = string.Empty;
        [Required]
        [Range(1, 10000)]
        public int Count { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}