using iTunesManager.Models;
using System;
using System.Runtime.Caching;

namespace iTunesManager.Controllers
{
    public class CacheController
    {
        public static SearchResultModel RetrieveFromCache(string term)
        {
            return MemoryCache.Default.Get(term) as SearchResultModel;
        }

        public static void InsertIntoCache(SearchResultModel searchResults, string term)
        {
            var cachePolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(2) };
            MemoryCache.Default.Add(term, searchResults, cachePolicy);
        }
    }
}