﻿using System;
using System.Collections.Generic;

namespace DBFirst.Models;

public partial class SprTheme
{
    public string? Themes { get; set; }

    public int Id { get; set; }

    public virtual ICollection<BooksNew> BooksNews { get; set; } = new List<BooksNew>();
}
