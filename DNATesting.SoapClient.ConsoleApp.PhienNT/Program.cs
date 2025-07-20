// File: DNATesting.SoapClient.ConsoleApp.PhienNT/Program.cs
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DNATesting.SoapClient.ConsoleApp.PhienNT
{
    class Program
    {
        private static readonly string ServiceUrlHttp = "http://localhost:5202/DnaTestsPhienNtSoapService.asmx";
        private static readonly string LociServiceUrlHttp = "http://localhost:5202/LociPhienNtSoapService.asmx";

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
                        case "1":
                            await TestDnaTests();
                            break;
                        case "2":
                            await TestLoci();
                            break;
                        case "3":
                            await TestCreateDnaTest();
                            break;
                        case "4":
                            await TestCreateLocus();
                            break;
                        case "0":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
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

            Console.WriteLine("Thank you for using DNA Testing PhienNT SOAP Client!");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Test Get All DNA Tests");
            Console.WriteLine("2. Test Get All Loci");
            Console.WriteLine("3. Test Create DNA Test");
            Console.WriteLine("4. Test Create Locus");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
        }

        static async Task TestDnaTests()
        {
            Console.WriteLine("\n--- Testing DNA Tests Service ---");

            // Note: In a real implementation, you would create proper SOAP client proxy
            // This is just a demonstration of the service endpoints
            Console.WriteLine($"Service URL: {ServiceUrlHttp}");
            Console.WriteLine("Available operations:");
            Console.WriteLine("- GetDnaTestsAsync()");
            Console.WriteLine("- GetDnaTestByIdAsync(int id)");
            Console.WriteLine("- CreateDnaTestAsync(DnaTestsPhienNt test)");
            Console.WriteLine("- UpdateDnaTestAsync(int id, DnaTestsPhienNt test)");
            Console.WriteLine("- DeleteDnaTestAsync(int id)");
            Console.WriteLine("- SearchDnaTestsAsync(string testType, bool? isCompleted)");

            Console.WriteLine("\nService is ready for SOAP requests!");
        }

        static async Task TestLoci()
        {
            Console.WriteLine("\n--- Testing Loci Service ---");

            Console.WriteLine($"Service URL: {LociServiceUrlHttp}");
            Console.WriteLine("Available operations:");
            Console.WriteLine("- GetLociAsync()");
            Console.WriteLine("- GetLocusByIdAsync(int id)");
            Console.WriteLine("- GetLocusByNameAsync(string name)");
            Console.WriteLine("- CreateLocusAsync(LociPhienNt locus)");
            Console.WriteLine("- UpdateLocusAsync(int id, LociPhienNt locus)");
            Console.WriteLine("- DeleteLocusAsync(int id)");
            Console.WriteLine("- SearchLociAsync(string name, bool? isCodis)");
            Console.WriteLine("- GetCodisLociAsync()");

            Console.WriteLine("\nService is ready for SOAP requests!");
        }

        static async Task TestCreateDnaTest()
        {
            Console.WriteLine("\n--- Sample DNA Test Creation ---");
            Console.WriteLine("Sample DNA Test data:");
            Console.WriteLine("{");
            Console.WriteLine("  \"TestType\": \"Paternity Test\",");
            Console.WriteLine("  \"Conclusion\": \"Sample test conclusion\",");
            Console.WriteLine("  \"ProbabilityOfRelationship\": 99.99,");
            Console.WriteLine("  \"RelationshipIndex\": 1000.50,");
            Console.WriteLine("  \"IsCompleted\": false");
            Console.WriteLine("}");
            Console.WriteLine("\nUse SOAP client to send this data to CreateDnaTestAsync()");
        }

        static async Task TestCreateLocus()
        {
            Console.WriteLine("\n--- Sample Locus Creation ---");
            Console.WriteLine("Sample Locus data:");
            Console.WriteLine("{");
            Console.WriteLine("  \"Name\": \"D3S1358\",");
            Console.WriteLine("  \"IsCodis\": true,");
            Console.WriteLine("  \"Description\": \"CODIS core locus\",");
            Console.WriteLine("  \"MutationRate\": 0.0001");
            Console.WriteLine("}");
            Console.WriteLine("\nUse SOAP client to send this data to CreateLocusAsync()");
        }
    }
}