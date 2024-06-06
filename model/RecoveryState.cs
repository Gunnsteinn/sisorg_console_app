namespace GitBinaryTree;

using System.Runtime.Serialization;

[DataContract]
public class RecoveryState
{
    [DataMember] public List<string> stagingArea { get; set; }
    [DataMember] public Dictionary<string, Commit> commits { get; set; }
    [DataMember] public TreeNode currentCommit { get; set; }
    [DataMember] public TreeNode rootNode { get; set; }
    [DataMember] public Branch branch { get; set; }
    [DataMember] public Dictionary<string, Commit> remoteCommits { get; set; }
    
    public RecoveryState(List<string> stagingArea, Dictionary<string, Commit> commits, TreeNode currentCommit, TreeNode rootNode, Branch branch, Dictionary<string, Commit> remoteCommits)
    {
        this.stagingArea = stagingArea;
        this.commits = commits;
        this.currentCommit = currentCommit;
        this.rootNode = rootNode;
        this.branch = branch;
        this.remoteCommits = remoteCommits;
    }


}