namespace Agendo.Server.Models
{
    public record EmployeeShiftDTO
    {
        public int EmpNr { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ShiftNR { get; set; }
        public string ShiftName { get; set; }
        public int ShiftHours { get; set; }
        public DomainDTO DomainDTO { get; set; }

        // New property to concatenate ShiftName and DomainName
        public string ShiftAndDomainName
        {
            get
            {
                // Ensure DomainDTO is not null
                if (DomainDTO != null)
                {
                    return $"{ShiftName} - {DomainDTO.Name}";
                }
                else
                {
                    return ShiftName;
                }
            }
        }
    }
}
