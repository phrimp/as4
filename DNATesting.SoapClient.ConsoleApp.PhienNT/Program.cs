using DNATesting.SoapClient.ConsoleApp.PhienNT.Models;
using System;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace DNATesting.SoapClient.ConsoleApp.PhienNT
{
    class Program
    {
        private static readonly string DnaTestServiceUrl = "http://localhost:5202/DnaTestsPhienNtSoapService.asmx";
        private static readonly string LociServiceUrl = "http://localhost:5202/LociPhienNtSoapService.asmx";

        // Alternative URLs to try if the above don't work:
        // private static readonly string DnaTestServiceUrl = "https://localhost:5202/DnaTestsPhienNtSoapService.asmx";
        // private static readonly string DnaTestServiceUrl = "http://localhost:5201/DnaTestsPhienNtSoapService.asmx";
        // private static readonly string DnaTestServiceUrl = "http://127.0.0.1:5202/DnaTestsPhienNtSoapService.asmx";

        static void Main(string[] args)
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
                        // Connection test
                        case "0": TestServiceConnection(); break;

                        // DNA Tests
                        case "1": GetAllDnaTests(); break;
                        case "2": CreateSampleDnaTest(); break;
                        case "3": GetDnaTestById(); break;
                        case "4": UpdateDnaTest(); break;
                        case "5": DeleteDnaTest(); break;
                        case "6": SearchDnaTests(); break;

                        // Loci
                        case "7": GetAllLoci(); break;
                        case "8": CreateSampleLocus(); break;
                        case "9": GetLocusById(); break;
                        case "10": GetLocusByName(); break;
                        case "11": UpdateLocus(); break;
                        case "12": DeleteLocus(); break;
                        case "13": SearchLoci(); break;
                        case "14": GetCodisLoci(); break;

                        case "99": running = false; break;
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
            Console.WriteLine("Connection:");
            Console.WriteLine("0. Test Service Connection");
            Console.WriteLine();
            Console.WriteLine("DNA Tests:");
            Console.WriteLine("1. Get All DNA Tests");
            Console.WriteLine("2. Create Sample DNA Test");
            Console.WriteLine("3. Get DNA Test by ID");
            Console.WriteLine("4. Update DNA Test");
            Console.WriteLine("5. Delete DNA Test");
            Console.WriteLine("6. Search DNA Tests");
            Console.WriteLine();
            Console.WriteLine("Loci:");
            Console.WriteLine("7. Get All Loci");
            Console.WriteLine("8. Create Sample Locus");
            Console.WriteLine("9. Get Locus by ID");
            Console.WriteLine("10. Get Locus by Name");
            Console.WriteLine("11. Update Locus");
            Console.WriteLine("12. Delete Locus");
            Console.WriteLine("13. Search Loci");
            Console.WriteLine("14. Get CODIS Loci");
            Console.WriteLine();
            Console.WriteLine("99. Exit");
            Console.Write("Choose: ");
        }

        // Connection Testing
        static void TestServiceConnection()
        {
            Console.WriteLine("\n--- Testing Service Connection ---");

            string[] urlsToTest = {
                DnaTestServiceUrl,
                LociServiceUrl,
                "http://localhost:5201/DnaTestsPhienNtSoapService.asmx",
                "https://localhost:5202/DnaTestsPhienNtSoapService.asmx",
                "http://127.0.0.1:5202/DnaTestsPhienNtSoapService.asmx"
            };

            using var httpClient = new System.Net.Http.HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(5);

            foreach (var url in urlsToTest)
            {
                Console.WriteLine($"\nTesting: {url}");
                try
                {
                    var response = httpClient.GetAsync($"{url}?wsdl").Result;
                    Console.WriteLine($"  ✅ Status: {response.StatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        if (content.Contains("wsdl:definitions"))
                        {
                            Console.WriteLine("  ✅ Valid WSDL response detected");
                        }
                        else
                        {
                            Console.WriteLine("  ⚠️  Response received but doesn't look like WSDL");
                        }
                    }
                }
                catch (AggregateException ex)
                {
                    var innerEx = ex.InnerException ?? ex;
                    Console.WriteLine($"  ❌ Failed: {innerEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ❌ Failed: {ex.Message}");
                }
            }

            Console.WriteLine("\nIf none of these work, please:");
            Console.WriteLine("1. Check that your SOAP service is running");
            Console.WriteLine("2. Verify the correct URL and port in your service configuration");
            Console.WriteLine("3. Check Windows Firewall settings");
            Console.WriteLine("4. Try running this app as Administrator");
        }

        // DNA Tests Operations
        static void GetAllDnaTests()
        {
            Console.WriteLine("\n--- Getting All DNA Tests ---");

            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1000000; // 1MB
            binding.ReaderQuotas.MaxArrayLength = 1000000;
            binding.ReaderQuotas.MaxStringContentLength = 1000000;

            var endpoint = new EndpointAddress(DnaTestServiceUrl);
            var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                Console.WriteLine("Calling GetDnaTests via SOAP...");
                var tests = client.GetDnaTests();

                if (tests != null && tests.Length > 0)
                {
                    Console.WriteLine($"Found {tests.Length} DNA tests:");
                    foreach (var test in tests)
                    {
                        Console.WriteLine($"  {test}");
                        if (!string.IsNullOrEmpty(test.Conclusion))
                        {
                            Console.WriteLine($"    Conclusion: {test.Conclusion}");
                        }
                        if (test.CreatedAt.HasValue)
                        {
                            Console.WriteLine($"    Created: {test.CreatedAt:yyyy-MM-dd HH:mm:ss}");
                        }
                        Console.WriteLine();
                    }
                }
                else if (tests != null && tests.Length == 0)
                {
                    Console.WriteLine("Service returned empty array - no DNA tests found.");
                }
                else
                {
                    Console.WriteLine("Service returned null - this might indicate a service issue.");
                }
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                Console.WriteLine($"SOAP Fault: {ex.Detail?.Message ?? ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.Detail?.StackTrace}");
            }
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine($"SOAP Fault: {ex.Message}");
                if (ex.Code != null)
                {
                    Console.WriteLine($"Fault Code: {ex.Code.Name}");
                }
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                Console.WriteLine($"Communication Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.GetType().Name}");
                Console.WriteLine($"Message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            finally
            {
                try
                {
                    if (client is System.ServiceModel.ICommunicationObject commObj)
                    {
                        if (commObj.State == System.ServiceModel.CommunicationState.Faulted)
                            commObj.Abort();
                        else
                            commObj.Close();
                    }
                    factory.Close();
                }
                catch
                {
                    factory.Abort();
                }
            }
        }

        static void CreateSampleDnaTest()
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
            var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var result = client.CreateDnaTest(test);

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

        static void GetDnaTestById()
        {
            Console.WriteLine("\n--- Get DNA Test by ID ---");
            Console.Write("Enter DNA Test ID: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(DnaTestServiceUrl);
                var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
                var client = factory.CreateChannel();

                try
                {
                    var test = client.GetDnaTestById(id);

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

        static void UpdateDnaTest()
        {
            Console.WriteLine("\n--- Update DNA Test ---");
            Console.Write("Enter DNA Test ID to update: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(DnaTestServiceUrl);
                var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
                var client = factory.CreateChannel();

                try
                {
                    var existingTest = client.GetDnaTestById(id);
                    if (existingTest == null)
                    {
                        Console.WriteLine($"DNA test with ID {id} not found.");
                        return;
                    }

                    Console.WriteLine($"Current test: {existingTest}");
                    Console.Write("Enter new conclusion (or press Enter to keep current): ");
                    var newConclusion = Console.ReadLine();

                    if (!string.IsNullOrEmpty(newConclusion))
                    {
                        existingTest.Conclusion = newConclusion;
                    }

                    var result = client.UpdateDnaTest(id, existingTest);
                    Console.WriteLine($"Updated DNA test: {result}");
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

        static void DeleteDnaTest()
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
                    var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
                    var client = factory.CreateChannel();

                    try
                    {
                        var result = client.DeleteDnaTest(id);

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

        static void SearchDnaTests()
        {
            Console.WriteLine("\n--- Search DNA Tests ---");
            Console.Write("Enter test type (or press Enter for all): ");
            var testType = Console.ReadLine();

            Console.Write("Search for completed tests only? (y/n): ");
            var completedInput = Console.ReadLine()?.ToLower();
            bool isCompleted = completedInput == "y" || completedInput == "yes";

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(DnaTestServiceUrl);
            var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var tests = client.SearchDnaTests(testType ?? "", isCompleted);

                if (tests != null && tests.Length > 0)
                {
                    Console.WriteLine($"Found {tests.Length} matching DNA tests:");
                    foreach (var test in tests)
                    {
                        Console.WriteLine($"  {test}");
                    }
                }
                else
                {
                    Console.WriteLine("No matching DNA tests found.");
                }
            }
            finally
            {
                factory.Close();
            }
        }

        // Loci Operations
        static void GetAllLoci()
        {
            Console.WriteLine("\n--- Getting All Loci ---");

            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1000000; // 1MB
            binding.ReaderQuotas.MaxArrayLength = 1000000;
            binding.ReaderQuotas.MaxStringContentLength = 1000000;

            var endpoint = new EndpointAddress(LociServiceUrl);
            var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                Console.WriteLine("Calling GetLoci via SOAP...");
                var loci = client.GetLoci();

                if (loci != null && loci.Length > 0)
                {
                    Console.WriteLine($"Found {loci.Length} loci:");
                    foreach (var locus in loci)
                    {
                        Console.WriteLine($"  {locus}");
                        if (!string.IsNullOrEmpty(locus.Description))
                        {
                            Console.WriteLine($"    Description: {locus.Description}");
                        }
                        if (locus.CreatedAt.HasValue)
                        {
                            Console.WriteLine($"    Created: {locus.CreatedAt:yyyy-MM-dd HH:mm:ss}");
                        }
                        Console.WriteLine();
                    }
                }
                else if (loci != null && loci.Length == 0)
                {
                    Console.WriteLine("Service returned empty array - no loci found.");
                }
                else
                {
                    Console.WriteLine("Service returned null - this might indicate a service issue.");
                }
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                Console.WriteLine($"SOAP Fault: {ex.Detail?.Message ?? ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.Detail?.StackTrace}");
            }
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine($"SOAP Fault: {ex.Message}");
                if (ex.Code != null)
                {
                    Console.WriteLine($"Fault Code: {ex.Code.Name}");
                }
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                Console.WriteLine($"Communication Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.GetType().Name}");
                Console.WriteLine($"Message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            finally
            {
                try
                {
                    if (client is System.ServiceModel.ICommunicationObject commObj)
                    {
                        if (commObj.State == System.ServiceModel.CommunicationState.Faulted)
                            commObj.Abort();
                        else
                            commObj.Close();
                    }
                    factory.Close();
                }
                catch
                {
                    factory.Abort();
                }
            }
        }

        static void CreateSampleLocus()
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
            var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var result = client.CreateLocus(locus);

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

        static void GetLocusById()
        {
            Console.WriteLine("\n--- Get Locus by ID ---");
            Console.Write("Enter Locus ID: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(LociServiceUrl);
                var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
                var client = factory.CreateChannel();

                try
                {
                    var locus = client.GetLocusById(id);

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

        static void GetLocusByName()
        {
            Console.WriteLine("\n--- Get Locus by Name ---");
            Console.Write("Enter Locus Name: ");
            var name = Console.ReadLine();

            if (!string.IsNullOrEmpty(name))
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(LociServiceUrl);
                var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
                var client = factory.CreateChannel();

                try
                {
                    var locus = client.GetLocusByName(name);

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
                        Console.WriteLine($"Locus with name '{name}' not found.");
                    }
                }
                finally
                {
                    factory.Close();
                }
            }
            else
            {
                Console.WriteLine("Name cannot be empty.");
            }
        }

        static void UpdateLocus()
        {
            Console.WriteLine("\n--- Update Locus ---");
            Console.Write("Enter Locus ID to update: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(LociServiceUrl);
                var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
                var client = factory.CreateChannel();

                try
                {
                    var existingLocus = client.GetLocusById(id);
                    if (existingLocus == null)
                    {
                        Console.WriteLine($"Locus with ID {id} not found.");
                        return;
                    }

                    Console.WriteLine($"Current locus: {existingLocus}");
                    Console.Write("Enter new description (or press Enter to keep current): ");
                    var newDescription = Console.ReadLine();

                    if (!string.IsNullOrEmpty(newDescription))
                    {
                        existingLocus.Description = newDescription;
                    }

                    var result = client.UpdateLocus(id, existingLocus);
                    Console.WriteLine($"Updated locus: {result}");
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

        static void DeleteLocus()
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
                    var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
                    var client = factory.CreateChannel();

                    try
                    {
                        var result = client.DeleteLocus(id);

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

        static void SearchLoci()
        {
            Console.WriteLine("\n--- Search Loci ---");
            Console.Write("Enter locus name (or press Enter for all): ");
            var name = Console.ReadLine();

            Console.Write("Search for CODIS loci only? (y/n): ");
            var codisInput = Console.ReadLine()?.ToLower();
            bool isCodis = codisInput == "y" || codisInput == "yes";

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(LociServiceUrl);
            var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var loci = client.SearchLoci(name ?? "", isCodis);

                if (loci != null && loci.Length > 0)
                {
                    Console.WriteLine($"Found {loci.Length} matching loci:");
                    foreach (var locus in loci)
                    {
                        Console.WriteLine($"  {locus}");
                    }
                }
                else
                {
                    Console.WriteLine("No matching loci found.");
                }
            }
            finally
            {
                factory.Close();
            }
        }

        static void GetCodisLoci()
        {
            Console.WriteLine("\n--- Getting CODIS Loci ---");

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(LociServiceUrl);
            var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                var loci = client.GetCodisLoci();

                if (loci != null && loci.Length > 0)
                {
                    Console.WriteLine($"Found {loci.Length} CODIS loci:");
                    foreach (var locus in loci)
                    {
                        Console.WriteLine($"  {locus}");
                    }
                }
                else
                {
                    Console.WriteLine("No CODIS loci found.");
                }
            }
            finally
            {
                factory.Close();
            }
        }
    }
}