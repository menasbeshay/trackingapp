using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int endsOn { get; set; }
    }
}
