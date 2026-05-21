using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    [Table("Cau_Hoi")]
    public class CauHoi
    {
        [Key] public int MaCauHoi { get; set; }

        [Required] public string NoiDung { get; set; } = string.Empty;

        public string? PhuongAnA { get; set; }
        public string? PhuongAnB { get; set; }
        public string? PhuongAnC { get; set; }
        public string? PhuongAnD { get; set; }

        [MaxLength(1)] public string? DapAnDung { get; set; }
        public string? MucDoKho { get; set; }

        public int MaChuDe { get; set; }
        [ForeignKey("MaChuDe")]
        public virtual ChuDe? ChuDe { get; set; }

        public int? MaDeThi { get; set; }
        [ForeignKey("MaDeThi")]
        public virtual DeThi? DeThi { get; set; }
        public string? AudioPath { get; set; } // Lưu đường dẫn file audio
        public bool LaCauHoiNghe { get; set; } // Đánh dấu đây là câu hỏi nghe
    }
}