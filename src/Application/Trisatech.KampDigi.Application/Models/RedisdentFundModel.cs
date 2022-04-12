namespace Trisatech.KampDigi.Application.Models;
public class ResidentFundModel {
    public int Year { get; set; }
    public decimal BeginingBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal EndingBalance { get; set; }
    public decimal HoldBalance { get; set; }
    public DateTime? LastUpdate { get; set; }
    public string UpdatedBy { get; set; } = null!;
}
