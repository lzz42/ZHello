using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace MyAspWeb.Controllers
{
    public class CachingController : Controller
    {
        private IMemoryCache _cache;

        public CachingController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IActionResult Index()
        {
            string key = "Time";
            string result = "";
            if (!_cache.TryGetValue(key, out result))
            {
                result = $"Line Zero : {DateTime.Now}";
                _cache.Set(key, result);
            }
            ViewBag.Cache = result;
            return View();
        }

        public IActionResult End()
        {
            string key = "Time";
            string result = "";
            if (!_cache.TryGetValue(key, out result))
            {
                result = $"Line Zero : {DateTime.Now}";
                _cache.Set(key, result);
                //设置2分钟的相对过期时间
                _cache.Set(key, result, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2)));
                //设置2分钟的绝对过期时间
                _cache.Set(key, result, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                //移除缓存
                _cache.Remove(key);
                //设置缓存优先级
                _cache.Set(key, result, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.High));
                //缓存回调 过期回调
                _cache.Set(key, result, new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(20))
                    .RegisterPostEvictionCallback((k, v, reason, substate) =>
                {
                    Console.WriteLine($"Key:{k};Value:{v};Reason:{reason}");
                }));
                //缓存回调 Token过期回调
                var cts = new CancellationTokenSource();
                _cache.Set(key, result, new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(20))
                    .AddExpirationToken(new CancellationChangeToken(cts.Token))
                    .RegisterPostEvictionCallback((k, v, reason, substate) =>
                {
                    Console.WriteLine($"Cancel Key:{k};Value:{v};Reason:{reason}");
                }));

            }
            ViewBag.Cache = result;
            return View();
        }
    }
}
