using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendo.Shared.Form.CreateShift
{
    public record CreateShift
    {
        public int Nr { get; set; }
        public string Name { get; set; }

        public int Hours { get; set; }
    }
}
