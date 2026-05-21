using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    [Table("Bao_Cao_Loi")]
    public class BaoCaoLoi
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        public int MaDeThi { get; set; }
        [ForeignKey("MaDeThi")]
        public virtual DeThi? DeThi { get; set; }

        public int? MaCauHoi { get; set; }
        [ForeignKey("MaCauHoi")]
        public virtual CauHoi? CauHoi { get; set; }

        [Required]
        public string LoaiBaoCao { get; set; } = string.Empty;

        public string MoTa { get; set; } = string.Empty;

        public string TrangThai { get; set; } = "Cho xu ly";

        public string? GhiChuAdmin { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public DateTime? NgayXuLy { get; set; }
    }
}
