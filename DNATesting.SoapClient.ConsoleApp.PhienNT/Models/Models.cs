using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DNATesting.SoapClient.ConsoleApp.PhienNT.Models
{
    // Data Models
    [DataContract]
    public class DnaTestsPhienNt
    {
        [DataMember] public int PhienNtid { get; set; }
        [DataMember] public string TestType { get; set; } = "";
        [DataMember] public string? Conclusion { get; set; }
        [DataMember] public decimal? ProbabilityOfRelationship { get; set; }
        [DataMember] public decimal? RelationshipIndex { get; set; }
        [DataMember] public bool? IsCompleted { get; set; }
        [DataMember] public DateTime? CreatedAt { get; set; }

        public override string ToString()
        {
            return $"ID: {PhienNtid}, Type: {TestType}, Probability: {ProbabilityOfRelationship:F2}%, Completed: {IsCompleted}";
        }
    }

    [DataContract]
    public class LociPhienNt
    {
        [DataMember] public int PhienNtid { get; set; }
        [DataMember] public string Name { get; set; } = "";
        [DataMember] public bool? IsCodis { get; set; }
        [DataMember] public string? Description { get; set; }
        [DataMember] public decimal? MutationRate { get; set; }
        [DataMember] public DateTime? CreatedAt { get; set; }

        public override string ToString()
        {
            return $"ID: {PhienNtid}, Name: {Name}, CODIS: {IsCodis}, Rate: {MutationRate:F4}";
        }
    }

    // Service Contracts
    [ServiceContract]
    public interface IDnaTestsService
    {
        [OperationContract] Task<List<DnaTestsPhienNt>> GetDnaTests();
        [OperationContract] Task<DnaTestsPhienNt?> GetDnaTestById(int id);
        [OperationContract] Task<DnaTestsPhienNt> CreateDnaTest(DnaTestsPhienNt test);
        [OperationContract] Task<bool> DeleteDnaTest(int id);
    }

    [ServiceContract]
    public interface ILociService
    {
        [OperationContract] Task<List<LociPhienNt>> GetLoci();
        [OperationContract] Task<LociPhienNt?> GetLocusById(int id);
        [OperationContract] Task<LociPhienNt> CreateLocus(LociPhienNt locus);
        [OperationContract] Task<bool> DeleteLocus(int id);
    }

}
