using Core;
using Lean.Pool;
using UnityEngine;

namespace Pool
{
    public class LeanPoolWrapper : IGamePool
    {
        public T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return LeanPool.Spawn(prefab, parent);
        }

        public void Despawn<T>(T instance) where T : Component
        {
            LeanPool.Despawn(instance);
        }
    }
    
    public interface IGamePool
    {
        T Spawn<T>(T prefab, Transform parent) where T : Component;
        void Despawn<T>(T instance) where T : Component;
    }
}