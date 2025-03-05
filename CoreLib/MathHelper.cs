using UnityEngine;

public static class MathHelper
{
    public static void LookRotation2D(GameObject mainObject, Vector2 direction)
    {
        Vector3 vectorToTarget = direction - mainObject.GetComponent<RectTransform>().anchoredPosition;
        vectorToTarget.z = 0;
        
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        
        mainObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}