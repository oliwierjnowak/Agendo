
namespace Agendo.Shared.Form.CreateEmployeeShift
{
    public class CreateMultipleEmpShift
    {
        public IEnumerable<int>? AddedDomains { get; set; }
        public IEnumerable<int>? RemovedDomains{ get; set; }
        public DateTime ShiftDate { get; set; }
        public int ShiftNr { get; set; }
        public IEnumerable<int>? NotChangedDomains{ get; set; }
    }
}
