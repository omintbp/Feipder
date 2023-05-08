using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Reflection.Emit;
using System.Text.Json.Nodes;

namespace Feipder.Entities.Models.ResponseModels
{
    public class CategoryTree
    {
        private IList<CategoryNode> _nodes = new List<CategoryNode>();

        public IList<CategoryNode> Nodes { get { return _nodes; } }

        public CategoryTree(IList<Category> categories) 
        {
            foreach(var category in categories)
            {
                var node = new CategoryNode(category)
                {
                    SubCategories = GetSubCategories(category, 1)
                };

                _nodes.Add(node);
            }
        }

        private IList<CategoryNode> GetSubCategories(Category category, int level)
        {
            var result = new List<CategoryNode>();

            if (category.Children.Any())
            {
                foreach(var child in category.Children)
                {
                    var childCategory = new CategoryNode(child, level)
                    {
                        SubCategories = GetSubCategories(child, level + 1)
                    };

                    result.Add(childCategory);
                }
            }

            return result;
        }

        public bool Contains(Category category)
        {
            return Contains(category.Id, _nodes);
        }

        private bool Contains(int categoryId, IList<CategoryNode> nodes)
        {
            if(nodes.Count == 0)
            {
                return false;
            }

            foreach(var node in nodes)
            {
                if(node.Id == categoryId || Contains(categoryId, node.SubCategories))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
