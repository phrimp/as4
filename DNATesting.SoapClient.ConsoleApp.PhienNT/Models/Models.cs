using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace DNATesting.SoapClient.ConsoleApp.PhienNT.Models
{
    // Data Models
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DNATesting.SoapAPIServices.PhienNT.SoapModels")]
    public class DnaTestsPhienNt
    {
        [DataMember(Name = "PhienNtid")]
        public int PhienNtid { get; set; }

        [DataMember(Name = "TestType")]
        public string? TestType { get; set; }

        [DataMember(Name = "Conclusion")]
        public string? Conclusion { get; set; }

        [DataMember(Name = "ProbabilityOfRelationship")]
        public decimal? ProbabilityOfRelationship { get; set; }

        [DataMember(Name = "RelationshipIndex")]
        public decimal? RelationshipIndex { get; set; }

        [DataMember(Name = "IsCompleted")]
        public bool? IsCompleted { get; set; }

        [DataMember(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        public override string ToString()
        {
            var probability = ProbabilityOfRelationship?.ToString("F2") ?? "N/A";
            return $"ID: {PhienNtid}, Type: {TestType ?? "N/A"}, Probability: {probability}%, Completed: {IsCompleted?.ToString() ?? "N/A"}";
        }
    }

    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DNATesting.SoapAPIServices.PhienNT.SoapModels")]
    public class LociPhienNt
    {
        [DataMember(Name = "PhienNtid")]
        public int PhienNtid { get; set; }

        [DataMember(Name = "Name")]
        public string? Name { get; set; }

        [DataMember(Name = "IsCodis")]
        public bool? IsCodis { get; set; }

        [DataMember(Name = "Description")]
        public string? Description { get; set; }

        [DataMember(Name = "MutationRate")]
        public decimal? MutationRate { get; set; }

        [DataMember(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        public override string ToString()
        {
            var rate = MutationRate?.ToString("F4") ?? "N/A";
            return $"ID: {PhienNtid}, Name: {Name ?? "N/A"}, CODIS: {IsCodis?.ToString() ?? "N/A"}, Rate: {rate}";
        }
    }

    // Service Contracts - matching WSDL exactly
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IDnaTestsPhienNtSoapService
    {
        [OperationContract(Action = "http://tempuri.org/IDnaTestsPhienNtSoapService/GetDnaTests")]
        DnaTestsPhienNt[] GetDnaTests();

        [OperationContract(Action = "http://tempuri.org/IDnaTestsPhienNtSoapService/GetDnaTestById")]
        DnaTestsPhienNt GetDnaTestById(int id);

        [OperationContract(Action = "http://tempuri.org/IDnaTestsPhienNtSoapService/CreateDnaTest")]
        DnaTestsPhienNt CreateDnaTest(DnaTestsPhienNt dnaTest);

        [OperationContract(Action = "http://tempuri.org/IDnaTestsPhienNtSoapService/UpdateDnaTest")]
        DnaTestsPhienNt UpdateDnaTest(int id, DnaTestsPhienNt dnaTest);

        [OperationContract(Action = "http://tempuri.org/IDnaTestsPhienNtSoapService/DeleteDnaTest")]
        bool DeleteDnaTest(int id);

        [OperationContract(Action = "http://tempuri.org/IDnaTestsPhienNtSoapService/SearchDnaTests")]
        DnaTestsPhienNt[] SearchDnaTests(string testType, bool isCompleted);
    }

    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface ILociPhienNtSoapService
    {
        [OperationContract(Action = "http://tempuri.org/ILociPhienNtSoapService/GetLoci")]
        LociPhienNt[] GetLoci();

        [OperationContract(Action = "http://tempuri.org/ILociPhienNtSoapService/GetLocusById")]
        LociPhienNt GetLocusById(int id);

        [OperationContract(Action = "http://tempuri.org/ILociPhienNtSoapService/GetLocusByName")]
        LociPhienNt GetLocusByName(string name);

        [OperationContract(Action = "http://tempuri.org/ILociPhienNtSoapService/CreateLocus")]
        LociPhienNt CreateLocus(LociPhienNt locus);

        [OperationContract(Action = "http://tempuri.org/ILociPhienNtSoapService/UpdateLocus")]
        LociPhienNt UpdateLocus(int id, LociPhienNt locus);

        [OperationContract(Action = "http://tempuri.org/ILociPhienNtSoapService/DeleteLocus")]
        bool DeleteLocus(int id);

        [OperationContract(Action = "http://tempuri.org/ILociPhienNtSoapService/SearchLoci")]
        LociPhienNt[] SearchLoci(string name, bool isCodis);

        [OperationContract(Action = "http://tempuri.org/ILociPhienNtSoapService/GetCodisLoci")]
        LociPhienNt[] GetCodisLoci();
    }
}