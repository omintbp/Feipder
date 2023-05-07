﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Feipder.Entities.Models;

public partial class Discount
{
    [JsonIgnore]
    public int Id { get; set; }

    public int Size { get; set; }

    public TimeOnly DateStart { get; set; }

    public TimeOnly DateEnd { get; set; }

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = null!;
}