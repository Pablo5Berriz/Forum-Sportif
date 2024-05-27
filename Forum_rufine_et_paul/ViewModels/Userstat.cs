using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_rufine_et_paul.ViewModels
{
    public class Userstat
    {
        [Key,Column(Order = 0)]
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string? Email { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalReponses { get; set; }
    }
}