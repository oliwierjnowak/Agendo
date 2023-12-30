using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendo.Shared.Form.CreateEmployeeShift
{
    public class CreateMultipleEmpShift
    {
        public IEnumerable<int>? AddedDomains { get; set; }
        public IEnumerable<int>? RemovedDomains{ get; set; }
        public DateTime ShiftDate { get; set; }
        public int ShiftNr { get; set; }

    }
}
