using System;
using System.Collections.Generic;
using Feipder.Entities.Models;

namespace Feipder.Entities.Models;

public partial class FavoriteProduct
{
    public int Id { get; set; }

    public virtual Product Product { get; set; } = null!;

}
