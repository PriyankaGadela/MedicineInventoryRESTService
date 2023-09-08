using System;
using System.Collections.Generic;

namespace WebRestApp.Models.Db;

public partial class OperatorDetail
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public long? PhoneNumber { get; set; }

    public string? Password { get; set; }

    public byte? ActiveStatus { get; set; }

    public DateTime? DoB { get; set; }
}
