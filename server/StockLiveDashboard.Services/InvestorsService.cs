using StockLiveDashboard.Contracts;
using StockLiveDashboard.Contracts.DTOs;
using StockLiveDashboard.Contracts.Entities;
using StockLiveDashboard.Services.DatabaseServices;
using StockLiveDashboard.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StockLiveDashboard.Services
{
    public class InvestorsService : IInvestorsService
    {
        private readonly CosmosDBService _db;

        public InvestorsService(CosmosDBService db)
        {
            _db = db;
        }

        public async Task<InvestorResponse> AddInvestor(InvestorAddRequest investorAddRequest)
        {
            Investor investor = investorAddRequest.ToInvestor();
            investor.ID = Guid.NewGuid();
            var response = await _db.CreateItem(
                    _db.GetContainer(ContainerTypes.investors),
                    investor,
                    investor.CountryCode
                );
            return response.ToInvestorResponse();
        }

        public async Task<bool> DeleteInvestor(Guid id, string countryCode)
        {
            return await _db.DeleteItem<Stock>(
                _db.GetContainer(ContainerTypes.investors),
                id.ToString(),
                countryCode);
        }

        public async Task<List<InvestorResponse>> GetAllInvestors()
        {
            Expression<Func<Investor, bool>> predicate = p => true;
            var response = await _db.GetItems<Investor>(
                _db.GetContainer(ContainerTypes.investors),
                predicate
                );
            List<InvestorResponse> investorResponses = new();
            response.ForEach(i => investorResponses.Add(i.ToInvestorResponse()));
            return investorResponses;
        }

        public async Task<InvestorResponse?> GetInvestorByID(Guid id, string countryCode)
        {
            Investor? investor = await _db.GetItem<Investor>(
                _db.GetContainer(ContainerTypes.investors),
                id.ToString(),
                countryCode
                );
            if (investor == null)
                return null;
            return investor.ToInvestorResponse();
        }

        public async Task<InvestorResponse> GetInvestorByUsername(string username, string countryCode)
        {
            Expression<Func<Investor, bool>> predicate = p => p.InvestorUniqueName.ToLower() == username.ToLower() && p.CountryCode == countryCode;
            List<Investor> investorList = await _db.GetItems<Investor>(
                _db.GetContainer(ContainerTypes.investors),
                predicate,
                countryCode
                );
            return investorList.FirstOrDefault().ToInvestorResponse();
        }

        public async Task<InvestorResponse> UpdateInvestor(InvestorUpdateRequest investorUpdateRequest)
        {
            Investor investor = investorUpdateRequest.ToInvestor();
            var response = await _db.UpdateItem(
                    _db.GetContainer(ContainerTypes.investors),
                    investor,
                    investor.CountryCode
                    );
            InvestorResponse investorResponse = response.ToInvestorResponse();
            return investorResponse;
        }
    }
}
