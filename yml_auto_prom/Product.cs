using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yml_auto_prom
{ 
    class Product
    {


        public string id { get; set; }
        public string name { get; set; }
        public string fulldescription { get; set; }
        public decimal price { get; set; }
        public decimal oldprice { get; set; }

        public string published { get; set; }
        public string manufactured { get; set; }
        public string sku { get; set; }
        public string url { get; set; }
        public string vendor { get; set; }
        public Picture picture { get; set; }
        public string catId { get; set; }
        public string stockquantity { get; set; }

        public bool preorder { get; set; }
       

    }
}
