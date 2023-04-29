using System.Reflection.Emit;
using System.Text.Json.Nodes;

namespace Feipder.Models.ResponseModels
{
    public class ResponseCategoryTree
    {
        private IList<CategoryResponse> _nodes = new List<CategoryResponse>();

        public IList<CategoryResponse> Nodes { get { return _nodes; } }

        public ResponseCategoryTree(IList<Category> categories) 
        {
            foreach(var category in categories)
            {
                var node = new CategoryResponse(category)
                {
                    SubCategories = GetSubCategories(category, 1)
                };

                _nodes.Add(node);
            }
        }

        private IList<CategoryResponse> GetSubCategories(Category category, int level)
        {
            var result = new List<CategoryResponse>();

            if (category.Children.Any())
            {
                foreach(var child in category.Children)
                {
                    var childCategory = new CategoryResponse(child, level)
                    {
                        SubCategories = GetSubCategories(child, level + 1)
                    };

                    result.Add(childCategory);
                }
            }

            return result;
        }
    }
}
