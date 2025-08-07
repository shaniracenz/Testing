using System.Collections.Generic;

namespace QuotationApp.Data
{
    public class Quotation
    {
        public int Id { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<QuotationItem> Items { get; set; } = new List<QuotationItem>();
    }
}
