using System;
using System.Collections.Generic;

namespace WebRestApp.Models.Db;

public partial class MedicineInventory
{
    public int MedicineId { get; set; }

    public string MedicineName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public DateTime Expirydate { get; set; }

    public int Stocklevel { get; set; }

    public decimal Price { get; set; }

    public virtual MedicineCategory? Category { get; set; }
}
