using System;

namespace Pharmacy
{
    public enum DiscountCategory
    {
        None = 0,
        Pensioner = 1,
        Disabled = 2,
        LargeFamily = 3
    }

    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DiscountCategory DiscountCategory { get; set; } = DiscountCategory.None;
    }
}
bash