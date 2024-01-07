namespace Agendo.Server.Models
{
    public record EmployeeShiftDTO
    {
        public IEnumerable<DomainDTO> Domains { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ShiftNR { get; set; }
        public string ShiftName { get; set; }
        public int ShiftHours { get; set; }
    }
}
