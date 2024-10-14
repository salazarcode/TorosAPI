using System;
using System.Collections.Generic;

namespace Infra.Repositories.EF.Models;

public partial class Property
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public int PropertyClassId { get; set; }

    public string Key { get; set; } = null!;

    public string? Name { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Class PropertyClass { get; set; } = null!;

    public virtual RelationDetail? RelationDetail { get; set; }

    public virtual ICollection<StringValue> StringValues { get; set; } = new List<StringValue>();
}
