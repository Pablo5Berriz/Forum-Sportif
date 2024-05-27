using System;
using System.Collections.Generic;

namespace Forum_rufine_et_paul.Models;

public partial class Question
{
    public int QuestPk { get; set; }

    public int CatFk { get; set; }

    public string? UserFk { get; set; }

    public string QuestTitle { get; set; } = null!;

    public string QuestText { get; set; } = null!;

    public DateTime QuestDate { get; set; }

    public int? QuestViews { get; set; }

    public bool? QuestActif { get; set; }

    public virtual Category? CatFkNavigation { get; set; } 

    public virtual ICollection<Response>? Responses { get; set; } = new List<Response>();

    public virtual AspNetUser? UserFkNavigation { get; set; }
}
