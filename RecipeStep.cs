using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeApp
{
   internal class RecipeStep
    {
        public string Description { get; private set; }

        public RecipeStep(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be null or empty.");
            Description = description;
        }
    }
}
