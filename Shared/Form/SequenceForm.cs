namespace Agendo.Shared.Form
{
    public record SequenceForm
    {
        public List<int> WeekDays;
        public List<int> DomainsIDs;
        public int ISOWeekFrom;
        public int ISOWeekTo;
        public int ShiftNR;
        public int Year;
    }

    public record MultipleSelectionForm
    {
        public List<DateOnly> Dates;
        public List<int> Domains;
        public int ShiftNR;
    }
    public record DayWeekYear
    {
        public int DayNumber;
        public int Year;
        public int WeekNumber;
    }
}
