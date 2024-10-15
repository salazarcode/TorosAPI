using System;
using System.Collections.Generic;

namespace Domain.Models;

public class XStringValue
{
    public int Id { get; set; }

    public int ObjectId { get; set; }

    public int PropertyId { get; set; }

    public string? Value { get; set; }

    public virtual XObject Object { get; set; } = null!;

    public virtual XProperty Property { get; set; } = null!;
}
