using System.Collections.Generic;
using Gameplay.Cameras;
using Gameplay.Clusters;
using UnityEngine;

namespace Gameplay.Placer
{
    public class ClusterPlacer : IClusterPlacer
    {
        private readonly Transform _moveParent;
        private readonly IClustersInitialContainer _clustersStartContainer;
        
        private List<IClusterContainer> _clustersContainers;
        private List<Cluster> _clusters;

        public ClusterPlacer(
            Transform moveParent, 
            IClustersInitialContainer clustersStartContainer, 
            List<IClusterContainer> clustersContainers, 
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
            IClusterContainer clusterContainer = GetClosestClusterContainer(cluster, out float distanceToContainer);

            if (distanceToContainer <= GetDistanceToStartContainer(cluster) &&
                clusterContainer.FreeSize >= cluster.ClusterLength)
            {
                clusterContainer.AddCluster(cluster);
            }
            else
            {
                cluster.transform.SetParent(_clustersStartContainer.Container);
            }
        }

        private IClusterContainer GetClosestClusterContainer(Cluster cluster, out float distance)
        {
            distance = float.MaxValue;
            IClusterContainer closestClusterContainer = null;
            
            foreach (IClusterContainer container in _clustersContainers)
            {
                float distanceToContainer = Vector2.Distance(cluster.transform.position, container.gameObject.transform.position);
                
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
            return Vector2.Distance(cluster.transform.position, _clustersStartContainer.Center.position);
        }
    }
}