using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Gameplay.Clusters
{
    public class ClustersClusterContainer : MonoBehaviour, IClusterContainer
    {
        [SerializeField, Range(1, 50)] private int _containerSize;
        
        [field: SerializeField] public Transform Container { get; private set; }

        private List<Cluster> _clusters = new();
        private int _filledSize;

        public int FreeSize
        {
            get
            {
                _filledSize = 0;
                _clusters.ForEach(x => _filledSize += x.ClusterLength);
                return _containerSize - _filledSize;
            }
        }

        public void AddCluster(Cluster cluster)
        {
            if (!_clusters.Contains(cluster))
            {
                _clusters.Add(cluster);
                cluster.transform.SetParent(Container);
                cluster.SetContainer(this);
            }
        }
        
        public void RemoveCluster(Cluster cluster)
        {
            if (_clusters.Contains(cluster))
            {
                _clusters.Remove(cluster);
                cluster.SetContainer(null);
            }
        }

        public string GetWordFromClusters()
        {
            StringBuilder word = new StringBuilder();
            foreach (var cluster in _clusters)
            {
                word.Append(cluster.Letters);
            }

            return word.ToString();
        }
    }
}