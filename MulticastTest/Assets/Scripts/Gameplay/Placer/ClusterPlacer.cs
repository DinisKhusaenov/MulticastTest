using System.Collections.Generic;
using Gameplay.Clusters;
using UnityEngine;

namespace Gameplay.Placer
{
    public class ClusterPlacer : IClusterPlacer
    {
        private Transform _moveParent;
        private Transform _clustersStartContainer;
        private List<ClustersClusterContainer> _clustersContainers;
        private List<Cluster> _clusters;

        public ClusterPlacer(
            Transform moveParent, 
            Transform clustersStartContainer, 
            List<ClustersClusterContainer> clustersContainers, 
            List<Cluster> clusters)
        {
            _moveParent = moveParent;
            _clustersStartContainer = clustersStartContainer;
            _clustersContainers = clustersContainers;
            _clusters = clusters;

            AddEvents();
        }
        
        public void Dispose()
        {
            foreach (Cluster cluster in _clusters)
            {
                cluster.OnDragBegun -= OnClusterDragBegun;
                cluster.OnDragEnded -= OnClusterDragEnded;
            }
        }

        private void AddEvents()
        {
            foreach (Cluster cluster in _clusters)
            {
                cluster.OnDragBegun += OnClusterDragBegun;
                cluster.OnDragEnded += OnClusterDragEnded;
            }
        }

        private void OnClusterDragBegun(Cluster cluster)
        {
            cluster.transform.SetParent(_moveParent);
            cluster.CurrentClusterContainer?.RemoveCluster(cluster);
        }

        private void OnClusterDragEnded(Cluster cluster)
        {
            float distanceToContainer = float.MaxValue;
            ClustersClusterContainer clusterContainer = GetClosestClusterContainer(cluster, ref distanceToContainer);

            if (distanceToContainer <= GetDistanceToStartContainer(cluster) &&
                clusterContainer.FreeSize >= cluster.ClusterLength)
            {
                clusterContainer.AddCluster(cluster);
            }
            else
            {
                cluster.transform.SetParent(_clustersStartContainer);
            }
        }

        private ClustersClusterContainer GetClosestClusterContainer(Cluster cluster, ref float distance)
        {
            ClustersClusterContainer closestClusterContainer = null;
            
            foreach (ClustersClusterContainer container in _clustersContainers)
            {
                float distanceToContainer = Vector2.Distance(cluster.transform.position, container.transform.position);
                if (distance > distanceToContainer)
                {
                    distance = distanceToContainer;
                    closestClusterContainer = container;
                }
            }

            return closestClusterContainer;
        }

        private float GetDistanceToStartContainer(Cluster cluster)
        {
            return Vector2.Distance(cluster.transform.position, _clustersStartContainer.position);
        }
    }
}