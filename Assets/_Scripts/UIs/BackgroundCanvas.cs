using System;
using UnityEngine;

public class BackgroundCanvas : MonoBehaviour {
    [SerializeField] private GameObject PlayingArea, leftPanel, rightPanel;

    private void Awake()
    {
        if (PlayingArea.TryGetComponent(out Renderer playingRenderer))
        {
            Bounds playingBound = playingRenderer.bounds;
            Camera mainCamera = Camera.main;

            Vector3 playingMinOnScreen = mainCamera.WorldToScreenPoint(playingBound.min);
            Vector3 playingMaxOnScreen = mainCamera.WorldToScreenPoint(playingBound.max);

            float playingWidthOnScreen = playingMaxOnScreen.x - playingMinOnScreen.x;

            UpdateLocalPos(leftPanel, (localPos) =>
            {

                localPos.x = -1 * (playingWidthOnScreen / 2);
                return localPos;
            });
            UpdateLocalPos(rightPanel, (localPos) =>
            {
                localPos.x = 1 * (playingWidthOnScreen / 2);
                return localPos;
            });
        }
    }

    private void UpdateLocalPos(GameObject obj, Func<Vector3, Vector3> updateAction)
    {
        Vector3 localPos = obj.transform.localPosition;
        localPos = updateAction(localPos);
        obj.transform.localPosition = localPos;
    }
}
