using DNATesting.Service.PhienNT;
using DNATesting.SoapAPIServices.PhienNT.SoapModels;
using System.ServiceModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DNATesting.SoapAPIServices.PhienNT.SoapServices
{
    [ServiceContract]
    public interface ILociPhienNtSoapService
    {
        [OperationContract]
        Task<List<LociPhienNt>> GetLociAsync();

        [OperationContract]
        Task<LociPhienNt?> GetLocusByIdAsync(int id);

        [OperationContract]
        Task<LociPhienNt?> GetLocusByNameAsync(string name);

        [OperationContract]
        Task<LociPhienNt> CreateLocusAsync(LociPhienNt locus);

        [OperationContract]
        Task<LociPhienNt?> UpdateLocusAsync(int id, LociPhienNt locus);

        [OperationContract]
        Task<bool> DeleteLocusAsync(int id);

        [OperationContract]
        Task<List<LociPhienNt>> SearchLociAsync(string name, bool? isCodis);

        [OperationContract]
        Task<List<LociPhienNt>> GetCodisLociAsync();
    }

    public class LociPhienNtSoapService : ILociPhienNtSoapService
    {
        private readonly IServiceProviders _serviceProviders;

        public LociPhienNtSoapService(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public async Task<List<LociPhienNt>> GetLociAsync()
        {
            try
            {
                var loci = await _serviceProviders.LociPhienNtService.GetAllAsync();

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var lociJsonString = JsonSerializer.Serialize(loci, opt);
                var result = JsonSerializer.Deserialize<List<LociPhienNt>>(lociJsonString, opt);

                return result ?? new List<LociPhienNt>();
            }
            catch (Exception)
            {
                return new List<LociPhienNt>();
            }
        }

        public async Task<LociPhienNt?> GetLocusByIdAsync(int id)
        {
            try
            {
                var locus = await _serviceProviders.LociPhienNtService.GetByIdAsync(id);

                if (locus == null) return null;

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var locusJsonString = JsonSerializer.Serialize(locus, opt);
                return JsonSerializer.Deserialize<LociPhienNt>(locusJsonString, opt);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<LociPhienNt?> GetLocusByNameAsync(string name)
        {
            try
            {
                var locus = await _serviceProviders.LociPhienNtService.GetByNameAsync(name);

                if (locus == null) return null;

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var locusJsonString = JsonSerializer.Serialize(locus, opt);
                return JsonSerializer.Deserialize<LociPhienNt>(locusJsonString, opt);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<LociPhienNt> CreateLocusAsync(LociPhienNt locus)
        {
            try
            {
                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var locusJsonString = JsonSerializer.Serialize(locus, opt);
                var locusEntity = JsonSerializer.Deserialize<DNATesting.Repository.PhienNT.LociPhienNt>(locusJsonString, opt);

                if (locusEntity != null)
                {
                    var result = await _serviceProviders.LociPhienNtService.CreateAsync(locusEntity);
                    return locus;
                }

                return new LociPhienNt();
            }
            catch (Exception)
            {
                return new LociPhienNt();
            }
        }

        public async Task<LociPhienNt?> UpdateLocusAsync(int id, LociPhienNt locus)
        {
            try
            {
                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var locusJsonString = JsonSerializer.Serialize(locus, opt);
                var locusEntity = JsonSerializer.Deserialize<DNATesting.Repository.PhienNT.LociPhienNt>(locusJsonString, opt);

                if (locusEntity != null)
                {
                    locusEntity.PhienNtid = id;
                    var result = await _serviceProviders.LociPhienNtService.UpdateAsync(locusEntity);
                    if (result > 0)
                    {
                        return locus;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteLocusAsync(int id)
        {
            try
            {
                return await _serviceProviders.LociPhienNtService.DeleteAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<LociPhienNt>> SearchLociAsync(string name, bool? isCodis)
        {
            try
            {
                var loci = await _serviceProviders.LociPhienNtService.SearchAsync(name, isCodis);

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var lociJsonString = JsonSerializer.Serialize(loci, opt);
                var result = JsonSerializer.Deserialize<List<LociPhienNt>>(lociJsonString, opt);

                return result ?? new List<LociPhienNt>();
            }
            catch (Exception)
            {
                return new List<LociPhienNt>();
            }
        }

        public async Task<List<LociPhienNt>> GetCodisLociAsync()
        {
            try
            {
                var loci = await _serviceProviders.LociPhienNtService.GetCodisLociAsync();

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var lociJsonString = JsonSerializer.Serialize(loci, opt);
                var result = JsonSerializer.Deserialize<List<LociPhienNt>>(lociJsonString, opt);

                return result ?? new List<LociPhienNt>();
            }
            catch (Exception)
            {
                return new List<LociPhienNt>();
            }
        }
    }
}