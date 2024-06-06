namespace GitBinaryTree;

using System.Runtime.Serialization;

[DataContract]
public class Branch
{
    [DataMember] public string CurrentBranch { get; set; }
    [DataMember] public List<string> List { get; set; }
    public Branch( List<string> list, string currentBranch)
    {
        CurrentBranch = currentBranch;
        List = list;
    }
}