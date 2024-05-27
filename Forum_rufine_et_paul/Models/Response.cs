using System;
using System.Collections.Generic;

namespace Forum_rufine_et_paul.Models;

public partial class Response
{
    public int RespPk { get; set; }

    public int QuestFk { get; set; }

    public string? UserFk { get; set; }

    public string RespText { get; set; } = null!;

    public DateTime? RespDate { get; set; }

    public bool? RespActif { get; set; }

    public virtual Question? QuestFkNavigation { get; set; }

    public virtual AspNetUser? UserFkNavigation { get; set; }
}
