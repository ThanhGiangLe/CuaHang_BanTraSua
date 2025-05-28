using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class TableMdl
    {
        [Key]
        public int TableId { get; set; } = default;
        public string TableName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
