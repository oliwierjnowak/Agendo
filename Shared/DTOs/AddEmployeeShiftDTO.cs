
namespace Agendo.Shared.DTOs
{
    public record AddEmployeeShiftDTO
    {
        public int EmpNr { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ShiftNR { get; set; }
    }
}
