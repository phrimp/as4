using DNATesting.SoapClient.ConsoleApp.PhienNT.Models;
using System;
using System.Collections.Generic;
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
                        case "2": CreateDnaTest(); break;
                        case "3": GetDnaTestById(); break;
                        case "4": UpdateDnaTest(); break;
                        case "5": DeleteDnaTest(); break;
                        case "6": SearchDnaTests(); break;

                        // Loci
                        case "7": GetAllLoci(); break;
                        case "8": CreateLocus(); break;
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
            Console.WriteLine("2. Create New DNA Test");
            Console.WriteLine("3. Get DNA Test by ID");
            Console.WriteLine("4. Update DNA Test");
            Console.WriteLine("5. Delete DNA Test");
            Console.WriteLine("6. Search DNA Tests");
            Console.WriteLine();
            Console.WriteLine("Loci:");
            Console.WriteLine("7. Get All Loci");
            Console.WriteLine("8. Create New Locus");
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

        static void CreateDnaTest()
        {
            Console.WriteLine("\n--- Create New DNA Test ---");

            var test = new DnaTestsPhienNt();

            // Get Test Type
            Console.Write("Enter Test Type (e.g., Paternity, Maternity, Sibling, Grandparent): ");
            test.TestType = Console.ReadLine();

            // Get Conclusion
            Console.Write("Enter Conclusion (optional, press Enter to skip): ");
            var conclusion = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(conclusion))
                test.Conclusion = conclusion;

            // Get Probability of Relationship
            Console.Write("Enter Probability of Relationship (0-100, or press Enter to skip): ");
            var probInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(probInput) && decimal.TryParse(probInput, out decimal probability))
            {
                test.ProbabilityOfRelationship = probability;
            }

            // Get Relationship Index
            Console.Write("Enter Relationship Index (decimal value, or press Enter to skip): ");
            var indexInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(indexInput) && decimal.TryParse(indexInput, out decimal relationshipIndex))
            {
                test.RelationshipIndex = relationshipIndex;
            }

            // Get Completion Status
            Console.Write("Is test completed? (y/n, default: n): ");
            var completedInput = Console.ReadLine()?.ToLower();
            test.IsCompleted = completedInput == "y" || completedInput == "yes";

            // Set creation time
            test.CreatedAt = DateTime.Now;

            // Display what will be created
            Console.WriteLine("\nCreating DNA Test with the following details:");
            Console.WriteLine($"  Test Type: {test.TestType ?? "N/A"}");
            Console.WriteLine($"  Conclusion: {test.Conclusion ?? "N/A"}");
            Console.WriteLine($"  Probability: {test.ProbabilityOfRelationship?.ToString("F2") ?? "N/A"}%");
            Console.WriteLine($"  Relationship Index: {test.RelationshipIndex?.ToString("F2") ?? "N/A"}");
            Console.WriteLine($"  Completed: {test.IsCompleted}");
            Console.WriteLine($"  Created At: {test.CreatedAt:yyyy-MM-dd HH:mm:ss}");

            Console.Write("\nProceed with creation? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm != "y" && confirm != "yes")
            {
                Console.WriteLine("Creation cancelled.");
                return;
            }

            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1000000;
            binding.ReaderQuotas.MaxArrayLength = 1000000;
            binding.ReaderQuotas.MaxStringContentLength = 1000000;

            var endpoint = new EndpointAddress(DnaTestServiceUrl);
            var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                Console.WriteLine("Sending request to server...");
                var result = client.CreateDnaTest(test);

                if (result != null)
                {
                    Console.WriteLine($"\n✅ Successfully created DNA test: {result}");
                    if (!string.IsNullOrEmpty(result.Conclusion))
                    {
                        Console.WriteLine($"   Conclusion: {result.Conclusion}");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Failed to create DNA test - server returned null.");
                }
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                Console.WriteLine($"❌ SOAP Fault: {ex.Detail?.Message ?? ex.Message}");
            }
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine($"❌ SOAP Fault: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
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

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Invalid ID format.");
                return;
            }

            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1000000;
            binding.ReaderQuotas.MaxArrayLength = 1000000;
            binding.ReaderQuotas.MaxStringContentLength = 1000000;

            var endpoint = new EndpointAddress(DnaTestServiceUrl);
            var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                // First get the existing test
                Console.WriteLine("Fetching existing DNA test...");
                var existingTest = client.GetDnaTestById(id);
                if (existingTest == null)
                {
                    Console.WriteLine($"❌ DNA test with ID {id} not found.");
                    return;
                }

                Console.WriteLine($"\nCurrent DNA Test Details:");
                Console.WriteLine($"  ID: {existingTest.PhienNtid}");
                Console.WriteLine($"  Test Type: {existingTest.TestType ?? "N/A"}");
                Console.WriteLine($"  Conclusion: {existingTest.Conclusion ?? "N/A"}");
                Console.WriteLine($"  Probability: {existingTest.ProbabilityOfRelationship?.ToString("F2") ?? "N/A"}%");
                Console.WriteLine($"  Relationship Index: {existingTest.RelationshipIndex?.ToString("F2") ?? "N/A"}");
                Console.WriteLine($"  Completed: {existingTest.IsCompleted?.ToString() ?? "N/A"}");
                Console.WriteLine($"  Created: {existingTest.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}");

                Console.WriteLine("\n--- Update Fields (press Enter to keep current value) ---");

                // Update Test Type
                Console.Write($"Test Type [{existingTest.TestType ?? "N/A"}]: ");
                var newTestType = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newTestType))
                    existingTest.TestType = newTestType;

                // Update Conclusion
                Console.Write($"Conclusion [{existingTest.Conclusion ?? "N/A"}]: ");
                var newConclusion = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newConclusion))
                    existingTest.Conclusion = newConclusion;

                // Update Probability
                Console.Write($"Probability of Relationship [{existingTest.ProbabilityOfRelationship?.ToString("F2") ?? "N/A"}]: ");
                var probInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(probInput) && decimal.TryParse(probInput, out decimal probability))
                {
                    existingTest.ProbabilityOfRelationship = probability;
                }

                // Update Relationship Index
                Console.Write($"Relationship Index [{existingTest.RelationshipIndex?.ToString("F2") ?? "N/A"}]: ");
                var indexInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(indexInput) && decimal.TryParse(indexInput, out decimal relationshipIndex))
                {
                    existingTest.RelationshipIndex = relationshipIndex;
                }

                // Update Completion Status
                Console.Write($"Is completed? (y/n) [{(existingTest.IsCompleted == true ? "y" : "n")}]: ");
                var completedInput = Console.ReadLine()?.ToLower();
                if (!string.IsNullOrWhiteSpace(completedInput))
                {
                    existingTest.IsCompleted = completedInput == "y" || completedInput == "yes";
                }

                // Show preview of changes
                Console.WriteLine("\n--- Updated DNA Test Preview ---");
                Console.WriteLine($"  ID: {existingTest.PhienNtid}");
                Console.WriteLine($"  Test Type: {existingTest.TestType ?? "N/A"}");
                Console.WriteLine($"  Conclusion: {existingTest.Conclusion ?? "N/A"}");
                Console.WriteLine($"  Probability: {existingTest.ProbabilityOfRelationship?.ToString("F2") ?? "N/A"}%");
                Console.WriteLine($"  Relationship Index: {existingTest.RelationshipIndex?.ToString("F2") ?? "N/A"}");
                Console.WriteLine($"  Completed: {existingTest.IsCompleted?.ToString() ?? "N/A"}");

                Console.Write("\nSave these changes? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();
                if (confirm != "y" && confirm != "yes")
                {
                    Console.WriteLine("Update cancelled.");
                    return;
                }

                Console.WriteLine("Sending update to server...");
                var result = client.UpdateDnaTest(id, existingTest);

                if (result != null)
                {
                    Console.WriteLine($"\n✅ Successfully updated DNA test: {result}");
                }
                else
                {
                    Console.WriteLine("❌ Failed to update DNA test - server returned null.");
                }
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                Console.WriteLine($"❌ SOAP Fault: {ex.Detail?.Message ?? ex.Message}");
            }
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine($"❌ SOAP Fault: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
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

            Console.Write("Enter test type to search for (or press Enter for all types): ");
            var testType = Console.ReadLine();

            Console.WriteLine("Filter by completion status:");
            Console.WriteLine("1. All tests (completed and incomplete)");
            Console.WriteLine("2. Only completed tests");
            Console.WriteLine("3. Only incomplete tests");
            Console.Write("Choose (1-3, default: 1): ");

            var statusChoice = Console.ReadLine();
            bool? completionFilter = null;
            bool isCompleted = false;

            switch (statusChoice)
            {
                case "2":
                    completionFilter = true;
                    isCompleted = true;
                    break;
                case "3":
                    completionFilter = false;
                    isCompleted = false;
                    break;
                default:
                    Console.WriteLine("Searching all tests regardless of completion status...");
                    // For the SOAP service, we need to pick one value, so we'll do two searches
                    break;
            }

            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1000000;
            binding.ReaderQuotas.MaxArrayLength = 1000000;
            binding.ReaderQuotas.MaxStringContentLength = 1000000;

            var endpoint = new EndpointAddress(DnaTestServiceUrl);
            var factory = new ChannelFactory<IDnaTestsPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                Console.WriteLine("Searching...");

                List<DnaTestsPhienNt> allResults = new List<DnaTestsPhienNt>();

                if (completionFilter.HasValue)
                {
                    // Single search with specific completion status
                    var tests = client.SearchDnaTests(testType ?? "", isCompleted);
                    if (tests != null)
                        allResults.AddRange(tests);
                }
                else
                {
                    // Search both completed and incomplete, then combine
                    var completedTests = client.SearchDnaTests(testType ?? "", true);
                    var incompleteTests = client.SearchDnaTests(testType ?? "", false);

                    if (completedTests != null)
                        allResults.AddRange(completedTests);
                    if (incompleteTests != null)
                        allResults.AddRange(incompleteTests);
                }

                if (allResults.Count > 0)
                {
                    Console.WriteLine($"\n✅ Found {allResults.Count} matching DNA tests:");
                    Console.WriteLine("".PadRight(80, '='));

                    foreach (var test in allResults)
                    {
                        Console.WriteLine($"🧬 {test}");
                        if (!string.IsNullOrEmpty(test.Conclusion))
                        {
                            Console.WriteLine($"   Conclusion: {test.Conclusion}");
                        }
                        if (test.CreatedAt.HasValue)
                        {
                            Console.WriteLine($"   Created: {test.CreatedAt:yyyy-MM-dd HH:mm:ss}");
                        }
                        Console.WriteLine("".PadRight(80, '-'));
                    }
                }
                else
                {
                    Console.WriteLine("❌ No matching DNA tests found.");
                    Console.WriteLine("Try:");
                    Console.WriteLine("- Different test type (Paternity, Maternity, Sibling, etc.)");
                    Console.WriteLine("- Different completion status filter");
                    Console.WriteLine("- Leave test type empty to search all types");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Search failed: {ex.Message}");
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

        static void CreateLocus()
        {
            Console.WriteLine("\n--- Create New Locus ---");

            var locus = new LociPhienNt();

            // Get Locus Name
            Console.Write("Enter Locus Name (required): ");
            locus.Name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(locus.Name))
            {
                Console.WriteLine("❌ Locus name is required. Creation cancelled.");
                return;
            }

            // Get CODIS status
            Console.Write("Is this a CODIS locus? (y/n, default: n): ");
            var codisInput = Console.ReadLine()?.ToLower();
            locus.IsCodis = codisInput == "y" || codisInput == "yes";

            // Get Description
            Console.Write("Enter Description (optional, press Enter to skip): ");
            var description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description))
                locus.Description = description;

            // Get Mutation Rate
            Console.Write("Enter Mutation Rate (decimal value like 0.0001, or press Enter to skip): ");
            var rateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(rateInput) && decimal.TryParse(rateInput, out decimal mutationRate))
            {
                locus.MutationRate = mutationRate;
            }

            // Set creation time
            locus.CreatedAt = DateTime.Now;

            // Display what will be created
            Console.WriteLine("\nCreating Locus with the following details:");
            Console.WriteLine($"  Name: {locus.Name}");
            Console.WriteLine($"  CODIS: {locus.IsCodis}");
            Console.WriteLine($"  Description: {locus.Description ?? "N/A"}");
            Console.WriteLine($"  Mutation Rate: {locus.MutationRate?.ToString("F6") ?? "N/A"}");
            Console.WriteLine($"  Created At: {locus.CreatedAt:yyyy-MM-dd HH:mm:ss}");

            Console.Write("\nProceed with creation? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();
            if (confirm != "y" && confirm != "yes")
            {
                Console.WriteLine("Creation cancelled.");
                return;
            }

            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1000000;
            binding.ReaderQuotas.MaxArrayLength = 1000000;
            binding.ReaderQuotas.MaxStringContentLength = 1000000;

            var endpoint = new EndpointAddress(LociServiceUrl);
            var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                Console.WriteLine("Sending request to server...");
                var result = client.CreateLocus(locus);

                if (result != null)
                {
                    Console.WriteLine($"\n✅ Successfully created locus: {result}");
                    if (!string.IsNullOrEmpty(result.Description))
                    {
                        Console.WriteLine($"   Description: {result.Description}");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Failed to create locus - server returned null.");
                }
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                Console.WriteLine($"❌ SOAP Fault: {ex.Detail?.Message ?? ex.Message}");
            }
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine($"❌ SOAP Fault: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
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

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Invalid ID format.");
                return;
            }

            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1000000;
            binding.ReaderQuotas.MaxArrayLength = 1000000;
            binding.ReaderQuotas.MaxStringContentLength = 1000000;

            var endpoint = new EndpointAddress(LociServiceUrl);
            var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                // First get the existing locus
                Console.WriteLine("Fetching existing locus...");
                var existingLocus = client.GetLocusById(id);
                if (existingLocus == null)
                {
                    Console.WriteLine($"❌ Locus with ID {id} not found.");
                    return;
                }

                Console.WriteLine($"\nCurrent Locus Details:");
                Console.WriteLine($"  ID: {existingLocus.PhienNtid}");
                Console.WriteLine($"  Name: {existingLocus.Name ?? "N/A"}");
                Console.WriteLine($"  CODIS: {existingLocus.IsCodis?.ToString() ?? "N/A"}");
                Console.WriteLine($"  Description: {existingLocus.Description ?? "N/A"}");
                Console.WriteLine($"  Mutation Rate: {existingLocus.MutationRate?.ToString("F6") ?? "N/A"}");
                Console.WriteLine($"  Created: {existingLocus.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}");

                Console.WriteLine("\n--- Update Fields (press Enter to keep current value) ---");

                // Update Name
                Console.Write($"Name [{existingLocus.Name ?? "N/A"}]: ");
                var newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                    existingLocus.Name = newName;

                // Update CODIS status
                Console.Write($"Is CODIS? (y/n) [{(existingLocus.IsCodis == true ? "y" : "n")}]: ");
                var codisInput = Console.ReadLine()?.ToLower();
                if (!string.IsNullOrWhiteSpace(codisInput))
                {
                    existingLocus.IsCodis = codisInput == "y" || codisInput == "yes";
                }

                // Update Description
                Console.Write($"Description [{existingLocus.Description ?? "N/A"}]: ");
                var newDescription = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDescription))
                    existingLocus.Description = newDescription;

                // Update Mutation Rate
                Console.Write($"Mutation Rate [{existingLocus.MutationRate?.ToString("F6") ?? "N/A"}]: ");
                var rateInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(rateInput) && decimal.TryParse(rateInput, out decimal mutationRate))
                {
                    existingLocus.MutationRate = mutationRate;
                }

                // Show preview of changes
                Console.WriteLine("\n--- Updated Locus Preview ---");
                Console.WriteLine($"  ID: {existingLocus.PhienNtid}");
                Console.WriteLine($"  Name: {existingLocus.Name ?? "N/A"}");
                Console.WriteLine($"  CODIS: {existingLocus.IsCodis?.ToString() ?? "N/A"}");
                Console.WriteLine($"  Description: {existingLocus.Description ?? "N/A"}");
                Console.WriteLine($"  Mutation Rate: {existingLocus.MutationRate?.ToString("F6") ?? "N/A"}");

                Console.Write("\nSave these changes? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();
                if (confirm != "y" && confirm != "yes")
                {
                    Console.WriteLine("Update cancelled.");
                    return;
                }

                Console.WriteLine("Sending update to server...");
                var result = client.UpdateLocus(id, existingLocus);

                if (result != null)
                {
                    Console.WriteLine($"\n✅ Successfully updated locus: {result}");
                }
                else
                {
                    Console.WriteLine("❌ Failed to update locus - server returned null.");
                }
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                Console.WriteLine($"❌ SOAP Fault: {ex.Detail?.Message ?? ex.Message}");
            }
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine($"❌ SOAP Fault: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
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

            Console.Write("Enter locus name to search for (or press Enter for all names): ");
            var name = Console.ReadLine();

            Console.WriteLine("Filter by CODIS status:");
            Console.WriteLine("1. All loci (CODIS and non-CODIS)");
            Console.WriteLine("2. Only CODIS loci");
            Console.WriteLine("3. Only non-CODIS loci");
            Console.Write("Choose (1-3, default: 1): ");

            var codisChoice = Console.ReadLine();
            bool? codisFilter = null;
            bool isCodis = false;

            switch (codisChoice)
            {
                case "2":
                    codisFilter = true;
                    isCodis = true;
                    break;
                case "3":
                    codisFilter = false;
                    isCodis = false;
                    break;
                default:
                    Console.WriteLine("Searching all loci regardless of CODIS status...");
                    break;
            }

            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 1000000;
            binding.ReaderQuotas.MaxArrayLength = 1000000;
            binding.ReaderQuotas.MaxStringContentLength = 1000000;

            var endpoint = new EndpointAddress(LociServiceUrl);
            var factory = new ChannelFactory<ILociPhienNtSoapService>(binding, endpoint);
            var client = factory.CreateChannel();

            try
            {
                Console.WriteLine("Searching...");

                List<LociPhienNt> allResults = new List<LociPhienNt>();

                if (codisFilter.HasValue)
                {
                    // Single search with specific CODIS status
                    var loci = client.SearchLoci(name ?? "", isCodis);
                    if (loci != null)
                        allResults.AddRange(loci);
                }
                else
                {
                    // Search both CODIS and non-CODIS, then combine
                    var codisLoci = client.SearchLoci(name ?? "", true);
                    var nonCodisLoci = client.SearchLoci(name ?? "", false);

                    if (codisLoci != null)
                        allResults.AddRange(codisLoci);
                    if (nonCodisLoci != null)
                        allResults.AddRange(nonCodisLoci);
                }

                if (allResults.Count > 0)
                {
                    Console.WriteLine($"\n✅ Found {allResults.Count} matching loci:");
                    Console.WriteLine("".PadRight(80, '='));

                    foreach (var locus in allResults)
                    {
                        Console.WriteLine($"🧬 {locus}");
                        if (!string.IsNullOrEmpty(locus.Description))
                        {
                            Console.WriteLine($"   Description: {locus.Description}");
                        }
                        if (locus.CreatedAt.HasValue)
                        {
                            Console.WriteLine($"   Created: {locus.CreatedAt:yyyy-MM-dd HH:mm:ss}");
                        }
                        Console.WriteLine("".PadRight(80, '-'));
                    }
                }
                else
                {
                    Console.WriteLine("❌ No matching loci found.");
                    Console.WriteLine("Try:");
                    Console.WriteLine("- Different locus name pattern");
                    Console.WriteLine("- Different CODIS status filter");
                    Console.WriteLine("- Leave name empty to search all loci");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Search failed: {ex.Message}");
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