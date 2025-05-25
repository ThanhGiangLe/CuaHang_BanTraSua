using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models.ChatBot
{
    public class AImodel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatBotId { get; set; } = default;
        public DateTime? CreateDate { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
    }
}
