using System;
using System.Collections.Generic;

namespace Domain.Models;

public class XProperty
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public int PropertyClassId { get; set; }

    public string Key { get; set; } = null!;

    public string? Name { get; set; }

    public virtual XAbstractPropertyDetail? AbstractPropertyDetail { get; set; }

    public virtual XClass Class { get; set; } = null!;

    public virtual XClass PropertyClass { get; set; } = null!;

    public virtual ICollection<XStringValue> StringValues { get; set; } = new List<XStringValue>();
}
