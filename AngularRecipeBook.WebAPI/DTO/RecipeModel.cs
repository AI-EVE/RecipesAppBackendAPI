using AngularRecipeBook.WebAPI.Entities;

namespace AngularRecipeBook.WebAPI.DTO
{
    public class RecipeModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImgUrl { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new();
    }
}
