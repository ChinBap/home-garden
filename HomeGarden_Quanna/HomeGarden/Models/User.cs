using System;
using System.Collections.Generic;

namespace HomeGarden.Models;

public partial class User
{
    public long UserId { get; set; }

    public int RoleId { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public int StatusId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();

    public virtual Role Role { get; set; } = null!;

    public virtual StatusDefinition Status { get; set; } = null!;
}
