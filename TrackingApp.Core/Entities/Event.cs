using TrackingApp.Core.Shared;

namespace TrackingApp.Core.Entities
{
    public class Event
    {
        public int eventId { get; set; }
        public string eventName { get; set; }
        public string status { get; set; }
        public int timeFrom { get; set; }
        public int timeTo { get; set; }
        public bool isRepeated { get; set; }
        public string repeatedEvery { get; set; }
        public int startFrom { get; set; }
        public long endsOn { get; set; }
        public int? CreatedByNameId { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
