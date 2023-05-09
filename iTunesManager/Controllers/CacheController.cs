using iTunesManager.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace iTunesManager.Controllers
{
    public class CacheController
    {
        public static List<SearchResultModel> RetrieveFromCache(string term)
        {
            return MemoryCache.Default.Get(term) as List<SearchResultModel>;
        }

        public static void InsertIntoCache(List<SearchResultModel> searchResultsList, string term)
        {
            var cachePolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(2) };
            MemoryCache.Default.Add(term, searchResultsList, cachePolicy);
        }
    }
}