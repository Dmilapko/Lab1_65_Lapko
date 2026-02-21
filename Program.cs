namespace Lab1_65_Lapko
{
    internal class Program
    {
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
            Console.WriteLine("search [entity?] [query?]");
            Console.WriteLine("add [entity] [parameters]");
            Console.WriteLine("update [entity] [id] [parameters]");
            Console.WriteLine("delete [entity] [id]");
        }

        static void Main(string[] args)
        {
            Hello();
            var service = new AcademicService();
            while (true)
            {
                Console.Write("#");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                switch (input.Split(' ')[0])
                {
                    case "exit":
                    case "quit":
                        return;

                    case "h":
                    case "help":
                        PrintHelp();
                        break;
                }
            }
        }
    }
}
