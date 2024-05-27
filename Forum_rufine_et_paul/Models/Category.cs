using System;
using System.Collections.Generic;

namespace Forum_rufine_et_paul.Models;

public partial class Category
{
    public int CatPk { get; set; }

    public string CatName { get; set; } = null!;

    public string CatDesc { get; set; } = null!;

    public string? CatImg { get; set; }

    public bool? CatActif { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
