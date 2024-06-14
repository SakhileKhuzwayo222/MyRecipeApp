using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeApp
{
    internal class Recipe
    {
        private readonly List<Ingredient> ingredients;
        private readonly List<RecipeStep> steps;
        public string Name { get; private set; }

        public delegate void CaloriesExceedHandler(Recipe recipe, double totalCalories);
        public event CaloriesExceedHandler CaloriesExceeded;

        public Recipe(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Recipe name cannot be null or empty.");
            Name = name;
            ingredients = new List<Ingredient>();
            steps = new List<RecipeStep>();

        }

        public double CalculateTotalCalories()
        {
            double totalCalories = ingredients.Sum(i => i.Calories * i.Quantity);
            if (totalCalories > 300)
            {
                CaloriesExceeded?.Invoke(this, totalCalories);
            }
            return totalCalories;
        }

        public void ScaleRecipe(double factor)
        {
            foreach (var ingredient in ingredients)
            {
                ingredient.Quantity *= factor;
            }
        }

        public void ResetQuantities()
        {
            foreach (var ingredient in ingredients)
            {
                ingredient.Quantity = ingredient.OriginalQuantity;
            }
        }

        public void DisplayRecipe()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Recipe: {Name}");
            Console.ResetColor();

            if (ingredients.Count != 0)
            {
                Console.WriteLine("Ingredients:");
                foreach (var ingredient in ingredients)
                {
                    Console.WriteLine($"{ingredient.Name} - {ingredient.Quantity} {ingredient.Unit} ({ingredient.FoodGroup})");
                }
            }
            else
            {
                Console.WriteLine("No ingredients added.");
            }

            if (steps.Count != 0)
            {
                Console.WriteLine("Steps:");
                for (int i = 0; i < steps.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{i + 1}. ");
                    Console.ResetColor();
                    Console.WriteLine(steps[i].Description);
                }
            }
            else
            {
                Console.WriteLine("No steps added.");
            }
        }

        public void ClearData()
        {
            ingredients.Clear();
            steps.Clear();
        }

        public void InputRecipeDetails()
        {
            // Prompt user for the number of ingredients
            Console.Write("Enter the number of ingredients: ");
            int numIngredients;
            while (!int.TryParse(Console.ReadLine(), out numIngredients) || numIngredients <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid number greater than 0.");
                Console.Write("Enter the number of ingredients: ");
            }

            // Prompt user for ingredient details
            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"Enter details for ingredient {i + 1}:");

                Console.Write("Ingredient Name: ");
                string ingredientName = Console.ReadLine()?.Trim() ?? "";

                // Check if the user wants to finish entering ingredients
                if (string.Equals(ingredientName, "done", StringComparison.OrdinalIgnoreCase))
                    break;

                Console.Write("Ingredient Quantity: ");
                double quantity;
                while (!double.TryParse(Console.ReadLine(), out quantity))
                {
                    Console.WriteLine("Invalid quantity. Please enter a valid number.");
                    Console.Write("Ingredient Quantity: ");
                }

                Console.Write("Ingredient Unit: ");
                string unit = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Ingredient Food Group: ");
                string foodGroup = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Ingredient Calories: ");
                double calories;
                while (!double.TryParse(Console.ReadLine(), out calories))
                {
                    Console.WriteLine("Invalid calories. Please enter a valid number.");
                    Console.Write("Ingredient Calories: ");
                }

                if (!string.IsNullOrEmpty(ingredientName))
                {
                    var ingredient = new Ingredient(ingredientName, quantity, unit, calories, foodGroup);
                    ingredients.Add(ingredient);
                    Console.WriteLine("Ingredient added successfully.");
                }
            }

            // Prompt user for the number of steps
            Console.Write("Enter the number of steps: ");
            int numSteps;
            while (!int.TryParse(Console.ReadLine(), out numSteps) || numSteps <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid number greater than 0.");
                Console.Write("Enter the number of steps: ");
            }

            // Prompt user for step details
            for (int i = 0; i < numSteps; i++)
            {
                Console.WriteLine($"Enter details for step {i + 1}:");

                Console.Write("Step Description: ");
                string description = Console.ReadLine()?.Trim() ?? "";

                // Check if the user wants to finish entering steps
                if (string.Equals(description, "done", StringComparison.OrdinalIgnoreCase))
                    break;

                var step = new RecipeStep(description);
                steps.Add(step);

                Console.WriteLine("Step added successfully.");
            }
        }
    }
}
