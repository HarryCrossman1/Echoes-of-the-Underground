using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public static LineManager instance;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        instance= this;
        lineRenderer= GetComponent<LineRenderer>();
    }

    public void RenderLine(Transform origin, Transform destination)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, destination.position);
    }
}
