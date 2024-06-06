namespace GitBinaryTree;

using System.IO;
using System.Runtime.Serialization.Json;

public static class Persistence
{
    public static void SaveToFile<T>(T obj, string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            if (directory != null) Directory.CreateDirectory(directory);
        }

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        using (MemoryStream memoryStream = new MemoryStream())
        {
            serializer.WriteObject(memoryStream, obj);
            File.WriteAllBytes(filePath, memoryStream.ToArray());
        }
    }
    
    public static T? LoadFromFile<T>(string filePath)
    {
        if (!File.Exists(filePath)) return default(T);

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        byte[] jsonBytes = File.ReadAllBytes(filePath);
        using (MemoryStream memoryStream = new MemoryStream(jsonBytes))
        {
            object? deserializedObject = serializer.ReadObject(memoryStream);
            return deserializedObject is T result ? result : default(T);
        }
    }
    
    public static void CopyFile(string sourceFilePath, string destinationFilePath)
    {

        if (!File.Exists(sourceFilePath))
        {
            Console.WriteLine($"Source file not found: {sourceFilePath}");
            throw new FileNotFoundException($"Source file not found: {sourceFilePath}");
        }

        var destinationDirectory = Path.GetDirectoryName(destinationFilePath);
        if (!Directory.Exists(destinationDirectory))
        {
            if (destinationDirectory != null) Directory.CreateDirectory(destinationDirectory);
        }

        File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
    }


    
    public static void CopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        if (!Directory.Exists(sourceDirectory))
        {
            Console.WriteLine($"Source directory not found: {sourceDirectory}");
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDirectory}");
        }

        if (!Directory.Exists(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
        }

        foreach (string filePath in Directory.GetFiles(sourceDirectory))
        {
            string fileName = Path.GetFileName(filePath);
            string destinationFilePath = Path.Combine(destinationDirectory, fileName);
            CopyFile(filePath, destinationFilePath);
        }

        foreach (string subDirectory in Directory.GetDirectories(sourceDirectory))
        {
            string subDirectoryName = Path.GetFileName(subDirectory);
            string destinationSubDirectory = Path.Combine(destinationDirectory, subDirectoryName);
            CopyDirectory(subDirectory, destinationSubDirectory);
        }
    }
    
    public static void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
    {
        if (!Directory.Exists(sourceDirectoryPath))
        {
            Console.WriteLine($"Source directory not found: {sourceDirectoryPath}");
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDirectoryPath}");
        }

        if (!Directory.Exists(destinationDirectoryPath))
        {
            Directory.CreateDirectory(destinationDirectoryPath);
        }
        
        DirectoryInfo dir = new DirectoryInfo(sourceDirectoryPath);
        DirectoryInfo[] dirs = dir.GetDirectories();
        FileInfo[] files = dir.GetFiles();
        
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destinationDirectoryPath, file.Name);
            file.MoveTo(temppath);
        }

        foreach (DirectoryInfo subdir in dirs)
        {
            string temppath = Path.Combine(destinationDirectoryPath, subdir.Name);
            MoveDirectory(subdir.FullName, temppath);
        }

        Directory.Delete(sourceDirectoryPath, true);
    }
    
    public static List<string> GetFilesRecursive(string directory)
    {
        var files = new List<string>();

        foreach (var file in Directory.GetFiles(directory))
        {
            files.Add(file);
        }

        foreach (var dir in Directory.GetDirectories(directory))
        {
            files.AddRange(GetFilesRecursive(dir));
        }

        return files;
    }

    public static bool AreFilesDifferent(string filePath1, string filePath2)
    {
        if (!File.Exists(filePath1) || !File.Exists(filePath2))
        {
            throw new FileNotFoundException("One or both files do not exist.");
        }

        byte[] file1Bytes = File.ReadAllBytes(filePath1);
        byte[] file2Bytes = File.ReadAllBytes(filePath2);

        if (file1Bytes.Length != file2Bytes.Length)
        {
            return true;
        }

        for (int i = 0; i < file1Bytes.Length; i++)
        {
            if (file1Bytes[i] != file2Bytes[i])
            {
                return true;
            }
        }
        return false;
    }
}