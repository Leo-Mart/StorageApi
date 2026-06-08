using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.DTOs
{
    public class StatsDto
    {
        public int TotalCount { get; set; }
        public int TotalInventoryValue { get; set; }
        public int AvergePrice { get; set;}
    }
}