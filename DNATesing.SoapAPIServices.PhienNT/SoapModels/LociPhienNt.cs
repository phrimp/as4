using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DNATesting.SoapAPIServices.PhienNT.SoapModels
{
    [DataContract]
    public partial class LociPhienNt
    {
        [DataMember]
        public int PhienNtid { get; set; }

        [DataMember]
        public string Name { get; set; } = null!;

        [DataMember]
        public bool? IsCodis { get; set; }

        [DataMember]
        public string? Description { get; set; }

        [DataMember]
        public decimal? MutationRate { get; set; }

        [DataMember]
        public DateTime? CreatedAt { get; set; }
    }
}