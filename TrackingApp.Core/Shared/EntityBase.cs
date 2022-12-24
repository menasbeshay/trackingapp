using TrackingApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackingApp.Core.Shared
{
    public class EntityBase
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
        public int? CreatedByNameId { get; set; }
        public int? ModifiedByNameId { get; set; }
        public ApplicationUser CreatedByName { get; set; }
        public ApplicationUser ModifiedByName { get; set; }
    }
}
