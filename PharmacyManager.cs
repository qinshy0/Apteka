// TODO:
// 1. Реализовать хранение лекарств, клиентов и рецептов
// 2. Реализовать поиск лекарств по названию и категории
// 3. Реализовать учет продаж и проверку рецептов


using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy
{
    public class PharmacyManager
    {
        private List<Medicine> medicines = new List<Medicine>();
        private List<Customer> customers = new List<Customer>();
        private List<Prescription> prescriptions = new List<Prescription>();

        private int nextCustomerId = 1;
        private int nextReceiptNumber = 1000;
        private decimal dailyRevenue = 0;

        public void AddMedicine(Medicine medicine)
        {
            if (medicine != null && !medicines.Any(m => m.Id == medicine.Id))
                medicines.Add(medicine);
        }

        public void AddCustomer(Customer customer)
        {
            if (customer != null)
            {
                customer.Id = nextCustomerId++;
                customers.Add(customer);
            }
        }

        public void AddPrescription(Prescription prescription)
        {
            if (prescription != null)
                prescriptions.Add(prescription);
        }

        public List<Medicine> GetAllMedicines() => medicines.ToList();
        public List<Customer> GetAllCustomers() => customers.ToList();
        public List<Prescription> GetAllPrescriptions() => prescriptions.ToList();
        public int GetNextReceiptNumber() => nextReceiptNumber++;
    }
}
