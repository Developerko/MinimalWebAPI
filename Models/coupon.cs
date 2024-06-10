namespace Web_Api_sam.Models
{
    public class coupon
    {
        public int id { get; set; }
        public string name { get; set; }
        public int percent { get; set; }
        public bool IaActive { get; set; }
        public DateTime? create { get; set; }
        public DateTime? Lastupdate { get; set; }
    }
}
