using Web_Api_sam.Models;

namespace Web_Api_sam.Data
{
    public static class CouponStore
    {
        public static List<coupon> couponList = new List<coupon> {
            new coupon{id=1,name="10ff",percent=10,IaActive=true},
            new coupon{id=2,name="20gh",percent=20,IaActive=false}
        };
    }
}
