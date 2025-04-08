using UnityEngine;

namespace Infrastructure.PoolService.Factory
{
    public interface IPoolFactory
    {
        TComponent CreateAsync<TComponent>(PoolObjectType type, Vector3 position, Transform parent = null) 
            where TComponent : MonoBehaviour;
    }
}