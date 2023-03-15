using System.Collections;

namespace LuzInga.Application.Services
{
    public class DefaultBloomFilter : IBloomFilter
    {
        private readonly Filter<string> filter;

        public DefaultBloomFilter(Filter<string> filter)
        {
            this.filter = filter;
        }

        public void Add(string data) { 
            this.filter.Add(data);
        }

        public bool MaybeContains(string data)
        {
           return this.filter.Contains(data);
        }

        public static IBloomFilter CreateFrom(
            List<string> items,
            Action<BloomFilterOptions> handler
        )
        {
            var options = new BloomFilterOptions();
            handler?.Invoke(options);
            var filterInstance = new Filter<string>(Math.Max(2 * items.Count, options.MinSize));

            var bloomFilter  = new DefaultBloomFilter(filterInstance);
            
            foreach(var item in items)
            {
                bloomFilter.Add(item);
            }
            
            return bloomFilter;
        }
    }

    public sealed class BloomFilterOptions
    {
        public float? ErrorRate { get; set; }
        public int MinSize { get; set; } = 100;
    }
}
