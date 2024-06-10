namespace Web_Api_sam.Models.DTO
{
    public class couponDTO
    {
        public int id { get; set; }
        
        public string name { get; set; }
        public int percent { get; set; }
        public bool IaActive { get; set; }
        public DateTime? create { get; set; }
    }
}
