using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [System.Flags]
    public enum eScreenLocations
    {
        onScreen = 0,
        offRight = 1,
        offLeft = 1 << 1,
        offUp = 1 << 2,
        offDown = 1 << 3 
    }
    public enum eType { center, inset, outset };

    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocations screenLocations = eScreenLocations.onScreen;
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
        screenLocations = eScreenLocations.onScreen;

        if (position.x > cameraWidth + checkRadius)
        {
            position.x = cameraWidth + checkRadius;
            screenLocations |= eScreenLocations.offRight;
        }
        
        if (position.x < -cameraWidth - checkRadius)
        {
            position.x = -cameraWidth - checkRadius;
            screenLocations |= eScreenLocations.offLeft;
        }
        
        if (position.y > cameraHeight + checkRadius)
        {
            position.y = cameraHeight + checkRadius;
            screenLocations |= eScreenLocations.offUp;
        }
        
        if (position.y < -cameraHeight - checkRadius)
        {
            position.y = -cameraHeight - checkRadius;
            screenLocations |= eScreenLocations.offDown;
        }

        if (keepOnScreen && !isOnScreen)
        {
            transform.position = position;
            screenLocations = eScreenLocations.onScreen;
        }
    }

    public bool isOnScreen { get { return screenLocations == eScreenLocations.onScreen; } }

    public bool LocationIs(eScreenLocations checkLocation)
    {
        return (checkLocation == eScreenLocations.onScreen)
            ? isOnScreen
            : ((screenLocations & checkLocation) == checkLocation);
    }
}
