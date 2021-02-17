using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class DeletionTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public DeletionTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DeletionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
        {

        }
    }

   
}
