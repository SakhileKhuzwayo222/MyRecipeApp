using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeApp
{
    internal class Ingredient
    {
        private double quantity;

        public string Name { get; private set; }
        public double Quantity
        {
            get => quantity;
            set
            {
                if (value <= 0) throw new ArgumentException("Quantity must be greater than zero.");
                quantity = value;
            }
        }
        public string Unit { get; private set; }
        public double Calories { get; private set; }
        public string FoodGroup { get; private set; }
        public double OriginalQuantity { get; private set; }

        public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Ingredient name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(unit)) throw new ArgumentException("Ingredient unit cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(foodGroup)) throw new ArgumentException("Ingredient food group cannot be null or empty.");
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
            if (calories < 0) throw new ArgumentException("Calories cannot be negative.");

            Name = name;
            Quantity = OriginalQuantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }
    }
}
