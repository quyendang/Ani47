using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie10.ViewModels
{
    public class Comment
    {
        public string id { get; set; }
        public string film_id { get; set; }
        public string user { get; set; }
        public string comment { get; set; }
        public string email { get; set; }
    }
}
