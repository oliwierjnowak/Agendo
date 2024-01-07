
namespace Agendo.Shared.DTOs
{
    public record AddEmployeeShiftDTO
    {
        public IEnumerable<int> Domains { get; set; }
        public DateTime Start { get; set; }
        public int ShiftNR { get; set; }
    }
}
