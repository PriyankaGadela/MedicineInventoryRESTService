using System;
using System.Collections.Generic;

namespace WebRestApp.Models.Db;

public partial class MedicineCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<MedicineInventory> MedicineInventories { get; set; } = new List<MedicineInventory>();
}
