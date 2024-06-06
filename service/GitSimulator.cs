namespace GitBinaryTree;

public class GitSimulator
{
    private List<string> stagingArea;
    private Dictionary<string, Commit> commits;
    private TreeNode currentCommit;
    private TreeNode rootNode;
    private Branch branch;
    private Dictionary<string, Commit> remoteCommits;
    private const string CurrentDirectory = "WorkArea";
    private const string PersistenceFile = "gitSimulatorState.json";

    public GitSimulator()
    {
        LoadState();
        branch = new Branch(["master"], "master");
        stagingArea = new List<string>();
        commits ??= new Dictionary<string, Commit>();
        remoteCommits ??= new Dictionary<string, Commit>();
    }

    public void Init()
    {
        rootNode ??= new TreeNode();
        currentCommit ??= new TreeNode();
        Console.WriteLine($"Initialization completed.");
    }

    public void Status()
    {
        string workAreaDirectory = Path.Combine(CurrentDirectory);
        string branchDirectory = Path.Combine(CurrentDirectory, ".git", "objects", branch.CurrentBranch);

        if (!Directory.Exists(workAreaDirectory))
        {
            Console.WriteLine("WorkArea directory does not exist.");
            return;
        }

        if (!Directory.Exists(branchDirectory))
        {
            Console.WriteLine($"Branch directory for {branch.CurrentBranch} does not exist.");
            return;
        }

        List<string> workAreaFiles = Persistence.GetFilesRecursive(workAreaDirectory);
        List<string> branchFiles = Persistence.GetFilesRecursive(branchDirectory);

        var newFiles = new List<string>();
        var stagingFiles = new List<string>();

        foreach (var file in workAreaFiles)
        {
            string relativePath = file.Substring(workAreaDirectory.Length + 1);
            string branchFilePath = Path.Combine(branchDirectory, relativePath);

            if (!branchFiles.Contains(branchFilePath))
            {
                if (relativePath.StartsWith(".git" + Path.DirectorySeparatorChar))
                {
                    stagingFiles.Add(relativePath.Substring(".git".Length + 1));
                }
                else
                {
                    newFiles.Add(relativePath);
                }
            }
        }

        if (newFiles.Count == 0 && stagingFiles.Count == 0)
        {
            Console.WriteLine("No changes detected.");
        }
        else
        {
            if (newFiles.Count > 0)
            {
                Console.WriteLine("New files:");
                foreach (var file in newFiles)
                {
                    Console.WriteLine($"\t{file}");
                }
            }

            if (stagingFiles.Count > 0)
            {
                Console.WriteLine("StagingArea:");
                foreach (var file in stagingFiles)
                {
                    Console.WriteLine($"\t{file}");
                }
            }
        }
    }

    public void Add(string fileName)
    {
        if (currentCommit == null)
        {
            Console.WriteLine("Initialization is required. git-sisorg> init");
            return;
        }

        var sourceFilePath = Path.Combine(CurrentDirectory, fileName);
        var destinationDirectory = Path.Combine(CurrentDirectory, ".git", "objects", currentCommit.Commit.Branch);
        var destinationFilePath = Path.Combine(destinationDirectory, fileName);

        if (!File.Exists(sourceFilePath))
        {
            Console.WriteLine($"File {sourceFilePath} does not exist.");
            return;
        }

        if (File.Exists(destinationFilePath) && !Persistence.AreFilesDifferent(sourceFilePath, destinationFilePath))
        {
            Console.WriteLine($"File {fileName} has not changed.");
            return;
        }

        if (!Directory.Exists(CurrentDirectory))
        {
            Console.WriteLine($"Source directory {CurrentDirectory} does not exist. Creating directory...");
            Directory.CreateDirectory(CurrentDirectory);
        }

        if (!Directory.Exists(destinationDirectory))
        {
            Console.WriteLine($"Destination directory {destinationDirectory} does not exist. Creating directory...");
            Directory.CreateDirectory(destinationDirectory);
        }

        Persistence.CopyFile(sourceFilePath, destinationFilePath);
        stagingArea.Add(fileName);
        Console.WriteLine($"File {fileName} added to staging area.");
        SaveState(commits);
    }

    public void Commit(string message)
    {
        if (stagingArea.Count == 0)
        {
            Console.WriteLine("Nothing to commit, working directory clean.");
            return;
        }

        var node = NodeLookup(this.rootNode, currentCommit.Commit.HashCode);
        var rootNode = node.Result;

        if (rootNode.Left != null)
        {
            rootNode = rootNode.Left;
        }

        Commit newCommit = new Commit(message, branch.CurrentBranch, stagingArea);
        rootNode.Right = new TreeNode();
        rootNode.Right.Commit = newCommit;

        currentCommit = rootNode.Right;
        commits.Add(newCommit.HashCode, newCommit);
        stagingArea.Clear();
        Console.WriteLine($"Committed with message: {message}");
        SaveState(commits);
    }

    public void Push()
    {
        if (currentCommit == null)
        {
            Console.WriteLine("Nothing to push.");
            return;
        }

        HashSet<string> remoteCommitHashes =
            new HashSet<string>(remoteCommits.Values.Select(commit => commit.HashCode));

        var commitsToPush = commits.Values
            .Where(commit => !remoteCommitHashes.Contains(commit.HashCode))
            .ToList();

        if (commitsToPush.Count == 0)
        {
            Console.WriteLine("No new commits to push.");
            return;
        }

        foreach (var commit in commitsToPush)
        {
            remoteCommits.Add(commit.HashCode, commit);
        }

        Console.WriteLine("Commits pushed to remote repository.");
        SaveState(commits);
    }

    public void Log()
    {
        if (commits.Count == 0)
        {
            Console.WriteLine("No commits found.");
            return;
        }

        var foundCommits = false;

        var filteredCommits = commits
            .Where(c => c.Value.Branch == branch.CurrentBranch)
            .OrderByDescending(c => c.Value.Date)
            .ToList();

        if (filteredCommits.Count == 0)
        {
            Console.WriteLine($"No commits found in the current branch ({branch.CurrentBranch}).");
            return;
        }

        foreach (var commit in filteredCommits)
        {
            foundCommits = true;
            var fullCommitFiles = string.Join(", ", commit.Value.Files);

            Console.WriteLine($"commit {commit.Key} in >> ({commit.Value.Branch})");
            Console.WriteLine("Author: Sisorg");
            Console.WriteLine($"Date: {commit.Value.Date}");
            Console.WriteLine();
            Console.WriteLine($"     Files: {fullCommitFiles}");
            Console.WriteLine();
        }

        if (!foundCommits)
        {
            Console.WriteLine($"No commits found in the current branch ({branch.CurrentBranch}).");
        }
    }

    public void CreateBranch(string branchName)
    {
        if (branch.List.Contains(branchName))
        {
            Console.WriteLine($"Branch {branchName} already exists.");
            return;
        }

        var node = NodeLookup(this.rootNode, currentCommit.Commit.HashCode);
        var rootNode = node.Result;

        while (rootNode != null)
        {
            if (rootNode.Commit.HashCode == currentCommit.Commit.HashCode)
            {
                rootNode.Left = new TreeNode(currentCommit.Commit);
                //currentCommit.Commit.Branch = branchName;
                currentCommit = rootNode.Left;
                break;
            }

            rootNode = rootNode.Right;
        }


        string directoryPathToMove = Path.Combine(CurrentDirectory, ".git", "objects", branch.CurrentBranch);
        string destinationDirectoryPath = Path.Combine(CurrentDirectory, ".git", "objects", branchName);
        Console.WriteLine(directoryPathToMove);
        Console.WriteLine(destinationDirectoryPath);

        if (!Directory.Exists(directoryPathToMove))
        {
            Console.WriteLine($"Source directory {CurrentDirectory} does not exist. Creating directory...");
            return;
        }

        if (!Directory.Exists(destinationDirectoryPath))
        {
            Console.WriteLine(
                $"Destination directory {destinationDirectoryPath} does not exist. Creating directory...");
            Directory.CreateDirectory(destinationDirectoryPath);
        }

        Persistence.CopyDirectory(directoryPathToMove, destinationDirectoryPath);

        branch.List.Add(branchName);
        branch.CurrentBranch = branchName;
        Console.WriteLine($"Branch {branchName} created.");
    }

    public void ListBranches()
    {
        Console.WriteLine("Available branches:");
        foreach (var value in branch.List)
        {
            Console.WriteLine(value);
        }
    }

    public static void Help()
    {
        Console.WriteLine("-----------------------------------------------------------------");
        Console.WriteLine("Available commands:");
        Console.WriteLine("init               - Initialize the application.");
        Console.WriteLine("status             - Show the information");
        Console.WriteLine("add <filename>     - Add file to staging area");
        Console.WriteLine("commit <message>   - Commit changes in staging area with message");
        Console.WriteLine("push               - Push commits to remote repository");
        Console.WriteLine("log                - Show commit history");
        Console.WriteLine("branch <name>      - Create new branch");
        Console.WriteLine("branch list        - List all branches");
        Console.WriteLine("help               - Show help");
        Console.WriteLine("exit               - Exit the application");
        Console.WriteLine("-----------------------------------------------------------------");
    }

    private async Task<TreeNode?> NodeLookup(TreeNode rootNode, string Hash)
    {
        if (rootNode == null)
        {
            return null;
        }

        if (rootNode.Commit.HashCode == Hash)
        {
            return rootNode;
        }

        if (rootNode.Left != null)
        {
            var leftResult = await NodeLookup(rootNode.Left, Hash);
            if (leftResult != null)
            {
                return leftResult;
            }
        }

        if (rootNode.Right == null) return null;
        var rightResult = await NodeLookup(rootNode.Right, Hash);
        if (rightResult != null)
        {
            return rightResult;
        }

        return null;
    }

    private void SaveState(Dictionary<string, Commit> commitListsDictionary)
    {
        var state = new GitSimulatorState(stagingArea, commitListsDictionary, remoteCommits);
        Persistence.SaveToFile(state, Path.Combine(CurrentDirectory, ".git", PersistenceFile));
    }

    private void LoadState()
    {
        var state = Persistence.LoadFromFile<RecoveryState>(Path.Combine(CurrentDirectory, ".git", "recoveryTree"));
        if (state == null) return;
        stagingArea = state.stagingArea;
        commits = state.commits;
        currentCommit = state.currentCommit;
        rootNode = state.rootNode;
        branch = state.branch;
        remoteCommits = state.remoteCommits;
    }

    public void PersistTree()
    {
        var state = new RecoveryState(stagingArea, commits, currentCommit, rootNode, branch, remoteCommits);
        Persistence.SaveToFile(state, Path.Combine(CurrentDirectory, ".git", "recoveryTree"));
    }
}