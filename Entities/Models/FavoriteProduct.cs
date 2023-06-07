using System;
using System.Collections.Generic;
using Feipder.Entities.Models;

namespace Feipder.Entities.Models;

public partial class FavoriteProduct
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

}
