using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAPI
{
    public abstract class Key
    {
        public string AccessKey { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
    }
}