using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DACS.Models;

namespace DACS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<CauHoi> CauHois { get; set; }
        public DbSet<DeThi> DeThis { get; set; }
        public DbSet<ChuDe> ChuDes { get; set; }
        public DbSet<KetQuaThi> KetQuaThis { get; set; }
        public DbSet<NhatKyHocTap> NhatKyHocTaps { get; set; }
        public DbSet<PhanTichAI> PhanTichAIs { get; set; }
        public DbSet<DeThiYeuThich> DeThiYeuThichs { get; set; }
        public DbSet<CauHoiSai> CauHoiSais { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<BaoCaoLoi> BaoCaoLois { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<KetQuaThi>(entity =>
            {
                entity.HasOne(k => k.User)
                    .WithMany()
                    .HasForeignKey(k => k.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<KetQuaThi>().ToTable("Ket_Qua_Thi");

            // Bookmark: mỗi user chỉ bookmark 1 đề 1 lần
            builder.Entity<DeThiYeuThich>()
                .HasIndex(d => new { d.UserId, d.MaDeThi })
                .IsUnique();

            // CauHoiSai: mỗi user - mỗi câu hỏi là 1 record
            builder.Entity<CauHoiSai>()
                .HasIndex(c => new { c.UserId, c.MaCauHoi })
                .IsUnique();
        }
    }

    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var context = service.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            if (context.ChuDes.Any()) return;
            await context.SaveChangesAsync();
        }
    }
}
