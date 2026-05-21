using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DACS.Models;

[Table("Thong_Bao")]
public class ThongBao
{
    [Key] public int Id { get; set; }
    public string? UserId { get; set; }  // null = gửi tất cả
    [ForeignKey("UserId")] public virtual ApplicationUser? User { get; set; }
    public string NoiDung { get; set; } = "";
    public string? DuongDan { get; set; }  // link khi click
    public string LoaiIcon { get; set; } = "info";  // info, success, warning
    public bool DaDoc { get; set; } = false;
    public DateTime ThoiGian { get; set; } = DateTime.Now;
}
