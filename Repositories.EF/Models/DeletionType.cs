using System;
using System.Collections.Generic;

namespace Infra.Repositories.EF.Models;

public partial class DeletionType
{
    public string Name { get; set; } = null!;

    public virtual ICollection<RelationDetail> RelationDetails { get; set; } = new List<RelationDetail>();
}
