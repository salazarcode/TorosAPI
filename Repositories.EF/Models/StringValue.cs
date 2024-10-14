using System;
using System.Collections.Generic;

namespace Infra.Repositories.EF.Models;

public partial class StringValue
{
    public int Id { get; set; }

    public int ObjectId { get; set; }

    public int PropertyId { get; set; }

    public string? Value { get; set; }

    public virtual Object Object { get; set; } = null!;

    public virtual Property Property { get; set; } = null!;
}
