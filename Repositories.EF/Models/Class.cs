using System;
using System.Collections.Generic;

namespace Infra.Repositories.EF.Models;

public partial class Class
{
    public int Id { get; set; }

    public string Key { get; set; } = null!;

    public bool IsPrimitive { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Object> Objects { get; set; } = new List<Object>();

    public virtual ICollection<Property> PropertyClasses { get; set; } = new List<Property>();

    public virtual ICollection<Property> PropertyPropertyClasses { get; set; } = new List<Property>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Class> Parents { get; set; } = new List<Class>();
}
