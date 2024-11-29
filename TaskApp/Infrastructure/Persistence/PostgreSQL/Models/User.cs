using System;
using System.Collections.Generic;

namespace TaskApp.Infrastructure.Persistence.PostgreSQL.Models;

public partial class User
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<TaskM> TasksM { get; set; } = new List<TaskM>();
}
