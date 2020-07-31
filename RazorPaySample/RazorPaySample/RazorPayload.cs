using System;
using System.Collections.Generic;
using System.Text;

namespace RazorPaySample
{
    public class RazorPayload
    {
        public int amount { get; set; }
        public string currency { get; set; }
        public string receipt { get; set; }
        public int payment_capture { get; set; }
    }
    public class RazorResp
    {
        public string id { get; set; }
        public string entity { get; set; }
        public int amount { get; set; }
        public int amount_paid { get; set; }
        public int amount_due { get; set; }
        public string currency { get; set; }
        public string receipt { get; set; }
        public object offer_id { get; set; }
        public string status { get; set; }
        public int attempts { get; set; }
        public List<object> notes { get; set; }
        public int created_at { get; set; }
    }

}
