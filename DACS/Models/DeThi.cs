using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    [Table("De_Thi")]
    public class DeThi
    {
        [Key]
        public int MaDeThi { get; set; }

        [Required]
        public string TenDeThi { get; set; } = string.Empty;

        public int? SoCauHoi { get; set; }

        public int? ThoiGian { get; set; }

        public int MaChuDe { get; set; }

        public string? MucDoKho { get; set; }

        [ForeignKey("MaChuDe")]
        public virtual ChuDe? ChuDe { get; set; }
    }
}
