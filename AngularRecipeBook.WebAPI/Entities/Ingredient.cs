using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularRecipeBook.WebAPI.Entities
{
    public class Ingredient
    {

        [Key]
        public Guid? Id { get; set; }
        public Guid? RecipeId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? Amount { get; set; }
    }
}
