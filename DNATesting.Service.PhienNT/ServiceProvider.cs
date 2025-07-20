using DNATesting.Services.PhienNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATesting.Service.PhienNT
{
    public interface IServiceProviders
    {
        SystemUserAccountService UserAccountService { get; }
        DnaTestsPhienNtService DnaTestsPhienNtService { get; }
        LociPhienNtService LociPhienNtService { get; }
        AlleleResultsPhienNtService AlleleResultsPhienNtService { get; }
    }

    public class ServiceProviders : IServiceProviders
    {
        private SystemUserAccountService _userAccountService;
        private DnaTestsPhienNtService _dnaTestsPhienNtService;
        private LociPhienNtService _lociPhienNtService;
        private AlleleResultsPhienNtService _alleleResultsPhienNtService;

        public ServiceProviders() { }

        public SystemUserAccountService UserAccountService
        {
            get { return _userAccountService ??= new SystemUserAccountService(); }
        }

        public DnaTestsPhienNtService DnaTestsPhienNtService
        {
            get { return _dnaTestsPhienNtService ??= new DnaTestsPhienNtService(); }
        }

        public LociPhienNtService LociPhienNtService
        {
            get { return _lociPhienNtService ??= new LociPhienNtService(); }
        }

        public AlleleResultsPhienNtService AlleleResultsPhienNtService
        {
            get { return _alleleResultsPhienNtService ??= new AlleleResultsPhienNtService(); }
        }
    }
}