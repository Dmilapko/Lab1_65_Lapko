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

        static void PrintPythonStyle<T>(IEnumerable<T> items)
        {
            var list = items.ToList();

            foreach (var item in list)
            {
                var type = typeof(T).Name;
                var props = typeof(T).GetProperties();
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

            if (entity == "subject")
            {
                var results = _service.GetAllSubjects();
                PrintPythonStyle(results);
            }
            else if (entity == "session")
            {
                var results = _service.GetAllSessions();
                PrintPythonStyle(results);
            }
            else
            {
                Console.WriteLine($"Unknown entity: {entity}");
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

                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        break;
                }
            }
        }
    }
}
