﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyRecipeApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Recipe> Recipes { get; set; }
        public ObservableCollection<Ingredient> NewIngredients { get; set; }
        public ObservableCollection<RecipeStep> NewSteps { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Recipes = new ObservableCollection<Recipe>();
            NewIngredients = new ObservableCollection<Ingredient>();
            NewSteps = new ObservableCollection<RecipeStep>();

            // Set the ItemsSource for the ListBox controls
            RecipeListBox.ItemsSource = Recipes;
            NewIngredientListBox.ItemsSource = NewIngredients;
            NewStepsListBox.ItemsSource = NewSteps;
        }

        // Event handler for Add Recipe button click
        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            var recipeName = NewRecipeNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(recipeName))
            {
                MessageBox.Show("Recipe name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newRecipe = new Recipe(recipeName);
            foreach (var ingredient in NewIngredients)
            {
                newRecipe.AddIngredient(ingredient);
            }
            foreach (var step in NewSteps)
            {
                newRecipe.AddStep(step);
            }

            Recipes.Add(newRecipe);
            MessageBox.Show("Recipe added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            NewRecipeNameTextBox.Clear();
            NewIngredients.Clear();
            NewSteps.Clear();
        }

        // Event handler for Edit Recipe button click
        private void EditRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRecipe = RecipeListBox.SelectedItem as Recipe;
            if (selectedRecipe != null)
            {
                var editDialog = new EditRecipeDialog(selectedRecipe);
                if (editDialog.ShowDialog() == true)
                {
                    ResetQuantities(selectedRecipe);
                    ScaleRecipe(selectedRecipe, editDialog.ScaleFactor);
                    // Optionally, save changes to recipe
                    //selectedRecipe.Save();
                }
            }
        }

        // Method to reset quantities of ingredients in a recipe
        private void ResetQuantities(Recipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.Quantity = ingredient.OriginalQuantity;
            }
        }

        // Method to scale quantities of ingredients in a recipe
        private void ScaleRecipe(Recipe recipe, double factor)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.Quantity *= factor;
            }
        }

        // Event handler for Delete Recipe button click
        private void DeleteRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRecipe = RecipeListBox.SelectedItem as Recipe;
            if (selectedRecipe != null)
            {
                Recipes.Remove(selectedRecipe);
                MessageBox.Show("Recipe deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Event handler for Filter button click
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var ingredientFilter = FilterTextBox.Text.ToLower();
            var selectedFoodGroup = FoodGroupComboBox.SelectedItem?.ToString();
            var maxCaloriesFilter = int.TryParse(MaxCaloriesTextBox.Text, out int maxCalories) ? (int?)maxCalories : null;

            var filteredRecipes = Recipes.Where(r =>
                (string.IsNullOrEmpty(ingredientFilter) || r.Ingredients.Any(i => i.Name.ToLower().Contains(ingredientFilter))) &&
                (string.IsNullOrEmpty(selectedFoodGroup) || r.Ingredients.Any(i => i.FoodGroup == selectedFoodGroup)) &&
                (!maxCaloriesFilter.HasValue || r.CalculateTotalCalories() <= maxCaloriesFilter)).ToList();

            RecipeListBox.ItemsSource = filteredRecipes;
        }

        // Event handler for RecipeListBox selection change
        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRecipe = RecipeListBox.SelectedItem as Recipe;
            if (selectedRecipe != null)
            {
                RecipeNameTextBlock.Text = selectedRecipe.Name;
                IngredientListBox.ItemsSource = selectedRecipe.Ingredients;
                StepsListBox.ItemsSource = selectedRecipe.Steps;
                TotalCaloriesTextBlock.Text = selectedRecipe.CalculateTotalCalories().ToString();
            }
        }
    }
}


