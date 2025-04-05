using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Clusters
{
    public class WordsContainer : MonoBehaviour
    {
        [SerializeField, Range(1, 50)] private int _containerSize;
        
        [field: SerializeField] public Transform Container { get; private set; }

        private List<Cluster> _clusters = new();
        private int _filledSize;

        public IReadOnlyList<Cluster> Clusters => _clusters;

        public int FreeSize
        {
            get
            {
                _filledSize = 0;
                _clusters.ForEach(x => _filledSize += x.ClusterLength);
                return _filledSize;
            }
        }

        public void AddCluster(Cluster cluster)
        {
            if (!_clusters.Contains(cluster))
                _clusters.Add(cluster);
        }
        
        public void RemoveCluster(Cluster cluster)
        {
            if (_clusters.Contains(cluster))
                _clusters.Remove(cluster);
        }
    }
}