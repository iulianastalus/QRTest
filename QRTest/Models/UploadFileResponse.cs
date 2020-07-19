using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRTest.Models
{
    public class UploadFileResponse
    {
        public string Type { get; set; }
        public List<ResponseSymbol> Symbol { get; set; }

        public class ResponseSymbol
        {
            public int Seq { get; set; }
            public string Data { get; set; }
            public string Error { get; set; }
        }

    }
}
