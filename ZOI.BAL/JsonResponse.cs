using System;
using System.Collections.Generic;
using System.Text;

namespace ZOI.BAL
{
    public class JsonResponse
    {
        public string Status { get; set; } = "F";
        
        public string Message { get; set; }
        
        public object Data { get; set; }

        public string LastUpdatedOn { get; set; }
       
        public string InceptionDate { get; set; }

    }
}
