using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoDialer.Contracts;
using System.Security.Cryptography.X509Certificates;

namespace AutoDialer.Repositories;

public class PhoneRepository : IPhoneRepository
{
    private List<string> phones = default!;
    private int currentIndex;
    private readonly string phonesFilePath;
    private readonly string lastPhoneFilePath;

    public PhoneRepository()
    {
        var baseDir = AppContext.BaseDirectory;
        phonesFilePath = Path.Combine(baseDir, "phones.json");
        lastPhoneFilePath = Path.Combine(baseDir, "lastPhone.json");

        LoadPhones();
    }

    private void LoadPhones()
    {
        try
        {
            var json = File.ReadAllText(phonesFilePath);
            var allPhones = JsonSerializer.Deserialize<List<string>>(json)!;

            currentIndex = LoadLastPhoneIndex();
            phones = allPhones.Skip(currentIndex).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading phones: " + ex.Message);
            phones = new List<string>();
        }
    }

    private int LoadLastPhoneIndex()
    {
        try
        {
            if (File.Exists(lastPhoneFilePath))
            {
                var json = File.ReadAllText(lastPhoneFilePath);
                return JsonSerializer.Deserialize<int>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading last phone index: " + ex.Message);
        }

        return 0;
    }

    private void SaveLastPhoneIndex()
    {
        var json = JsonSerializer.Serialize(currentIndex);
        File.WriteAllText(lastPhoneFilePath, json);
    }

    public string? GetPhone()
    {
        if (currentIndex < phones.Count)
        {
            var phone = phones[currentIndex];
            return phone;
        }

        return null;
    }

    public void SetPhoneAsUsed()
    {
        currentIndex++;
        SaveLastPhoneIndex();
    }
}
