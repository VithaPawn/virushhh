using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour {
    public IObjectPool<Bullet> ParentBulletPool {  get; set; }

    public void Show() { gameObject.SetActive(true); }

    public void Hide() { gameObject.SetActive(false); }
    public void Deactivate(float fadeTime)
    {
        StartCoroutine(DeactivateCoroutine(fadeTime));
    }

    public IEnumerator DeactivateCoroutine(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);
        ParentBulletPool.Release(this);
    }
}
