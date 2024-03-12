namespace Agendo.Shared.Form
{
    public record SequenceForm
    {
        public List<int> weekDays;
        public List<int> domainsIDs;
        public int ISOWeekFrom;
        public int ISOWeekTo;
        public int shiftNR;
        public int year;
    }

    public class CreateSequenceForm
    {
        public List<int> WeekDays { get; set; }
        public List<int> DomainsIDs { get; set; }
        public int ISOWeekFrom { get; set; }
        public int ISOWeekTo { get; set; }
        public int ShiftNR { get; set; }
        public int Year { get; set; }
    }

    public record MultipleSelectionForm
    {
        public List<DateOnly> dates;
        public List<int> domains;
        public int shiftNR;
    }
}
