using UnityEngine;
using UnityEngine.Pool;

public class GhostingEffectManager : MonoBehaviour {
    [SerializeField] private GameObject virusVisual;
    [Header("Ghost Attributes")]
    [SerializeField] private VirusGhost ghostPrefab;
    [SerializeField] private float ghostAlpha;
    [SerializeField] private float fadeDuration;

    private GameObject virus;
    private IObjectPool<VirusGhost> ghostPool;

    private void Awake()
    {
        ghostPool = new ObjectPool<VirusGhost>(CreateShadow, OnGetShadowFromPool,
            OnRealeaseShadowToPool, OnDestroyShadow, defaultCapacity: 3, maxSize: 7);
    }

    #region ObjectPooling
    private VirusGhost CreateShadow()
    {
        VirusGhost shadowObj = Instantiate(ghostPrefab);
        shadowObj.ParentShadowPool = ghostPool;
        return shadowObj;
    }

    private void OnGetShadowFromPool(VirusGhost shadow)
    {
        shadow.UpdateScale(transform.localScale);
        if (virusVisual.TryGetComponent(out SpriteRenderer virusRender))
        {
            shadow.UpdateVisual(virusRender.sprite);
        }
        shadow.SetFadeDuration(fadeDuration);
        shadow.Show();
    }

    private void OnRealeaseShadowToPool(VirusGhost shadow)
    {
        shadow.Hide();
    }

    private void OnDestroyShadow(VirusGhost shadow)
    {
        Destroy(shadow.gameObject);
    }
    #endregion ObjectPooling

    public void GenerateGhost(Vector3 shadowPos, float fadeTime)
    {
        VirusGhost shadow = ghostPool.Get();
        if (shadow == null) return;

        shadow.SetPosition(shadowPos);
        shadow.UpdateColorAlpha(ghostAlpha);
        shadow.SetGhostAlpha(ghostAlpha);
        shadow.Deactivate(fadeTime);
    }
}
