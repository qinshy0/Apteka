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
    }
}
public bool IsValid()
{
    return DateTime.Now.Date <= ExpiryDate.Date && UsedQuantity < MaxQuantity;
}

public void ShowPrescriptionInfo()
{
    Console.WriteLine("╔════════════════════════════════════════════╗");
    Console.WriteLine($"║                 РЕЦЕПТ #{Id,-13}           ║");
    Console.WriteLine("╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ Пациент:        {PatientName,-25} ║");
    Console.WriteLine($"║ Врач:           {DoctorName,-25} ║");
    Console.WriteLine($"║ Лицензия врача: {DoctorLicense,-25} ║");
    Console.WriteLine($"║ Лекарство:      {MedicineName,-25} ║");
    Console.WriteLine($"║ Выписан:        {IssueDate:dd.MM.yyyy,-25} ║");
    Console.WriteLine($"║ Действует до:   {ExpiryDate:dd.MM.yyyy,-25} ║");
    Console.WriteLine($"║ Количество:     {UsedQuantity}/{MaxQuantity,-22} ║");
    Console.WriteLine($"║ Осталось:       {RemainingQuantity,-25} ║");

    if (IsValid())
        Console.WriteLine("║ Статус:         ДЕЙСТВИТЕЛЕН             ║");
    else if (DateTime.Now.Date > ExpiryDate.Date)
        Console.WriteLine("║ Статус:         ПРОСРОЧЕН                ║");
    else if (IsFullyUsed)
        Console.WriteLine("║ Статус:         ИСПОЛЬЗОВАН             ║");
    else
        Console.WriteLine("║ Статус:         НЕДЕЙСТВИТЕЛЕН          ║");

    Console.WriteLine("╚════════════════════════════════════════════╝");
}

public bool Extend(int additionalDays)
{
    if (additionalDays <= 0) return false;
    ExpiryDate = ExpiryDate.AddDays(additionalDays);
    return true;
}
public bool Use(int quantity)
{
    if (quantity <= 0)
    {
        Console.WriteLine($"Ошибка: Количество должно быть положительным. Запрошено: {quantity}");
        return false;
    }

    if (!IsValid())
    {
        Console.WriteLine($"Ошибка: Рецепт #{Id} недействителен!");
        return false;
    }

    if (UsedQuantity + quantity > MaxQuantity)
    {
        Console.WriteLine($"Ошибка: Превышение допустимого количества по рецепту. " +
                         $"Доступно: {MaxQuantity - UsedQuantity}, запрошено: {quantity}");
        return false;
    }

    UsedQuantity += quantity;
    return true;
}

public double GetUsagePercentage()
{
    return (double)UsedQuantity / MaxQuantity * 100;
}

public override string ToString()
{
    string status = IsValid() ? "✓ Действителен" :
                   (DateTime.Now.Date > ExpiryDate.Date ? "✗ Просрочен" : "✗ Использован");

    return $"Рецепт #{Id}: {PatientName} - {MedicineName} " +
           $"[{UsedQuantity}/{MaxQuantity}] {status}";
}