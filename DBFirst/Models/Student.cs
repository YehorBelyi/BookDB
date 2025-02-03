using System;
using System.Collections.Generic;

namespace DBFirst.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<BooksNew> Books { get; set; } = new List<BooksNew>();
}
