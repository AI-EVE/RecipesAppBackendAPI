using AngularRecipeBook.WebAPI.AppDbContext;
using AngularRecipeBook.WebAPI.DTO;
using AngularRecipeBook.WebAPI.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularRecipeBook.WebAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipesController(ApplicationDbContext context)
        {

            _context = context;

        }
  

        [HttpGet]
        public async Task<ActionResult> GetRecipes()
        {
            var recipes = await _context.Recipes.Select(recipe => new RecipeModel() { Id = recipe.Id, Description = recipe.Description, ImgUrl = recipe.ImgUrl, Name = recipe.Name }).ToListAsync();

            for (int i = 0; i < recipes.Count; i++)
            {
                recipes[i].Ingredients = await _context.Ingredients.Where(ingredient => ingredient.RecipeId == recipes[i].Id).ToListAsync();
            }

            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRecipe(Guid id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if(recipe == null)
            {
                return NotFound();
            }

            var recipeModel = new RecipeModel() { Id = recipe.Id, Description = recipe.Description, ImgUrl = recipe.ImgUrl, Name = recipe.Name };

            recipeModel.Ingredients = await _context.Ingredients.Where(ingredient => ingredient.RecipeId == recipe.Id).ToListAsync();

            return Ok(recipeModel);
        }


        [HttpPost]
        public async Task<ActionResult> CreateRecipe(RecipeModel recipeModel)
        {
            var recipe = new Recipe() {Id = Guid.NewGuid(), Name = recipeModel.Name, Description = recipeModel.Description, ImgUrl = recipeModel.ImgUrl };

            _context.Recipes.Add(recipe);

            await _context.SaveChangesAsync();



            for (int i = 0; i < recipeModel.Ingredients.Count(); i++)
            {
                var newIngredient = new Ingredient();
                newIngredient.Name = recipeModel.Ingredients[i].Name;
                newIngredient.Amount = recipeModel.Ingredients[i].Amount;
                newIngredient.RecipeId = recipe.Id;
                recipeModel.Ingredients[i].RecipeId = newIngredient.RecipeId;
                newIngredient.Id = Guid.NewGuid();
                recipeModel.Ingredients[i].Id = newIngredient.Id;

                _context.Ingredients.Add(newIngredient);
            }

            await _context.SaveChangesAsync();

            recipeModel.Id = recipe.Id;
            return Ok(recipeModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRecipe(Guid id, RecipeModel recipeModel)
        {
            if(id != recipeModel.Id)
            {
                return BadRequest();
            }

            var recipe = await _context.Recipes.FindAsync(id);

            if(recipe == null)
            {
                return NotFound();
            }

            recipe.Name = recipeModel.Name;
            recipe.Description = recipeModel.Description;
            recipe.ImgUrl = recipeModel.ImgUrl;

            await _context.SaveChangesAsync();

            var ingredients = await _context.Ingredients.Where(ingredient => ingredient.RecipeId == recipe.Id).ToListAsync();

            ingredients.ForEach(ingredient =>
            {
                _context.Ingredients.Remove(ingredient);
            });

            await _context.SaveChangesAsync();

            recipeModel.Ingredients.ForEach(ingredient =>
            {
                var newIngredient = new Ingredient();
                newIngredient.Name = ingredient.Name;
                newIngredient.Amount = ingredient.Amount;
                newIngredient.RecipeId = recipe.Id;
                newIngredient.Id = Guid.NewGuid();
              

                ingredient.Id = newIngredient.Id;
                ingredient.RecipeId = newIngredient.RecipeId;

                _context.Ingredients.Add(newIngredient);
            });

            await _context.SaveChangesAsync();

            return Ok(recipeModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(Guid id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if(recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);

            await _context.SaveChangesAsync();

            var ingredients = await _context.Ingredients.Where(ingredient => ingredient.RecipeId == recipe.Id).ToListAsync();

            ingredients.ForEach(async ingredient =>
            {
                _context.Ingredients.Remove(ingredient);

            });

            await _context.SaveChangesAsync();

            return Ok();
        }   


    }
}
