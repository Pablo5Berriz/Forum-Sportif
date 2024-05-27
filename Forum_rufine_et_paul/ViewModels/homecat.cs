using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Forum_rufine_et_paul.Models;
using Forum_rufine_et_paul.ViewModels;

namespace Forum_rufine_et_paul.ViewModels
{
    public class homecat
    {
        [Key, Column(Order = 0)]
        public int CatPk { get; set; }
        public string CatName { get; set; }
        public string CatDesc { get; set; }
        public string CatImg { get; set; }
        public bool? CatActif { get; set; }
        public List<Question> Questions { get; set; }
    }
}