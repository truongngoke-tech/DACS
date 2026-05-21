namespace DACS.Models
{
    public class HomeViewModel
    {
        public int SoDeDaHoanThanh { get; set; }
        public double TiLeDungTrungBinh { get; set; }
        public int PhutHocTapTuanNay { get; set; }
        public int NgayLienTiep { get; set; }
        // Thêm danh sách đề thi vào đây
        public IEnumerable<DeThi> DanhSachDeThi { get; set; } = new List<DeThi>();
    }
}