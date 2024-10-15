using System;
using System.Collections.Generic;

namespace Domain.Models;

public class XDeleteBehaviour
{
    public string Name { get; set; } = null!;

    public virtual ICollection<XAbstractPropertyDetail> AbstractPropertyDetails { get; set; } = new List<XAbstractPropertyDetail>();
}
