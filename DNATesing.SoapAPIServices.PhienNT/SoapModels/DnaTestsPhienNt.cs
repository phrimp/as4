using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DNATesting.SoapAPIServices.PhienNT.SoapModels
{
    [DataContract]
    public partial class DnaTestsPhienNt
    {
        [DataMember]
        public int PhienNtid { get; set; }

        [DataMember]
        public string TestType { get; set; } = null!;

        [DataMember]
        public string? Conclusion { get; set; }

        [DataMember]
        public decimal? ProbabilityOfRelationship { get; set; }

        [DataMember]
        public decimal? RelationshipIndex { get; set; }

        [DataMember]
        public bool? IsCompleted { get; set; }

        [DataMember]
        public DateTime? CreatedAt { get; set; }
    }
}