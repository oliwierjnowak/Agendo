namespace Agendo.Server.Models
{
    public record EmployeeShift
    {
        public int EmpNr { get; set; }
        public int ISOWeek { get; set; }
        public int ISOYear { get; set; }
        public int DOW { get; set; }    
        public int ShiftNR { get; set; }
        public string ShiftName { get; set; }
        public int ShiftHours { get; set; }

    }
}
