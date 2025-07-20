using DNATesting.Service.PhienNT;
using DNATesting.SoapAPIServices.PhienNT.SoapModels;
using System.ServiceModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DNATesting.SoapAPIServices.PhienNT.SoapServices
{
    [ServiceContract]
    public interface IDnaTestsPhienNtSoapService
    {
        [OperationContract]
        Task<List<DnaTestsPhienNt>> GetDnaTestsAsync();

        [OperationContract]
        Task<DnaTestsPhienNt?> GetDnaTestByIdAsync(int id);

        [OperationContract]
        Task<DnaTestsPhienNt> CreateDnaTestAsync(DnaTestsPhienNt dnaTest);

        [OperationContract]
        Task<DnaTestsPhienNt?> UpdateDnaTestAsync(int id, DnaTestsPhienNt dnaTest);

        [OperationContract]
        Task<bool> DeleteDnaTestAsync(int id);

        [OperationContract]
        Task<List<DnaTestsPhienNt>> SearchDnaTestsAsync(string testType, bool? isCompleted);
    }

    public class DnaTestsPhienNtSoapService : IDnaTestsPhienNtSoapService
    {
        private readonly IServiceProviders _serviceProviders;

        public DnaTestsPhienNtSoapService(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public async Task<List<DnaTestsPhienNt>> GetDnaTestsAsync()
        {
            try
            {
                var tests = await _serviceProviders.DnaTestsPhienNtService.GetAllAsync();

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var testsJsonString = JsonSerializer.Serialize(tests, opt);
                var result = JsonSerializer.Deserialize<List<DnaTestsPhienNt>>(testsJsonString, opt);

                return result ?? new List<DnaTestsPhienNt>();
            }
            catch (Exception)
            {
                return new List<DnaTestsPhienNt>();
            }
        }

        public async Task<DnaTestsPhienNt?> GetDnaTestByIdAsync(int id)
        {
            try
            {
                var test = await _serviceProviders.DnaTestsPhienNtService.GetByIdAsync(id);

                if (test == null) return null;

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var testJsonString = JsonSerializer.Serialize(test, opt);
                return JsonSerializer.Deserialize<DnaTestsPhienNt>(testJsonString, opt);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<DnaTestsPhienNt> CreateDnaTestAsync(DnaTestsPhienNt dnaTest)
        {
            try
            {
                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var testJsonString = JsonSerializer.Serialize(dnaTest, opt);
                var testEntity = JsonSerializer.Deserialize<DNATesting.Repository.PhienNT.DnaTestsPhienNt>(testJsonString, opt);

                if (testEntity != null)
                {
                    var result = await _serviceProviders.DnaTestsPhienNtService.CreateAsync(testEntity);
                    return dnaTest;
                }

                return new DnaTestsPhienNt();
            }
            catch (Exception)
            {
                return new DnaTestsPhienNt();
            }
        }

        public async Task<DnaTestsPhienNt?> UpdateDnaTestAsync(int id, DnaTestsPhienNt dnaTest)
        {
            try
            {
                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var testJsonString = JsonSerializer.Serialize(dnaTest, opt);
                var testEntity = JsonSerializer.Deserialize<DNATesting.Repository.PhienNT.DnaTestsPhienNt>(testJsonString, opt);

                if (testEntity != null)
                {
                    testEntity.PhienNtid = id;
                    var result = await _serviceProviders.DnaTestsPhienNtService.UpdateAsync(testEntity);
                    if (result > 0)
                    {
                        return dnaTest;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteDnaTestAsync(int id)
        {
            try
            {
                return await _serviceProviders.DnaTestsPhienNtService.DeleteAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<DnaTestsPhienNt>> SearchDnaTestsAsync(string testType, bool? isCompleted)
        {
            try
            {
                var tests = await _serviceProviders.DnaTestsPhienNtService.SearchAsync(testType, isCompleted);

                var opt = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                var testsJsonString = JsonSerializer.Serialize(tests, opt);
                var result = JsonSerializer.Deserialize<List<DnaTestsPhienNt>>(testsJsonString, opt);

                return result ?? new List<DnaTestsPhienNt>();
            }
            catch (Exception)
            {
                return new List<DnaTestsPhienNt>();
            }
        }
    }
}