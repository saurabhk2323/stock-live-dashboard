using StockLiveDashboard.Contracts.Entities;

namespace StockLiveDashboard.WorkerService.DTOs
{
    public class StockSnapshot
    {
        public string StockID { get; set; }
        public string Sector { get; set; }
        public decimal Price { get; set; }
        public int TotalShares { get; set; }
        public List<InvestorKind> Investors { get; set; }
        public bool IsUpdated { get; set; }
    }
}
