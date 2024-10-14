using System;
using System.Collections.Generic;

namespace Infra.Repositories.EF.Models;

public partial class RelationDetail
{
    public int PropertyId { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? OnDelete { get; set; }

    public virtual DeletionType? OnDeleteNavigation { get; set; }

    public virtual Property Property { get; set; } = null!;
}
