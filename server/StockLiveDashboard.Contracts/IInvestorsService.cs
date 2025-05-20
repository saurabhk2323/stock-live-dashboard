using System;
using System.Collections.Generic;
using StockLiveDashboard.Contracts.DTOs;

namespace StockLiveDashboard.Contracts
{
    public interface IInvestorsService
    {
        Task<InvestorResponse> AddInvestor(InvestorAddRequest investorAddRequest);

        Task<bool> DeleteInvestor(Guid id, string countryCode);

        Task<InvestorResponse?> GetInvestorByID(Guid id, string countryCode);

        Task<InvestorResponse> GetInvestorByUsername(string username, string countryCode);

        Task<List<InvestorResponse>> GetAllInvestors();

        Task<InvestorResponse> UpdateInvestor(InvestorUpdateRequest investorUpdateRequest);
    }
}
