using UnityEngine;

namespace Gameplay.Clusters
{
    public interface IClusterContainer
    {
        GameObject gameObject { get; }
        int FreeSize { get; }
        void AddCluster(Cluster cluster);
        void RemoveCluster(Cluster cluster);
        string GetWordFromClusters();
    }
}