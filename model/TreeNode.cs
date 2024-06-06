namespace GitBinaryTree;

using System.Runtime.Serialization;

[DataContract]
public class TreeNode
{
    [DataMember]
    public Commit Commit { get; set; }
    [DataMember]
    public TreeNode Left { get; set; }
    [DataMember]
    public TreeNode Right { get; set; }

    public TreeNode()
    {
        Commit = new Commit();
        Left = null;
        Right = null;
    }

    public TreeNode(Commit commit)
    {
        Commit = commit;
        Left = null;
        Right = null;
    }
}