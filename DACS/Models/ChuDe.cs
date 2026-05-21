using DACS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Chu_De")]
public class ChuDe
{
    [Key] public int MaChuDe { get; set; }

    [Required] public string TenChuDe { get; set; } = string.Empty;

    public int? MaChuDeCha { get; set; }

    [ForeignKey("MaChuDeCha")]
    public virtual ChuDe? ChuDeCha { get; set; }

    public virtual ICollection<CauHoi> CauHois { get; set; } = new List<CauHoi>();
}