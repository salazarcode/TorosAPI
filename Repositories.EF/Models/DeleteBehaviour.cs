using System;
using System.Collections.Generic;

namespace Infra.Repositories.EF.Models;

public partial class DeleteBehaviour
{
    public string Name { get; set; } = null!;

    public virtual ICollection<AbstractPropertyDetail> AbstractPropertyDetails { get; set; } = new List<AbstractPropertyDetail>();
}
