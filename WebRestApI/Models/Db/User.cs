using System;
using System.Collections.Generic;

namespace WebRestApp.Models.Db;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }
}
