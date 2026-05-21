using DACS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Nhat_Ky_Hoc_Tap")]
public class NhatKyHocTap
{
    [Key] public int MaNhatKy { get; set; }

    public string? UserId { get; set; }   // thay MaNguoiDung int → UserId string

    public int MaCauHoi { get; set; }
    [ForeignKey("MaCauHoi")]
    public virtual CauHoi? CauHoi { get; set; }

    public string DapAnDaChon { get; set; } = string.Empty;
    public bool LaDung { get; set; }
    public int ThoiGianLamGiay { get; set; }
    public DateTime NgayThucHien { get; set; } = DateTime.Now;
}