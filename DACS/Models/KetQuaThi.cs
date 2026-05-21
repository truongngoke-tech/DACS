using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    [Table("Ket_Qua_Thi")]
    public class KetQuaThi
    {
        [Key]
        public int MaKetQua { get; set; }
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; } // Đã có liên kết User

        public int MaDeThi { get; set; }
        [ForeignKey("MaDeThi")]
        public virtual DeThi? DeThi { get; set; }

        public float TongDiem { get; set; }
        public int SoCauDung { get; set; }
        public DateTime NgayThi { get; set; } = DateTime.Now;
        public string? ChiTietJson { get; set; }
    }
}