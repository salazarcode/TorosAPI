using System;
using System.Collections.Generic;

namespace Infra.Repositories.EF.Models;

public partial class Object
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<StringValue> StringValues { get; set; } = new List<StringValue>();
}
