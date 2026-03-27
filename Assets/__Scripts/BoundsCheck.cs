using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    public enum eType { center, inset, outset };

    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    
    [Header("Dynamic")]
    public float cameraWidth;
    public float cameraHeight;

    void Awake()
    {
        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        float checkRadius = 0;
        if (boundsType == eType.inset) checkRadius = -radius;
        if (boundsType == eType.outset) checkRadius = radius;
        
        Vector3 position = transform.position;

        if (position.x > cameraWidth + checkRadius)
            position.x = cameraWidth + checkRadius;
        
        if (position.x < -cameraWidth - checkRadius)
            position.x = -cameraWidth - checkRadius;
        
        if (position.y > cameraHeight + checkRadius)
            position.y = cameraHeight + checkRadius;
        
        if (position.y < -cameraHeight - checkRadius)
            position.y = -cameraHeight - checkRadius;
        
        transform.position = position;
    }
}
