using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Inscribed")]
    public Transform playerTransform;
    public Transform[] panels;
    [Tooltip("Speed at which the panels move in Y")]
    public float scrollSpeed = -30;
    [Tooltip("Controls how much panels react to player movement (Default 0.25)")]
    public float motionMultiplier = 0.25f;

    private float panelHeight;
    private float depth;

    void Start()
    {
        panelHeight = panels[0].localScale.y;
        depth = panels[0].position.z;

        panels[0].position = new(0, 0, depth);
        panels[1].position = new(0, panelHeight, depth);
    }

    void Update()
    {
        float transformY, transformX = 0;
        transformY = Time.time * scrollSpeed % panelHeight + (panelHeight * 0.5f);

        if (playerTransform != null)
        {
            transformX = -playerTransform.transform.position.x * motionMultiplier;
        }

        panels[0].position = new(transformX, transformY, depth);
        if (transformY >= 0)
        {
            panels[1].position = new(transformX, transformY - panelHeight, depth);
        }
        else
        {
            panels[1].position = new(transformX, transformY + panelHeight, depth);
        }
    }
}
