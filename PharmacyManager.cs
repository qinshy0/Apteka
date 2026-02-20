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
public List<Medicine> FindMedicineByName(string name)
{
    if (string.IsNullOrWhiteSpace(name)) return new List<Medicine>();

    return medicines.Where(m => !m.IsExpired() &&
        m.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
        .OrderBy(m => m.Name).ToList();
}

public List<Medicine> FindMedicineByCategory(string category)
{
    if (string.IsNullOrWhiteSpace(category)) return new List<Medicine>();

    return medicines.Where(m => !m.IsExpired() &&
        m.Category.IndexOf(category, StringComparison.OrdinalIgnoreCase) >= 0)
        .OrderBy(m => m.Name).ToList();
}

public Customer FindCustomerByPhone(string phone)
{
    if (string.IsNullOrWhiteSpace(phone)) return null;

    string cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
    return customers.FirstOrDefault(c =>
        c.Phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "") == cleanPhone);
}

public List<Medicine> GetExpiringMedicines(int daysThreshold = 30)
{
    return medicines.Where(m => m.IsExpiringSoon(daysThreshold) && !m.IsExpired())
        .OrderBy(m => m.ExpiryDate).ToList();
}

public List<Medicine> GetLowStockMedicines(int threshold = 10)
{
    return medicines.Where(m => m.Quantity <= threshold && !m.IsExpired())
        .OrderBy(m => m.Quantity).ToList();
}
public Prescription FindPrescription(string patientName, string medicineName)
{
    if (string.IsNullOrWhiteSpace(patientName) || string.IsNullOrWhiteSpace(medicineName))
        return null;

    return prescriptions.FirstOrDefault(p =>
        p.PatientName.IndexOf(patientName, StringComparison.OrdinalIgnoreCase) >= 0 &&
        p.MedicineName.IndexOf(medicineName, StringComparison.OrdinalIgnoreCase) >= 0 &&
        p.IsValid());
}

public void RecordSale(decimal amount)
{
    if (amount > 0) dailyRevenue += amount;
}

public decimal GetDailyRevenue() => dailyRevenue;

public int GetPrescriptionMedicinesCount()
{
    return medicines.Count(m => m.RequiresPrescription && !m.IsExpired());
}

public void ResetDailyRevenue()
{
    dailyRevenue = 0;
}