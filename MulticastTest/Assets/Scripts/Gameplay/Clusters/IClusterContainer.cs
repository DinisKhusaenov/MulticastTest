namespace Gameplay.Clusters
{
    public interface IClusterContainer
    {
        void AddCluster(Cluster cluster);
        void RemoveCluster(Cluster cluster);
        string GetWordFromClusters();
    }
}