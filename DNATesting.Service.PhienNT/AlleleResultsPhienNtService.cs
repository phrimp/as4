using DNATesting.Repository.PhienNT;
using DNATesting.Repository.PhienNT.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATesting.Services.PhienNT
{
    public class AlleleResultsPhienNtService : IAlleleResultsPhienNtService
    {
        private readonly AlleleResultsPhienNtRepository _repository;

        public AlleleResultsPhienNtService() => _repository = new AlleleResultsPhienNtRepository();

        public async Task<List<AlleleResultsPhienNt>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<AlleleResultsPhienNt> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<AlleleResultsPhienNt>> GetByTestIdAsync(int testId)
        {
            return await _repository.GetByTestIdAsync(testId);
        }

        public async Task<List<AlleleResultsPhienNt>> GetByProfileIdAsync(int profileId)
        {
            return await _repository.GetByProfileIdAsync(profileId);
        }

        public async Task<List<AlleleResultsPhienNt>> SearchAsync(int testId, int profileId, string role)
        {
            return await _repository.SearchAsync(testId, profileId, role ?? "");
        }

        public async Task<List<AlleleResultsPhienNt>> GetOutliersAsync()
        {
            return await _repository.GetOutliersAsync();
        }

        public async Task<int> CreateAsync(AlleleResultsPhienNt alleleResult)
        {
            return await _repository.CreateAsync(alleleResult);
        }

        public async Task<int> UpdateAsync(AlleleResultsPhienNt alleleResult)
        {
            return await _repository.UpdateAsync(alleleResult);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = _repository.GetById(id);
            return await _repository.RemoveAsync(entity);
        }

        public async Task<PaginationResult<List<AlleleResultsPhienNt>>> SearchWithPagingAsync(int testId, int profileId, string role, int page, int pageSize)
        {
            return await _repository.SearchWithPagingAsync(testId, profileId, role, page, pageSize);
        }

        public async Task<PaginationResult<List<AlleleResultsPhienNt>>> GetAllWithPagingAsync(int page, int pageSize)
        {
            return await _repository.GetAllWithPagingAsync(page, pageSize);
        }
    }
}