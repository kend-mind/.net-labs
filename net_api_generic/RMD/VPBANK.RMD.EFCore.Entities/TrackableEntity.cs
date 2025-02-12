using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TrackableEntities.Common.Core;

namespace VPBANK.RMD.EFCore.Entities
{
    public abstract class TrackableEntity : ITrackable, IMergeable
    {
        [NotMapped]
        [JsonIgnore]
        public TrackingState TrackingState { get; set; } = TrackingState.Unchanged;

        [NotMapped]
        [JsonIgnore]
        public ICollection<string> ModifiedProperties { get; set; } = new List<string>();

        [NotMapped]
        [JsonIgnore]
        public Guid EntityIdentifier { get; set; } = Guid.NewGuid();
    }
}
