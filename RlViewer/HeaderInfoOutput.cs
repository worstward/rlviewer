using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer
{

    /// <summary>
    /// Incapsulates parsed header info (for output purposes)
    /// </summary>
    public struct HeaderInfoOutput
    {
        public HeaderInfoOutput(string headerName, IEnumerable<Tuple<string, string>> parameters)
        {
            _headerName = headerName;
            _parameters = parameters;
        }

        private string _headerName;

        /// <summary>
        /// Describes header contents
        /// </summary>
        public string HeaderName
        {
          get
          {
              return _headerName;
          }
        }

        private IEnumerable<Tuple<string, string>> _parameters;

        /// <summary>
        /// Name-value pair of header parameters
        /// </summary>
        public IEnumerable<Tuple<string, string>> Params
        {
          get
          {
              return _parameters;
          }
        }
    }
}
