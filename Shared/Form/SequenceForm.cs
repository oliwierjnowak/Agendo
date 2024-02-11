namespace Agendo.Shared.Form
{
    public record SequenceForm
    {
        public List<DayOfWeek> weekDays;
        public List<int> domainsIDs;
        public int ISOWeekFrom;
        public int ISOWeekTo;
        public int shiftNR;
        public int year;
    }

    public record MultipleSelectionForm
    {
        public List<DateOnly> dates;
        public List<int> domains;
        public int shiftNR;
    }
}
