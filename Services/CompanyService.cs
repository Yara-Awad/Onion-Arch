﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

//using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Models;
using Service.Contract;
using Shared.DataTransferObjects;

namespace Services
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }
        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

        }
        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }
        public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(id, trackChanges);

            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }
        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;
        }
        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities =await _repository.Company.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;
        }
        public async Task<(IEnumerable<CompanyDto> companies, string ids)>CreateCompanyCollectionAsync (IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }

            await _repository.SaveAsync();

            var companyCollectionToReturn =
           _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

            return (companies: companyCollectionToReturn, ids: ids);
        }

        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            _repository.Company.DeleteCompany(company);
           await _repository.SaveAsync();
        }
        public async Task UpdateCompanyAsync(Guid companyId,CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var companyEntity = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _mapper.Map(companyForUpdate, companyEntity);
          await  _repository.SaveAsync();
        }
        //public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        //{
        //    try
        //    {
        //        var companies =
        //     _repository.Company.GetAllCompanies(trackChanges);

        //        return companies;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Something went wrong in the { nameof(GetAllCompanies)}service method { ex}"); 
        //     throw;
        //    }
        //}


    }
}
