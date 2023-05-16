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

        public CategoryTree(Category root, IList<Category> categories, bool isForwardDirection = true)
        {
            var node = new CategoryNode(root, 0);

            if (isForwardDirection)
            {
                node.SubCategories = GetSubCategories(node, categories, 1);
                Nodes.Add(node);
            }
            else
            {
                Nodes.Add(node);

                if(root.Parent != null)
                {
                    var parent = categories.Where(x => x.Id == root.Parent.Id).FirstOrDefault();

                    while (parent != null)
                    {
                        Nodes.Add(new CategoryNode(parent));

                        parent = categories.Where(x => x.Id == parent.ParentId).FirstOrDefault();
                    }

                    var nodesCount = Nodes.Count - 1;
                    for(var i = 0; i < nodesCount; i++)
                    {
                        Nodes[0].Level = nodesCount - i;

                        Nodes[1].SubCategories = new List<CategoryNode>()
                        {
                            Nodes[0]
                        };
                        Nodes.RemoveAt(0);
                    }

                    Nodes[0].Level = 0;
                }

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

        private bool Contains(int? categoryId, IList<CategoryNode> nodes)
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
