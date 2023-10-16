using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendo.Shared.Form.CreateEmployeeShift
{
    public record CreateEmployeeShift
    {
        public int EmpNr { get; set; }
        public DateTime ShiftDate { get; set; }
        public int ShiftNr { get; set; }

    }
}
