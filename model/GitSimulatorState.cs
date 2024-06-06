namespace GitBinaryTree;

using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class GitSimulatorState
{
    
    [DataMember] public List<string> StagingArea { get; set; }
    [DataMember] public Dictionary<string, Commit> Commits { get; set; }
    [DataMember] public Dictionary<string, Commit> RemoteCommits { get; set; }
    public GitSimulatorState(List<string> stagingArea, Dictionary<string, Commit> commits, Dictionary<string, Commit> remoteCommits)
    {
        StagingArea = stagingArea;
        Commits = commits;
        RemoteCommits = remoteCommits;
    }
}