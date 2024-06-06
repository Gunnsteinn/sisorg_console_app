namespace GitBinaryTree;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class Commit
{
    public string Message { get; set; }
    public string Branch { get; set; }
    public string HashCode { get; set; }
    public DateTime Date { get; set; }
    public List<string> Files { get; set; }

    public Commit()
    {
        Message = "";
        Branch = "master";
        Date = DateTime.Now;
        Files = new List<string>();
        HashCode = ComputeHash();
    }

    public Commit(string message, string branch, List<string> files)
    {
        Message = message;
        Branch = branch;
        Date = DateTime.Now;
        Files = new List<string>(files);
        HashCode = ComputeHash();
    }

    private string ComputeHash()
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            string input = $"{Message}{Date}{string.Join(",", Files)}";
            byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }

    public override string ToString()
    {
        return $"Commit: {Message}, Hash: {HashCode}, Date: {Date}, Files: {string.Join(", ", Files)}";
    }
}