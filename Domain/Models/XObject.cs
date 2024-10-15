using System;
using System.Collections.Generic;

namespace Domain.Models;

public class XObject
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual XClass Class { get; set; } = null!;

    public virtual ICollection<XStringValue> StringValues { get; set; } = new List<XStringValue>();
}
