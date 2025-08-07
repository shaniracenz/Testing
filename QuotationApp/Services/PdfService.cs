using QuotationApp.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuotationApp.Services
{
    public class PdfService
    {
        public byte[] GenerateQuotationPdf(Quotation quotation)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Quotation #{quotation.QuotationNumber}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(20);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text("Customer Information").SemiBold();
                                    col.Item().Text(quotation.Customer.Name);
                                    col.Item().Text(quotation.Customer.Email);
                                    col.Item().Text(quotation.Customer.Address);
                                });

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text("Date").SemiBold();
                                    col.Item().Text(quotation.Date.ToShortDateString());
                                });
                            });

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(100);
                                    columns.ConstantColumn(100);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Description").Bold();
                                    header.Cell().Text("Qty").Bold();
                                    header.Cell().Text("Price").Bold();
                                    header.Cell().Text("Total").Bold();
                                });

                                foreach (var item in quotation.Items)
                                {
                                    table.Cell().Text(item.Description);
                                    table.Cell().Text(item.Quantity.ToString());
                                    table.Cell().Text(item.Price.ToString("C"));
                                    table.Cell().Text((item.Quantity * item.Price).ToString("C"));
                                }
                            });

                            column.Item().AlignRight().Text($"Total: {quotation.Items.Sum(i => i.Quantity * i.Price):C}").Bold();
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
