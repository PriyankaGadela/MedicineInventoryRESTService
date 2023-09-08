using WebRestApp.Models.Db;





namespace WebRestApp.Models
{
    public class MedicineStockService
    {
        MedicineDbContext context;
        public MedicineStockService(MedicineDbContext context)
        {
            this.context = context;
        }



        public List<MedicineInventory> GetAllMeds()
        {
            List<MedicineInventory> meds = context.MedicineInventories.ToList();
            return meds;
        }



        public MedicineInventory GetMed(int id)
        {
            var med = context.MedicineInventories.SingleOrDefault(c => c.MedicineId == id);
            return med!;
        }
        public bool DeleteMed(int id)
        {
            var med = context.MedicineInventories.SingleOrDefault(c => c.MedicineId == id);
            if (med != null)
            {
                context.MedicineInventories.Remove(med);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdateMed(MedicineInventory md)
        {
            var med = context.MedicineInventories.SingleOrDefault(c => c.MedicineId == md.MedicineId);
            if (med != null)
            {
                med.MedicineName = md.MedicineName;
                med.Price = md.Price;
                context.SaveChanges();
                return true;
            }
            return false;
        }





        public int AddMed(MedicineInventory md)
        {
            context.MedicineInventories.Add(md);
            return context.SaveChanges();
        }



        public List<MedicineInventory> GetMedicinesExpiringNextMonth()
        {
            /*DateTime nextMonth = DateTime.Today.AddMonths(1);
            DateTime endOfMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1).AddMonths(1).AddDays(-1);*/



            /*List<MedicineInventory> expiringMeds = context.MedicineInventories
                .Where(med => med.Expirydate >= nextMonth && med.Expirydate <= endOfMonth)
                .ToList();*/
            List<MedicineInventory> expiringMeds = context.MedicineInventories.Where(m => m.Expirydate >= DateTime.Now && m.Expirydate <= DateTime.Now.AddMonths(1)).ToList();
            return expiringMeds;
            //?? new List<MedicineInventory>();
        }
        public List<MedicineInventory> GetMedicinesReachingCriticalStock(int criticalStockLevel)
        {
            // Fetch medicines with stock levels less than or equal to the critical stock level
            var medicines = context.MedicineInventories
                .Where(m => m.Stocklevel <= criticalStockLevel)
                .ToList();



            return medicines;
        }
        public List<MedicineInventory> GetMedicinesByCategoryId(int categoryId)
        {
            var medicines = context.MedicineInventories
                .Where(m => m.CategoryId == categoryId)
                .ToList();



            return medicines;
        }



        public List<MedicineInventory> SearchMeds(string searchTerm)
        {
            // You can implement your search logic here.
            // For example, search for medicines by name containing the searchTerm.
            List<MedicineInventory> searchResults = (from c in context.MedicineCategories
                                                     join
                                                    m in context.MedicineInventories on c.CategoryId equals m.CategoryId
                                                     where c.CategoryName == searchTerm
                                                     select m).ToList();




            return searchResults;
        }



        /*public List<MedicineInventory> GetMedicinesReachingCriticalStockLevel(int criticalStockLevel)
        {
            List<MedicineInventory> criticalMeds = context.MedicineInventories
                .Where(m => m.Stocklevel <= criticalStockLevel)
                .ToList();

 

            return criticalMeds;
        }*/
    }
}