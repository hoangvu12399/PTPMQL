namespace DemoMVC.Models.Entities
{
    public class Daily : HeThongPhanPhoi
    {
        public required string MaDaily { get; set; }
        public required string TenDaily { get; set; }
        public required string DiaChi { get; set; }
        public required string NguoiDaiDien { get; set; }
        public required string DienThoai { get; set; }
        public required HeThongPhanPhoi HeThongPhanPhoi { get; set; }
    }
}
