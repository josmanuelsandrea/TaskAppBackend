using System;
using System.Collections.Generic;

namespace TaskApp.Infrastructure.Persistence.PostgreSQL.Models;

public partial class TaskM
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual User? User { get; set; }
}
