using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class VerificationCodeManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<VerificationCodeManager> _logger;

        public VerificationCodeManager(IMemoryCache memoryCache, ILogger<VerificationCodeManager> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task SetVerificationCodeAsync(string email, string code, TimeSpan expiration)
        {
            _memoryCache.Set(email, code, expiration);
            //_logger.LogInformation($"Memory CACHE : {code}");
            await Task.CompletedTask;
        }

        public async Task<bool> ValidateVerificationCodeAsync(string email, string code)
        {
            if (_memoryCache.TryGetValue(email, out string storedCode))
            {
                //_logger.LogInformation($"Stored code : {storedCode}");
                return await Task.FromResult(storedCode == code);
            }
            return await Task.FromResult(false);
        }

        public async Task RemoveVerificationCodeAsync(string email)
        {
            _memoryCache.Remove(email);
            await Task.CompletedTask;
        }
    }
}
