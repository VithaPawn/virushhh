using UnityEngine;
using UnityEngine.Pool;

public class GhostingEffectManager : MonoBehaviour {
    [SerializeField] private VirusShadow shadowPrefab;

    private IObjectPool<VirusShadow> shadowPool;

    private void Awake()
    {
        shadowPool = new ObjectPool<VirusShadow>(CreateShadow, OnGetShadowFromPool,
            OnRealeaseShadowToPool, OnDestroyShadow, defaultCapacity: 3, maxSize: 7);
    }

    #region ObjectPooling
    private VirusShadow CreateShadow()
    {
        VirusShadow shadowObj = Instantiate(shadowPrefab);
        shadowObj.ParentShadowPool = shadowPool;
        return shadowObj;
    }

    private void OnGetShadowFromPool(VirusShadow shadow)
    {
        shadow.Show();
    }

    private void OnRealeaseShadowToPool(VirusShadow shadow)
    {
        shadow.Hide();
    }

    private void OnDestroyShadow(VirusShadow shadow)
    {
        Destroy(shadow.gameObject);
    }
    #endregion ObjectPooling

    public void GenerateGhost(Vector3 shadowPos, float shadowColorAlpha, float fadeTime)
    {
        VirusShadow shadow = shadowPool.Get();
        if (shadow == null) return;

        shadow.SetPosition(shadowPos);
        shadow.UpdateColorAlpha(shadowColorAlpha);
        shadow.Deactivate(fadeTime);
    }
}
