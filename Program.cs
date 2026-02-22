using System.IO;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;

namespace Lab1_65_Lapko
{
    internal class Program
    {
        private static AcademicService _service = new AcademicService();

        static void Hello()
        {
            Console.WriteLine("Welcome to lab by Lapko Dmytro!");
            Console.WriteLine("help | h for help");
        }

        static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("help | h - Show this help message");
            Console.WriteLine("exit | quit - Exit the application");
            Console.WriteLine("s | search [entity?] [query?]");
            Console.WriteLine("a | add [entity] [parameters]");
            Console.WriteLine("u | update [entity] [id] [parameters]");
            Console.WriteLine("d | delete [entity] [id]");
        }

        static List<string> GetObjectValues<T>(T item)
        {
            return item.GetType().GetProperties()
                .Select(p => $"{p.GetValue(item)}")
                .ToList();
        }

        static void PrintPythonStyle<T>(IEnumerable<T> items)
        {
            var list = items.ToList();

            foreach (var item in list)
            {
                var type = item.GetType();
                var props = type.GetProperties();
                var fields = string.Join("\n    ", props.Select(p => $"'{p.Name}': '{p.GetValue(item)}'"));
                Console.WriteLine($"{type}: \n    {fields}  \n");
            }
        }

        static List<KeyValuePair<string, string>> ParseParameters(string[] parts, int startIdx)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            for (int i = startIdx; i < parts.Length; i++)
            {
                var paramParts = parts[i].Split('=');
                if (paramParts.Length == 2)
                {
                    parameters.Add(new KeyValuePair<string, string>(paramParts[0], paramParts[1]));
                }
            }
            return parameters;
        }

        static void HandleSearch(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Usage: search [subject|session] [query?]");
                return;
            }

            string entity = parts[1];
            IEnumerable<object> results;
            if (entity == "subject")
            {
                results = _service.GetAllSubjects();
            }
            else if (entity == "session")
            {
                results = _service.GetAllSessions();
            }
            else
            {
                Console.WriteLine($"Unknown entity: {entity}");
                return;
            }
            if (parts.Length > 2)
            {
                string query = parts[2];
                results = results.Where(r => GetObjectValues(r).Any(v => v.Contains(query)));
            }
            PrintPythonStyle(results);
        }

        static void FillObjectFromParameters<T>(T obj, List<KeyValuePair<string, string>> parameters)
        {
            foreach (var param in parameters)
            {
                var prop = obj.GetType().GetProperty(param.Key, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (prop != null)
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(param.Value, prop.PropertyType);
                        prop.SetValue(obj, convertedValue);
                    }
                    catch
                    {
                        Console.WriteLine($"Failed to set property {param.Key} with value {param.Value}");
                    }
                }
            }
        }

        static void HandleAdd(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Usage: add [subject|session]");
                return;
            }

            string entity = parts[1].ToLower();
            object newObj = null;

            if (entity == "subject")
            {
                newObj = new Subject();
            }
            else if (entity == "session")
            {
                newObj = new Session();
            }
            else
            {
                Console.WriteLine($"Unknown entity: {entity}");
                return;
            }
            var parameters = ParseParameters(parts, 2);
            FillObjectFromParameters(newObj, parameters);

            if (entity == "subject")
            {
                _service.AddSubject((Subject)newObj);
                Console.WriteLine("Subject added.");
            }
            else if (entity == "session")
            {
                _service.AddSession((Session)newObj);
                Console.WriteLine("Session added.");
            }
        }

        static void HandleDelete(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("Usage: delete [subject|session] [id]");
                return;
            }

            string entity = parts[1];
            if (Guid.TryParse(parts[2], out Guid id))
            {
                bool success = false;
                if (entity == "subject")
                {
                    success = _service.DeleteSubject(id);
                }
                else if (entity == "session")
                {
                    success = _service.DeleteSession(id);
                }
                else
                {
                    Console.WriteLine($"Unknown entity: {entity}");
                    return;
                }
                Console.WriteLine(success ? "Deleted successfully." : "Item not found.");
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        static void HandleUpdate(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("Usage: update [subject|session] [id]");
                return;
            }

            string entity = parts[1].ToLower();
            if (Guid.TryParse(parts[2], out Guid id))
            {
                var parameters = ParseParameters(parts, 3);
                object template = null;
                if (entity == "subject")
                {
                    template = new Subject();
                }
                else if (entity == "session")
                {
                    template = new Session();
                }
                else
                {
                    Console.WriteLine($"Unknown entity: {entity}");
                    return;
                }
                FillObjectFromParameters(template, parameters);

                if (entity == "subject")
                {
                    bool success = _service.UpdateSubject(id, (Subject)template);
                    Console.WriteLine(success ? "Updated." : "Error updating.");
                }
                else if (entity == "session")
                {
                    bool success = _service.UpdateSession(id, (Session)template);
                    Console.WriteLine(success ? "Updated." : "Error updating.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        static void Main(string[] args)
        {
            Hello();
            while (true)
            {
                Console.Write("#");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var command = parts[0];

                switch (command)
                {
                    case "exit":
                    case "quit":
                        return;

                    case "h":
                    case "help":
                        PrintHelp();
                        break;

                    case "s":
                    case "search":
                        HandleSearch(parts);
                        break;

                    case "add":
                        HandleAdd(parts);
                        break;
                    case "update":
                        HandleUpdate(parts);
                        break;
                    case "delete":
                        HandleDelete(parts);
                        break;

                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        break;
                }
            }
        }
    }
}
