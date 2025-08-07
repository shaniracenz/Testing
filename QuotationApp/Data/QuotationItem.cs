namespace QuotationApp.Data
{
    public class QuotationItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int QuotationId { get; set; }
        public Quotation Quotation { get; set; }
    }
}
