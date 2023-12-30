namespace Agendo.Server.Models
{
    public record Shift
    {
        public int ISOWeek { get; set; }
        public int ISOYear { get; set; }
        public int DOW { get; set; }
        public int ShiftNR { get; set; }
    }
}
