using SosyalYardim.Models;
using SosyalYardim.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SosyalYardim.Services;

public class PdfExportService : IPdfExportService
{
    private readonly AppDbContext _context;

    public PdfExportService(AppDbContext context)
    {
        _context = context;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> GenerateImpactReportAsync(string userId, DateTime startDate, DateTime endDate)
    {
        var user = await _context.Users.FindAsync(userId);
        var donations = await _context.Donations
            .Where(d => d.UserId == userId && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
            .ToListAsync();

        var campaigns = await _context.Campaigns
            .Where(c => donations.Select(d => d.CampaignId).Contains(c.Id))
            .ToListAsync();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header().Element(ComposeHeader);

                page.Content().Column(column =>
                {
                    column.Spacing(15);

                    // Kullanıcı Bilgisi
                    column.Item().Text($"Kullanıcı: {user?.FullName ?? user?.UserName ?? "Bilinmeyen"}")
                        .FontSize(14).Bold();

                    column.Item().Text($"Rapor Dönemi: {startDate:dd MMMM yyyy} - {endDate:dd MMMM yyyy}")
                        .FontSize(12);

                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    // Özet Bilgiler
                    column.Item().Text("KATKIYA GENEL BAKIŞ").FontSize(14).Bold();
                    
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Toplam Katkı Sayısı:").FontSize(11);
                            col.Item().Text(donations.Count.ToString()).FontSize(18).Bold().FontColor(Colors.Green.Darken2);
                        });
                        
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Toplam Tutar:").FontSize(11);
                            col.Item().Text($"{donations.Sum(d => d.Amount):C}").FontSize(18).Bold().FontColor(Colors.Green.Darken2);
                        });
                        
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Kazanılan Puan:").FontSize(11);
                            col.Item().Text(donations.Sum(d => d.PointsEarned).ToString()).FontSize(18).Bold().FontColor(Colors.Green.Darken2);
                        });
                    });

                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    // Desteklenen Alanlar
                    column.Item().Text("DESTEKLENEN ALANLAR").FontSize(14).Bold();
                    
                    var categoryGroups = campaigns
                        .GroupBy(c => c.CategoryDisplayName)
                        .Select(g => new { Category = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .ToList();

                    foreach (var group in categoryGroups)
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"• {group.Category}");
                            row.ConstantItem(50).Text($"{group.Count} kampanya").AlignRight();
                        });
                    }

                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    // Katkı Detayları
                    if (donations.Any())
                    {
                        column.Item().Text("KATKIYA DETAYLARI").FontSize(14).Bold();
                        
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Kampanya").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tutar").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Puan").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tarih").Bold();
                            });

                            foreach (var donation in donations.OrderByDescending(d => d.CreatedAt))
                            {
                                var campaign = campaigns.FirstOrDefault(c => c.Id == donation.CampaignId);
                                
                                table.Cell().Padding(5).Text(campaign?.Title ?? "Bilinmeyen");
                                table.Cell().Padding(5).Text($"{donation.Amount:C}");
                                table.Cell().Padding(5).Text(donation.PointsEarned.ToString());
                                table.Cell().Padding(5).Text(donation.CreatedAt.ToString("dd/MM/yyyy"));
                            }
                        });
                    }
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Sayfa ");
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                    text.Span(" • İyilik Puan Platformu");
                });
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Text("İYİLİK PUAN").FontSize(24).Bold().FontColor(Colors.Green.Darken2);
            column.Item().AlignCenter().Text("Etki Raporu").FontSize(16).FontColor(Colors.Grey.Darken1);
            column.Item().PaddingTop(10).LineHorizontal(2).LineColor(Colors.Green.Darken2);
        });
    }
}
