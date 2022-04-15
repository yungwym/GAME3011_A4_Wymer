using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private List<Transform> points;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetUpLine(List<Transform> points)
    {
        lineRenderer.positionCount = points.Count;
        this.points = points;
    }

    private void Update()
    {
        if (points != null)
        {
            for (int i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i].position);
            }
        }
    }
}
