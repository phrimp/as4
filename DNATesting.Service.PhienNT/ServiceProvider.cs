﻿using DNATesting.Services.PhienNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATesting.Service.PhienNT
{
    public interface IServiceProvider
    {
        SystemUserAccountService UserAccountService { get; }
        DnaTestsPhienNtService DnaTestsPhienNtService { get; }
    }

    public class ServiceProvider : IServiceProvider
    {
        SystemUserAccountService _userAccountService;
        DnaTestsPhienNtService _dnaTestsPhienNtService;

        public SystemUserAccountService UserAccountService
        {
            get { return _userAccountService ??= new SystemUserAccountService(); }
        }

        public DnaTestsPhienNtService DnaTestsPhienNtService
        {
            get { return _dnaTestsPhienNtService ??= new DnaTestsPhienNtService(); }
        }
    }
}
