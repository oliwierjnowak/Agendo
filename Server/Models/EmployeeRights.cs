namespace Agendo.Server.Models
{
    public record EmployeeRights
    {
        public int AuthType { get; set; }
        public int Superior { get; set; }
        public int Emp { get; set; }
        public byte Enabled { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
