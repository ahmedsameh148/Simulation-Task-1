using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class Customer
    {
        public int RandomInterArrival { set; get; }
        public int InterArrivalTime { set; get; }
        public int CustomerNumber { set; get; }
        public int ArrivalTime { set; get; }
    }
}
