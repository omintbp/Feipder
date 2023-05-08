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
            var firstLevel = categories.Where(x => x.Parent == null).Select(x => new CategoryNode(x));

            foreach(var category in firstLevel)
            {
                category.SubCategories = GetSubCategories(category, categories, 1);

                _nodes.Add(category);
            }
        }

        private IList<CategoryNode> GetSubCategories(CategoryNode category, IList<Category> categories, int level)
        {
            var subCategories = categories.Where(x => x.ParentId == category.Id).Select(x => new CategoryNode(x, level)).ToList();

            if (!subCategories.Any())
            {
                return new List<CategoryNode>();
            }

            foreach(var item in subCategories)
            {
                item.SubCategories = GetSubCategories(item, categories, level + 1);
            }

            return subCategories.ToList();
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
