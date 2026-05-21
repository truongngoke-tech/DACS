using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DACS.Models;

[Table("CauHoi_Sai")]
public class CauHoiSai
{
    [Key] public int Id { get; set; }
    public string UserId { get; set; } = "";
    [ForeignKey("UserId")] public virtual ApplicationUser? User { get; set; }
    public int MaCauHoi { get; set; }
    [ForeignKey("MaCauHoi")] public virtual CauHoi? CauHoi { get; set; }
    public string? DapAnDaChon { get; set; }
    public int LanSai { get; set; } = 1;
    public DateTime LanCuoiSai { get; set; } = DateTime.Now;
}
