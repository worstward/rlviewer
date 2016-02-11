using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace RlViewer
{
    public class ParallelProperties
    {

        public ParallelProperties(int offset, int dataSize)
        {
            options = new ParallelOptions { MaxDegreeOfParallelism = maxThread };
            chunks = System.Collections.Concurrent.Partitioner.Create(0, dataSize, dataSize / maxThread);
        }


        private int maxThread = Environment.ProcessorCount;

        public int MaxThread
        {
            get { return maxThread; }
        }

        private ParallelOptions options;
        public ParallelOptions Options
        {
            get
            {
                return options;
            }
        }

        private OrderablePartitioner<Tuple<int, int>> chunks;
        public OrderablePartitioner<Tuple<int, int>> Chunks
        {
            get
            {
                return chunks;
            }
        }
    }
}
