using System;
using System.Collections.Generic;

namespace webtmdt.Data;

public partial class Admin
{
    public int AdminId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}
