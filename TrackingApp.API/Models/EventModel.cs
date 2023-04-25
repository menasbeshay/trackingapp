namespace TrackingApp.API.Models
{
    public class EventModel
    {
        public string eventName { get; set; }
        public string status { get; set; }
        public int timeFrom { get; set; }
        public int timeTo { get; set; }
        public bool isRepeated { get; set; }
        public string repeatedEvery { get; set; }
        public int startFrom { get; set; }
        public int endsOn { get; set; }
    }
}
