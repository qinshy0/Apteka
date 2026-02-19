// TODO:
// 1. Реализовать хранение информации о рецепте
// 2. Реализовать проверку срока действия рецепта
// 3. Реализовать использование рецепта при покупке


using System;

namespace Pharmacy
{
    public class Prescription
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string MedicineName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MaxQuantity { get; set; }
        public int UsedQuantity { get; set; }
        public string DoctorLicense { get; set; }

        public bool IsFullyUsed => UsedQuantity >= MaxQuantity;
        public int RemainingQuantity => MaxQuantity - UsedQuantity;

        public Prescription(int id, string patient, string doctor, string medicine,
                           int maxQuantity, string doctorLicense)
        {
            Id = id;
            PatientName = patient;
            DoctorName = doctor;
            MedicineName = medicine;
            IssueDate = DateTime.Now;
            ExpiryDate = IssueDate.AddDays(30);
            MaxQuantity = maxQuantity > 0 ? maxQuantity : 1;
            UsedQuantity = 0;
            DoctorLicense = doctorLicense ?? "Не указана";
        }

        public Prescription(int id, string patient, string doctor, string medicine,
                           int maxQuantity, string doctorLicense, int daysValid)
            : this(id, patient, doctor, medicine, maxQuantity, doctorLicense)
        {
            ExpiryDate = IssueDate.AddDays(daysValid);
        }

        public bool IsValid() => DateTime.Now.Date <= ExpiryDate.Date && UsedQuantity < MaxQuantity;

        public bool Use(int quantity)
        {
            if (quantity <= 0 || !IsValid() || UsedQuantity + quantity > MaxQuantity)
                return false;

            UsedQuantity += quantity;
            return true;
        }

        public void ShowPrescriptionInfo()
        {
            Console.WriteLine($"=== РЕЦЕПТ #{Id} ===");
            Console.WriteLine($"Пациент: {PatientName}");
            Console.WriteLine($"Врач: {DoctorName} (лиц. {DoctorLicense})");
            Console.WriteLine($"Лекарство: {MedicineName}");
            Console.WriteLine($"Действителен до: {ExpiryDate:dd.MM.yyyy}");
            Console.WriteLine($"Использовано: {UsedQuantity}/{MaxQuantity}");
            Console.WriteLine($"Статус: {(IsValid() ? "ДЕЙСТВИТЕЛЕН" : "НЕДЕЙСТВИТЕЛЕН")}");
        }
    }
}