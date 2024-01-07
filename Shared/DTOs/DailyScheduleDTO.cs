namespace Agendo.Shared.DTOs
{
    public record DailyScheduleDTO
    {
        public int Nr { get; set; }
        public string Name{ get; set; }
        public int Hours { get; set; }
        public string Color { get; set; }
    }
}
