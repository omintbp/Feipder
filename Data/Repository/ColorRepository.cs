﻿using Feipder.Entities;
using Feipder.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Feipder.Data.Repository
{
    public class ColorRepository : RepositoryBase<Color>, IColorRepository
    {
        public ColorRepository(DataContext context) : base(context)
        {

        }

        public IEnumerable<Color> FindByProduct(Product product)
            => RepositoryContext.Colors.Include(x => x.Products).Where(x => x.Products.Contains(product)).ToList();
    }
}
