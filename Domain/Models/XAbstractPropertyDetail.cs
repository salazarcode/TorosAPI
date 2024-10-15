using System;
using System.Collections.Generic;

namespace Domain.Models;

public class XAbstractPropertyDetail
{
    public int PropertyId { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? DeleteBehaviour { get; set; }

    public virtual XDeleteBehaviour? DeleteBehaviourNavigation { get; set; }

    public virtual XProperty Property { get; set; } = null!;
}
