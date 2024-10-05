using UnityEngine;
using UnityEngine.Pool;

public class GhostingEffectManager : MonoBehaviour {
    [SerializeField] private ObjectBase shadowPrefab;

    private IObjectPool<ObjectBase> shadowPool;

    private void Awake()
    {
        //shadowPool = new ObjectPool<ObjectBase>(CreateShadow, OnGetShadowFromPool, OnRealeaseShadowToPool, OnDestroyPoolObject,, 3, 7);
    }

    //private ObjectBase CreateShadow()
    //{

    //}
}
