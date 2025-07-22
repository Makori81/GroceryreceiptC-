using System;
using System.Collections.Generic;
using System.IO;

class GroceryItem
{
    public string ItemID { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double Total { get; set; }
}

class Program
{
    static List<GroceryItem> groceryItems = new List<GroceryItem>();
    static double subTotal = 0;
    static double tax = 0;
    static double grandTotal = 0;
    static double VAT_RATE = 0.16; // 16% VAT rate in Kenya

    static void Main()
    {
        bool exit = false;

        while (!exit)
        {
            DisplayMenu();
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ReadGroceryFile();
                    break;
                case "2":
                    CalculateTotals();
                    break;
                case "3":
                    PrintReceipt();
                    break;
                case "4":
                    WriteReceiptToFile();
                    break;
                case "5":
                    exit = true;
                    Console.WriteLine("Thank you for using the Grocery Receipt System!");
                    break;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("===============================================");
        Console.WriteLine("        GROCERY RECEIPT SYSTEM");
        Console.WriteLine("===============================================");
        Console.WriteLine("1. Read Grocery Data from File");
        Console.WriteLine("2. Calculate Totals and Tax");
        Console.WriteLine("3. Print Shopping Receipt");
        Console.WriteLine("4. Save Receipt to File");
        Console.WriteLine("5. Exit");
        Console.WriteLine("===============================================");
    }

    static void ReadGroceryFile()
    {
        Console.Write("Enter the input file path / press Enter for default 'grocery.txt': ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrEmpty(filePath))
        {
            filePath = "grocery.txt";
        }

        try
        {
            groceryItems.Clear(); 

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
               
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    // Split the line by tabs or spaces
                    string[] parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length >= 4)
                    {
                        GroceryItem item = new GroceryItem
                        {
                            ItemID = parts[0],
                            Name = parts[1],
                            Quantity = int.Parse(parts[2]),
                            Price = double.Parse(parts[3])
                        };
                        groceryItems.Add(item);
                    }
                }
            }

            Console.WriteLine($"Successfully read {groceryItems.Count} items from the file.");
            DisplayItems();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: File not found! Please check the file path.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }

    static void DisplayItems()
    {
        Console.WriteLine("\nItems loaded:");
        Console.WriteLine("ID\tName\t\tQty\tPrice");
        Console.WriteLine("----------------------------------------");
        foreach (var item in groceryItems)
        {
            Console.WriteLine($"{item.ItemID}\t{item.Name}\t\t{item.Quantity}\t{item.Price:F2}");
        }
    }

    static void CalculateTotals()
    {
        if (groceryItems.Count == 0)
        {
            Console.WriteLine("No items loaded! Please read data from file first.");
            return;
        }

        subTotal = 0;

      
        foreach (var item in groceryItems)
        {
            item.Total = item.Quantity * item.Price;
            subTotal += item.Total;
        }

        
        tax = subTotal * VAT_RATE;

        // Calculate total
        grandTotal = subTotal + tax;

        Console.WriteLine("Calculations completed successfully!");
        Console.WriteLine($"Sub-total: KSh {subTotal:F2}");
        Console.WriteLine($"Tax (16%): KSh {tax:F2}");
        Console.WriteLine($"Grand Total: KSh {grandTotal:F2}");
    }

    static void PrintReceipt()
    {
        if (groceryItems.Count == 0)
        {
            Console.WriteLine("No items to display! Please read data from file first.");
            return;
        }

        Console.Clear();
        Console.WriteLine("================================================");
        Console.WriteLine("              GROCERY STORE RECEIPT");
        Console.WriteLine("================================================");
        Console.WriteLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine("================================================");
        Console.WriteLine("ID\tItem\t\tQty\tPrice\tTotal");
        Console.WriteLine("------------------------------------------------");

        foreach (var item in groceryItems)
        {
            Console.WriteLine($"{item.ItemID}\t{item.Name}\t\t{item.Quantity}\t{item.Price:F2}\t{item.Total:F2}");
        }

        Console.WriteLine("------------------------------------------------");
        Console.WriteLine($"Sub-total:\t\t\t\tKSh {subTotal:F2}");
        Console.WriteLine($"Tax (16%):\t\t\t\tKSh {tax:F2}");
        Console.WriteLine("================================================");
        Console.WriteLine($"GRAND TOTAL:\t\t\t\tKSh {grandTotal:F2}");
        Console.WriteLine("================================================");
        Console.WriteLine("        Thank you for shopping with us!");
        Console.WriteLine("================================================");
    }

    static void WriteReceiptToFile()
    {
        if (groceryItems.Count == 0)
        {
            Console.WriteLine("No data to write! Please read and process data first.");
            return;
        }

        Console.Write("Enter output file name (or press Enter for 'receipt.txt'): ");
        string fileName = Console.ReadLine();

        if (string.IsNullOrEmpty(fileName))
        {
            fileName = "receipt.txt";
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("================================================");
                writer.WriteLine("              GROCERY STORE RECEIPT");
                writer.WriteLine("================================================");
                writer.WriteLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.WriteLine("================================================");
                writer.WriteLine("ID\tItem\t\tQty\tPrice\tTotal");
                writer.WriteLine("------------------------------------------------");

                foreach (var item in groceryItems)
                {
                    writer.WriteLine($"{item.ItemID}\t{item.Name}\t\t{item.Quantity}\t{item.Price:F2}\t{item.Total:F2}");
                }

                writer.WriteLine("------------------------------------------------");
                writer.WriteLine($"Sub-total:\t\t\t\tKSh {subTotal:F2}");
                writer.WriteLine($"Tax (16%):\t\t\t\tKSh {tax:F2}");
                writer.WriteLine("================================================");
                writer.WriteLine($"GRAND TOTAL:\t\t\t\tKSh {grandTotal:F2}");
                writer.WriteLine("================================================");
                writer.WriteLine("        Thank you for shopping with us!");
                writer.WriteLine("================================================");
            }

            Console.WriteLine($"Receipt successfully saved to '{fileName}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }
    }
}
