using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class MovementUtilities {
    public static Vector3 LimitPositionInsideArea(GameObject area, GameObject obj, Vector3 pos)
    {
        Vector3 limitedPos = pos;
        Bounds areaBounds = GetObjectBounds(area);
        //Get object width/height
        Renderer objRenderer = obj.GetComponent<Renderer>();
        float objWidth = objRenderer ? objRenderer.bounds.size.x : 0;
        float objHeight = objRenderer ? objRenderer.bounds.size.y : 0;
        //Limit position inside area
        limitedPos.x = Mathf.Clamp(limitedPos.x, areaBounds.min.x + objWidth / 2, areaBounds.max.x - objWidth / 2);
        limitedPos.y = Mathf.Clamp(limitedPos.y, areaBounds.min.y + objHeight / 2, areaBounds.max.y - objHeight / 2);
        return limitedPos;
    }


    public static Vector3 GenerateRandomVectorInsideArea(GameObject area)
    {
        Bounds areaBounds = GetObjectBounds(area);
        Vector3 randomVector = Vector3.zero;
        randomVector.x = Random.Range(areaBounds.min.x, areaBounds.max.x);
        randomVector.y = Random.Range(areaBounds.min.y, areaBounds.max.y);
        return randomVector;
    }

    private static Bounds GetObjectBounds(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        return objRenderer ? objRenderer.bounds : new Bounds(Vector3.zero, Vector3.zero);
    }

    public static Quaternion Rotate2dByTargetPosition(Vector3 targetPos, Vector3 ownerPos)
    {
        Vector3 lookingVector = (targetPos - ownerPos).normalized;
        float angle = Mathf.Atan2(lookingVector.y, lookingVector.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle);
    }

}
