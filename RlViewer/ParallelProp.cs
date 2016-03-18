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
            _options = new ParallelOptions { MaxDegreeOfParallelism = _maxThread };
            _chunks = System.Collections.Concurrent.Partitioner.Create(0, dataSize, dataSize / _maxThread);
        }


        private int _maxThread = Environment.ProcessorCount;

        public int MaxThread
        {
            get
            {
                return _maxThread;
            }
        }

        private ParallelOptions _options;
        public ParallelOptions Options
        {
            get
            {
                return _options;
            }
        }

        private OrderablePartitioner<Tuple<int, int>> _chunks;
        public OrderablePartitioner<Tuple<int, int>> Chunks
        {
            get
            {
                return _chunks;
            }
        }
    }
}
