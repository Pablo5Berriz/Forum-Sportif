using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Forum_rufine_et_paul.Models;

namespace Forum_rufine_et_paul.ViewModels
{
    public class custquest
    {
        [Key, Column(Order = 0)]

        public int QuestPk { get; set; }

        public int CatFk { get; set; }

        public string? UserFk { get; set; }
        public string QuestTitle { get; set; }
        public string? UserName { get; set; }
        public DateTime QuestDate { get; set; }
        public int? TotalReponses { get; set; }
        public int? QuestViews { get; set; }
        public DateTime DateDerniereReponse { get; set; }
        public string? DernierPosteur { get; set; }
        public List<Response> Responses { get; set; }
    }
}