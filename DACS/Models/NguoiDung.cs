using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    [Table("Nguoi_Dung")]
    public class NguoiDung
    {
        [Key] public int MaNguoiDung { get; set; }

        [Required, StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        public string MatKhau { get; set; } = string.Empty;

        public string HoTen { get; set; } = string.Empty;
        public string? Email { get; set; }
        public float DiemMucTieu { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;

        public virtual ICollection<KetQuaThi> KetQuaThis { get; set; } = new List<KetQuaThi>();
        public virtual ICollection<NhatKyHocTap> NhatKyHocTaps { get; set; } = new List<NhatKyHocTap>();
        public virtual ICollection<PhanTichAI> PhanTichAIs { get; set; } = new List<PhanTichAI>();
    }
}