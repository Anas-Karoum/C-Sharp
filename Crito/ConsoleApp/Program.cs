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
        // Returnerar en sträng som representerar kontaktinformation
        return $"{FirstName} {LastName} - {PhoneNumber} - {Email} - {Address}";
    }
}

class Program
{
    private static List<Contact>? contacts;

    static Program()
    {
        // Skapar en ny lista för kontakter
        contacts = new List<Contact>();
    }

    static void Main()
    {
        // Laddar kontakter från fil vid programstart
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
                    AddContact(); // Anropar funktionen för att lägga till en kontakt
                    break;
                case 2:
                    ListContacts(); // Anropar funktionen för att lista alla kontakter
                    break;
                case 3:
                    ShowContactDetails(); // Anropar funktionen för att visa detaljerad information om en kontakt
                    break;
                case 4:
                    RemoveContact(); // Anropar funktionen för att ta bort en kontakt
                    break;
                case 5:
                    SaveContacts(); // Sparar kontakter och avslutar programmet
                    return;
            }
        }
    }

    private static void PrintHeader(string text)
    {
        // Skriver ut en rubrik i versaler
        Console.WriteLine(text.ToUpper());
        Console.WriteLine();
    }

    private static void LoadContacts()
    {
        try
        {
            // Försöker läsa kontakter från JSON-fil
            string json = File.ReadAllText("contacts.json");
            contacts = JsonConvert.DeserializeObject<List<Contact>>(json) ?? new List<Contact>();
        }
        catch (FileNotFoundException)
        {
            // Om filen inte hittas, skapas en ny tom lista för kontakter
            contacts = new List<Contact>();
        }
    }

    private static void SaveContacts()
    {
        if (contacts != null)
        {
            // Sparar kontakter till JSON-fil
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
        // Lägger till en ny kontakt med användarens inmatning
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
            // Skapar en ny kontakt och lägger till den i listan
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
            // Skriver ut alla kontakter i listan
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
        // Visar detaljerad information om en kontakt baserat på e-postadress
        Console.Write("\nAnge e-postadress för att visa detaljerad information: ");
        string? email = Console.ReadLine();

        if (contacts != null)
        {
            // Letar efter kontakten med den angivna e-postadressen
            Contact? contact = contacts?.FirstOrDefault(c => c.Email?.Equals(email, StringComparison.OrdinalIgnoreCase) ?? false);

            if (contact != null)
            {
                // Visar detaljerad information om kontakten
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
        // Tar bort en kontakt baserat på e-postadress
        Console.Write("\nAnge e-postadress för att ta bort kontakten: ");
        string? email = Console.ReadLine();

        if (contacts != null)
        {
            // Letar efter kontakten med den angivna e-postadressen
            Contact? contactToRemove = contacts?.FirstOrDefault(c => c.Email?.Equals(email, StringComparison.OrdinalIgnoreCase) ?? false);

            if (contactToRemove != null)
            {
                // Tar bort kontakten från listan
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
        // Hanterar användarens val av menyalternativ
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
        {
            Console.WriteLine("\nOgiltigt val. Försök igen.");
        }
        return choice;
    }
}
