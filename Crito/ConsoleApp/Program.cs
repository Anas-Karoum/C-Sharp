using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Contact
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName} - {PhoneNumber} - {Email} - {Address}";
    }
}

class Program
{
    private static List<Contact>? contacts;

    static Program()
    {
        contacts = new List<Contact>();
    }

    static void Main()
    {
        LoadContacts();

        while (true)
        {
            Console.Clear(); // Rensa konsolfönstret vid varje iteration

            PrintHeader("Välkommen till Kontakthanteraren");
            Console.WriteLine("1. Lägg till kontakt");
            Console.WriteLine("2. Lista alla kontakter");
            Console.WriteLine("3. Visa detaljerad information om en kontakt");
            Console.WriteLine("4. Ta bort kontakt");
            Console.WriteLine("5. Avsluta");

            int choice = GetChoice();

            switch (choice)
            {
                case 1:
                    AddContact();
                    break;
                case 2:
                    ListContacts();
                    break;
                case 3:
                    ShowContactDetails();
                    break;
                case 4:
                    RemoveContact();
                    break;
                case 5:
                    SaveContacts();
                    return;
            }
        }
    }

    private static void PrintHeader(string text)
    {
        Console.WriteLine(text.ToUpper());
        Console.WriteLine();
    }

    private static void LoadContacts()
    {
        try
        {
            string json = File.ReadAllText("contacts.json");
            contacts = JsonConvert.DeserializeObject<List<Contact>>(json) ?? new List<Contact>();
        }
        catch (FileNotFoundException)
        {
            contacts = new List<Contact>();
        }
    }

    private static void SaveContacts()
    {
        if (contacts != null)
        {
            string json = JsonConvert.SerializeObject(contacts, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("contacts.json", json);
        }
        else
        {
            Console.WriteLine("Fel: Kontakter är null.");
        }
    }

    private static void AddContact()
    {
        Console.WriteLine("Lägg till en ny kontakt:\n");

        Console.Write("Förnamn: ");
        string? firstName = Console.ReadLine();

        Console.Write("Efternamn: ");
        string? lastName = Console.ReadLine();

        Console.Write("Telefonnummer: ");
        string? phoneNumber = Console.ReadLine();

        Console.Write("E-postadress: ");
        string? email = Console.ReadLine();

        Console.Write("Adress: ");
        string? address = Console.ReadLine();

        if (contacts != null)
        {
            Contact newContact = new Contact
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Address = address
            };

            contacts.Add(newContact);

            Console.WriteLine("\nKontakt tillagd!");
        }
        else
        {
            Console.WriteLine("\nFel: Kontakter är null.");
        }

        Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
        Console.ReadKey();
    }

    private static void ListContacts()
    {
        if (contacts != null)
        {
            Console.WriteLine("\nAlla kontakter:\n");

            foreach (var contact in contacts)
            {
                Console.WriteLine(contact);
            }
        }
        else
        {
            Console.WriteLine("\nFel: Kontakter är null.");
        }

        Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
        Console.ReadKey();
    }

    private static void ShowContactDetails()
    {
        Console.Write("\nAnge e-postadress för att visa detaljerad information: ");
        string? email = Console.ReadLine();

        if (contacts != null)
        {
            Contact? contact = contacts?.FirstOrDefault(c => c.Email?.Equals(email, StringComparison.OrdinalIgnoreCase) ?? false);

            if (contact != null)
            {
                Console.WriteLine("\nDetaljerad information om kontakten:\n");
                Console.WriteLine(contact);
            }
            else
            {
                Console.WriteLine("\nKontakten hittades inte.");
            }
        }
        else
        {
            Console.WriteLine("\nFel: Kontakter är null.");
        }

        Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
        Console.ReadKey();
    }

    private static void RemoveContact()
    {
        Console.Write("\nAnge e-postadress för att ta bort kontakten: ");
        string? email = Console.ReadLine();

        if (contacts != null)
        {
            Contact? contactToRemove = contacts?.FirstOrDefault(c => c.Email?.Equals(email, StringComparison.OrdinalIgnoreCase) ?? false);

            if (contactToRemove != null)
            {
                contacts?.Remove(contactToRemove);
                Console.WriteLine("\nKontakten har tagits bort.");
            }
            else
            {
                Console.WriteLine("\nKontakten hittades inte.");
            }
        }
        else
        {
            Console.WriteLine("\nFel: Kontakter är null.");
        }

        Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
        Console.ReadKey();
    }

    private static int GetChoice()
    {
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
        {
            Console.WriteLine("\nOgiltigt val. Försök igen.");
        }
        return choice;
    }
}
