using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Rack
{
    public int RackId { get; set; }

    public string Code { get; set; } = null!;

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}
