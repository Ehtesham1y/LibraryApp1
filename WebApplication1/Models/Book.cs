using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string BookName { get; set; } = null!;

    public string Author { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public decimal Price { get; set; }

    public int ShelfId { get; set; }

    public int PublishYear { get; set; }

    public DateTime CreatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Shelf Shelf { get; set; } = null!;
}
