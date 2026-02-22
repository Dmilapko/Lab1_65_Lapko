using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;

namespace Lab1_65_Lapko
{
    internal class Program
    {
        private static AcademicService _service = new AcademicService();

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

                    case "a":
                    case "add":
                        HandleAdd(parts);
                        break;

                    case "u":
                    case "update":
                        HandleUpdate(parts);
                        break;

                    case "d":
                    case "delete":
                        HandleDelete(parts);
                        break;

                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        break;
                }
            }
        }

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
            Console.WriteLine("s | search [entity?] [query?] [DESC|ASC=field]");
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
                else
                {
                    Console.WriteLine($"Invalid parameter format: {parts[i]}");
                    Console.WriteLine("Expected format: key=value");
                    return new List<KeyValuePair<string, string>>();
                }
            }
            return parameters;
        }

        static void HandleSearch(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Usage: search [subject|session] [query?] [DESC|ASC=field]");
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
            bool hasQuery = parts.Length > 2 && !(parts[2].StartsWith("DESC=") || parts[2].StartsWith("ASC="));
            if (parts.Length > 2 && hasQuery)
            {
                string query = parts[2];
                results = results.Where(r => GetObjectValues(r).Any(v => v.Contains(query)));
            }
            if (parts.Length != 2 && (parts.Length > 3 || !hasQuery))
            {
                //sort by value
                var parameters = ParseParameters(parts, hasQuery ? 3 : 2);
                if (parameters.Count == 0)
                {
                    Console.WriteLine("Expected DESC=name or ASC=name for sorting");
                    return;
                }
                var sortParam = parameters[0];
                if (sortParam.Key.StartsWith("DESC"))
                {
                    string propName = sortParam.Value;
                    results = results.OrderByDescending(r => r.GetType().GetProperty(propName)?.GetValue(r)?.ToString());
                }
                else if (sortParam.Key.StartsWith("ASC"))
                {
                    string propName = sortParam.Value;
                    results = results.OrderBy(r => r.GetType().GetProperty(propName)?.GetValue(r)?.ToString());
                }
            }
            PrintPythonStyle(results);
        }

        static void FillObjectFromParameters<T>(T obj, List<KeyValuePair<string, string>> parameters)
        {
            if (obj == null) return;

            foreach (var param in parameters)
            {
                var prop = obj.GetType().GetProperty(param.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (prop != null)
                {
                    try
                    {
                        string rawValue = param.Value;
                        Type targetType = prop.PropertyType;

                        // Get the underlying type e.g. int? -> int
                        Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                        object convertedValue = null;

                        if (!string.IsNullOrEmpty(rawValue))
                        {
                            // 1. Try to find a constructor that takes a single string argument
                            var constructor = underlyingType.GetConstructor(new[] { typeof(string) });

                            if (constructor != null)
                            {
                                convertedValue = constructor.Invoke(new object[] { rawValue });
                            }
                            // 2. Fallback: Handle Enums (they don't have constructors)
                            else if (underlyingType.IsEnum)
                            {
                                convertedValue = Enum.Parse(underlyingType, rawValue, true);
                            }
                            // 3. Fallback: Use TypeConverter (handles primitives like int, bool, and Guid)
                            else
                            {
                                var converter = TypeDescriptor.GetConverter(underlyingType);
                                if (converter != null && converter.CanConvertFrom(typeof(string)))
                                {
                                    convertedValue = converter.ConvertFromString(rawValue);
                                }
                            }
                        }

                        prop.SetValue(obj, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to set {param.Key}: {ex.InnerException?.Message ?? ex.Message}");
                    }
                }
            }
        }

        static void HandleAdd(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("Usage: add [subject|session] [parameters]");
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
            if (parameters.Count == 0)
                return;
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
            if (parts.Length < 4)
            {
                Console.WriteLine("Usage: update [subject|session] [id] [parameters]");
                return;
            }

            string entity = parts[1].ToLower();
            if (Guid.TryParse(parts[2], out Guid id))
            {
                var parameters = ParseParameters(parts, 3);
                if (parameters.Count == 0)
                    return;
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
    }
}
