using System;
using System.Collections.Generic;

namespace Infra.Repositories.EF.Models;

public partial class AbstractPropertyDetail
{
    public int PropertyId { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? DeleteBehaviour { get; set; }

    public virtual DeleteBehaviour? DeleteBehaviourNavigation { get; set; }

    public virtual Property Property { get; set; } = null!;
}
