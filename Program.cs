using System.IO;

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

                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        break;
                }
            }
        }
    }
}
