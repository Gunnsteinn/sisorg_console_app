namespace GitBinaryTree;

class Program
{
    static void Main(string[] args)
    {
        GitSimulator gitSimulator = new GitSimulator();
        Console.WriteLine("Welcome to Git Simulation");
        GitSimulator.Help();

        while (true)
        {
            Console.Write("git-sisorg> ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No input received. Please enter a valid command.");
                continue;
            }

            var command = input.Split(' ')[0];
            var arguments = input.Substring(command.Length).Trim();

            try
            {
                switch (command)
                {
                    case "status":
                        //git-sisorg> status
                        if (!string.IsNullOrEmpty(arguments))
                        {
                            Console.WriteLine("Invalid usage of 'status' command.");
                            break;
                        }

                        gitSimulator.Status();
                        break;

                    case "init":
                        //git-sisorg> init
                        if (!string.IsNullOrEmpty(arguments))
                        {
                            Console.WriteLine("Invalid usage of 'init' command.");
                            break;
                        }

                        gitSimulator.Init();
                        break;

                    case "add":
                        //git-sisorg> add README.md
                        if (string.IsNullOrEmpty(arguments))
                        {
                            Console.WriteLine(
                                "Invalid usage of 'add' command. Expected: 'add <filename>'.");
                            break;
                        }

                        gitSimulator.Add(arguments);
                        break;

                    case "commit":
                        //git-sisorg> commit "Any words"
                        if (string.IsNullOrEmpty(arguments) || !arguments.StartsWith("\"") ||
                            !arguments.EndsWith("\""))
                        {
                            Console.WriteLine(
                                "Invalid usage of 'commit' command. Expected: 'commit \"<message>\"'.");
                            break;
                        }

                        gitSimulator.Commit(arguments);
                        break;

                    case "push":
                        //git-sisorg> push
                        if (!string.IsNullOrEmpty(arguments))
                        {
                            Console.WriteLine("Invalid usage of 'push' command.");
                            break;
                        }

                        gitSimulator.Push();
                        break;

                    case "log":
                        //git-sisorg> log
                        if (!string.IsNullOrEmpty(arguments))
                        {
                            Console.WriteLine("Invalid usage of 'log' command.");
                            break;
                        }

                        gitSimulator.Log();
                        break;

                    case "branch":
                        //git-sisorg> branch list
                        //git-sisorg> branch <branchName>
                        if (string.IsNullOrEmpty(arguments))
                        {
                            Console.WriteLine(
                                "Invalid usage of 'branch' command. Expected: 'branch list' or 'branch <branchName>'.");
                            break;
                        }

                        if (arguments == "list")
                            gitSimulator.ListBranches();
                        else
                            gitSimulator.CreateBranch(arguments);
                        break;

                    case "help":
                        //git-sisorg> help
                        if (!string.IsNullOrEmpty(arguments))
                        {
                            Console.WriteLine("Invalid usage of 'help' command.");
                            break;
                        }

                        GitSimulator.Help();
                        break;

                    case "exit":
                        //git-sisorg> exit
                        if (!string.IsNullOrEmpty(arguments))
                        {
                            Console.WriteLine("Invalid usage of 'exit' command.");
                            break;
                        }

                        gitSimulator.PersistTree();
                        return;

                    default:
                        Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}