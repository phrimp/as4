using DNATesting.SoapClient.ConsoleApp.PhienNT.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DNATesting.SoapClient.ConsoleApp.PhienNT
{
   
    class Program
    {
        private static readonly string DnaTestServiceUrl = "http://localhost:5202/DnaTestsPhienNtSoapService.asmx";
        private static readonly string LociServiceUrl = "http://localhost:5202/LociPhienNtSoapService.asmx";

        static async Task Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("    DNA Testing PhienNT SOAP Client");
            Console.WriteLine("===========================================");

            bool running = true;
            while (running)
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": await GetAllDnaTests(); break;
                        case "2": await GetAllLoci(); break;
                        case "3": await CreateSampleDnaTest(); break;
                        case "4": await CreateSampleLocus(); break;
                        case "5": await GetDnaTestById(); break;
                        case "6": await GetLocusById(); break;
                        case "7": await DeleteDnaTest(); break;
                        case "8": await DeleteLocus(); break;
                        case "0": running = false; break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }

                    if (running)
                    {
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine("Goodbye!");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("=== DNA Testing SOAP Client ===");
            Console.WriteLine("1. Get All DNA Tests");
            Console.WriteLine("2. Get All Loci");
            Console.WriteLine("3. Create Sample DNA Test");
            Console.WriteLine("4. Create Sample Locus");
            Console.WriteLine("5. Get DNA Test by ID");
            Console.WriteLine("6. Get Locus by ID");
            Console.WriteLine("7. Delete DNA Test");
            Console.WriteLine("8. Delete Locus");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");
        }

        // DNA Tests Operations
        static async Task GetAllDnaTests()
        {
            Console.WriteLine("\n--- Getting All DNA Tests ---");

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(DnaTestServiceUrl);
            var factory = new ChannelFactory<IDnaTestsService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var tests = await client.GetDnaTests();

                if (tests?.Count > 0)
                {
                    Console.WriteLine($"Found {tests.Count} DNA tests:");
                    foreach (var test in tests)
                    {
                        Console.WriteLine($"  {test}");
                    }
                }
                else
                {
                    Console.WriteLine("No DNA tests found.");
                }
            }
            finally
            {
                factory.Close();
            }
        }

        static async Task GetAllLoci()
        {
            Console.WriteLine("\n--- Getting All Loci ---");

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(LociServiceUrl);
            var factory = new ChannelFactory<ILociService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var loci = await client.GetLoci();

                if (loci?.Count > 0)
                {
                    Console.WriteLine($"Found {loci.Count} loci:");
                    foreach (var locus in loci)
                    {
                        Console.WriteLine($"  {locus}");
                    }
                }
                else
                {
                    Console.WriteLine("No loci found.");
                }
            }
            finally
            {
                factory.Close();
            }
        }

        static async Task CreateSampleDnaTest()
        {
            Console.WriteLine("\n--- Creating Sample DNA Test ---");

            var test = new DnaTestsPhienNt
            {
                TestType = "Paternity Test",
                Conclusion = "Sample test created by console client",
                ProbabilityOfRelationship = 99.99m,
                RelationshipIndex = 1000.50m,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(DnaTestServiceUrl);
            var factory = new ChannelFactory<IDnaTestsService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var result = await client.CreateDnaTest(test);

                if (result != null)
                {
                    Console.WriteLine($"Created DNA test: {result}");
                }
                else
                {
                    Console.WriteLine("Failed to create DNA test.");
                }
            }
            finally
            {
                factory.Close();
            }
        }

        static async Task CreateSampleLocus()
        {
            Console.WriteLine("\n--- Creating Sample Locus ---");

            var locus = new LociPhienNt
            {
                Name = $"TEST_LOCUS_{DateTime.Now:HHmmss}",
                IsCodis = true,
                Description = "Sample locus created by console client",
                MutationRate = 0.0001m,
                CreatedAt = DateTime.Now
            };

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(LociServiceUrl);
            var factory = new ChannelFactory<ILociService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var result = await client.CreateLocus(locus);

                if (result != null)
                {
                    Console.WriteLine($"Created locus: {result}");
                }
                else
                {
                    Console.WriteLine("Failed to create locus.");
                }
            }
            finally
            {
                factory.Close();
            }
        }

        static async Task GetDnaTestById()
        {
            Console.WriteLine("\n--- Get DNA Test by ID ---");
            Console.Write("Enter DNA Test ID: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(DnaTestServiceUrl);
                var factory = new ChannelFactory<IDnaTestsService>(binding, endpoint);
                var client = factory.CreateChannel();

                try
                {
                    var test = await client.GetDnaTestById(id);

                    if (test != null)
                    {
                        Console.WriteLine($"Found: {test}");
                        if (!string.IsNullOrEmpty(test.Conclusion))
                        {
                            Console.WriteLine($"Conclusion: {test.Conclusion}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"DNA test with ID {id} not found.");
                    }
                }
                finally
                {
                    factory.Close();
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static async Task GetLocusById()
        {
            Console.WriteLine("\n--- Get Locus by ID ---");
            Console.Write("Enter Locus ID: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(LociServiceUrl);
                var factory = new ChannelFactory<ILociService>(binding, endpoint);
                var client = factory.CreateChannel();

                try
                {
                    var locus = await client.GetLocusById(id);

                    if (locus != null)
                    {
                        Console.WriteLine($"Found: {locus}");
                        if (!string.IsNullOrEmpty(locus.Description))
                        {
                            Console.WriteLine($"Description: {locus.Description}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Locus with ID {id} not found.");
                    }
                }
                finally
                {
                    factory.Close();
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static async Task DeleteDnaTest()
        {
            Console.WriteLine("\n--- Delete DNA Test ---");
            Console.Write("Enter DNA Test ID to delete: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write($"Are you sure you want to delete DNA test {id}? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();

                if (confirm == "y" || confirm == "yes")
                {
                    var binding = new BasicHttpBinding();
                    var endpoint = new EndpointAddress(DnaTestServiceUrl);
                    var factory = new ChannelFactory<IDnaTestsService>(binding, endpoint);
                    var client = factory.CreateChannel();

                    try
                    {
                        var result = await client.DeleteDnaTest(id);

                        if (result)
                        {
                            Console.WriteLine($"DNA test {id} deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to delete DNA test.");
                        }
                    }
                    finally
                    {
                        factory.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Delete cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static async Task DeleteLocus()
        {
            Console.WriteLine("\n--- Delete Locus ---");
            Console.Write("Enter Locus ID to delete: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write($"Are you sure you want to delete locus {id}? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();

                if (confirm == "y" || confirm == "yes")
                {
                    var binding = new BasicHttpBinding();
                    var endpoint = new EndpointAddress(LociServiceUrl);
                    var factory = new ChannelFactory<ILociService>(binding, endpoint);
                    var client = factory.CreateChannel();

                    try
                    {
                        var result = await client.DeleteLocus(id);

                        if (result)
                        {
                            Console.WriteLine($"Locus {id} deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to delete locus.");
                        }
                    }
                    finally
                    {
                        factory.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Delete cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }
    }
}