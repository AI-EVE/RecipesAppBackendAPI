using System.ComponentModel.DataAnnotations;

namespace AngularRecipeBook.WebAPI.Entities
{
    public class Recipe
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public string ImgUrl { get; set; }
    }
}
