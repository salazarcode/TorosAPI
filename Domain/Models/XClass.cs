using System;
using System.Collections.Generic;

namespace Domain.Models;

public class XClass
{
    public int Id { get; set; }

    public string Key { get; set; } = null!;

    public bool IsPrimitive { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<XObject> Objects { get; set; } = new List<XObject>();

    public virtual ICollection<XProperty> PropertyClasses { get; set; } = new List<XProperty>();

    public virtual ICollection<XProperty> PropertyPropertyClasses { get; set; } = new List<XProperty>();

    public virtual ICollection<XClass> Classes { get; set; } = new List<XClass>();

    public virtual ICollection<XClass> Parents { get; set; } = new List<XClass>();
}
