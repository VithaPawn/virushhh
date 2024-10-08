using UnityEngine;

public static class MovementUtilities {
    public static Vector3 LimitPositionInsideArea(GameObject area, GameObject obj, Vector3 pos)
    {
        Vector3 limitedPos = pos;
        Renderer areaRenderer = area.GetComponent<Renderer>();
        Renderer objRenderer = obj.GetComponent<Renderer>();

        Bounds areaBounds = areaRenderer ? areaRenderer.bounds : new Bounds(Vector3.zero, Vector3.zero);
        float objWidth = objRenderer ? objRenderer.bounds.size.x : 0;
        float objHeight = objRenderer ? objRenderer.bounds.size.y : 0;

        limitedPos.x = Mathf.Clamp(limitedPos.x, areaBounds.min.x + objWidth, areaBounds.max.x - objWidth);
        limitedPos.y = Mathf.Clamp(limitedPos.y, areaBounds.min.y + objHeight, areaBounds.max.y - objHeight);

        return limitedPos;
    }

    public static float GetObjectWidth(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        return objRenderer ? objRenderer.bounds.size.x : 0;
    }

}
