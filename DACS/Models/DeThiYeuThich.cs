using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DACS.Models;

[Table("DeThi_YeuThich")]
public class DeThiYeuThich
{
    [Key] public int Id { get; set; }
    public string UserId { get; set; } = "";
    [ForeignKey("UserId")] public virtual ApplicationUser? User { get; set; }
    public int MaDeThi { get; set; }
    [ForeignKey("MaDeThi")] public virtual DeThi? DeThi { get; set; }
    public DateTime NgayLuu { get; set; } = DateTime.Now;
}
