using System;
using System.Collections.Generic;

namespace Feipder.Models;

public partial class Delivery
{
    public int Id { get; set; }

    public string City { get; set; } = null!;

    public string MailIndex { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string House { get; set; } = null!;

    public string Flat { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
