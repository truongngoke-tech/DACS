using DACS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Phan_Tich_AI")]
public class PhanTichAI
{
    [Key] public int MaPhanTich { get; set; }

    public string? UserId { get; set; }

    public int MaChuDeYeu { get; set; }
    [ForeignKey("MaChuDeYeu")]
    public virtual ChuDe? ChuDe { get; set; }

    public string LoiKhuyenAI { get; set; } = string.Empty;
    public DateTime CapNhatLanCuoi { get; set; } = DateTime.Now;
}