using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yml_auto_prom
{
    class Nastroyki
    {


        public bool ini_preorder { get; set; }
        public bool ini_vsitovaru { get; set; }
        public bool ini_vnayavnosti { get; set; }

        public string ini_node_name { get; set; }
        public List <string> ini_manufact { get; set; }

    }
}
