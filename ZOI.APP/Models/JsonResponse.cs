using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZOI.APP.Models
{
    public class JsonResponse
    {
        public string Status { get; set; }

        public string Message { get; set; }
        public string LastUpdatedOn { get; set; }
        public string InceptionDate { get; set; }

        public object Data { get; set; }
    }
}
