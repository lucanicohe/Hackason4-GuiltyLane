using System.Collections.Generic;
using UnityEngine;

public class SplineRail : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField] private int _resolutionPerSegment = 20;

    public List<Transform> controlPoints = new List<Transform>();

    private List<Vector3> _samples = new List<Vector3>();
    private List<float> _distances = new List<float>();
    private float _totalLength;

    public float TotalLength => _totalLength;

    void Awake()
    {
        BuildSpline();
    }

    public void BuildSpline()
    {
        _samples.Clear();
        _distances.Clear();

        if (controlPoints.Count < 4)
        {
            Debug.LogError("SplineRail needs at least 4 control points");
            return;
        }

        Vector3 prev = GetPoint(0f);
        _samples.Add(prev);
        _distances.Add(0f);

        float dist = 0f;

        int segments = controlPoints.Count - 3;
        for (int i = 0; i < segments; i++)
        {
            for (int j = 1; j <= _resolutionPerSegment; j++)
            {
                float t = j / (float)_resolutionPerSegment;
                Vector3 p = GetPoint(i + t);

                dist += Vector3.Distance(prev, p);
                _samples.Add(p);
                _distances.Add(dist);

                prev = p;
            }
        }

        _totalLength = dist;
    }

    public Vector3 GetPositionAtDistance(float distance)
    {
        distance = Mathf.Clamp(distance, 0f, _totalLength);

        for (int i = 1; i < _distances.Count; i++)
        {
            if (_distances[i] >= distance)
            {
                float t = Mathf.InverseLerp(_distances[i - 1], _distances[i], distance);
                return Vector3.Lerp(_samples[i - 1], _samples[i], t);
            }
        }

        return _samples[^1];
    }

    public Vector3 GetTangentAtDistance(float distance)
    {
        float d1 = Mathf.Clamp(distance - 0.1f, 0f, _totalLength);
        float d2 = Mathf.Clamp(distance + 0.1f, 0f, _totalLength);
        return (GetPositionAtDistance(d2) - GetPositionAtDistance(d1)).normalized;
    }

    private Vector3 GetPoint(float t)
    {
        int i = Mathf.FloorToInt(t);
        t -= i;

        Vector3 p0 = controlPoints[i].position;
        Vector3 p1 = controlPoints[i + 1].position;
        Vector3 p2 = controlPoints[i + 2].position;
        Vector3 p3 = controlPoints[i + 3].position;

        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    void OnDrawGizmos()
    {
        if (controlPoints.Count < 4) return;

        Gizmos.color = Color.cyan;
        Vector3 prev = GetPoint(0f);

        int segments = controlPoints.Count - 3;
        for (int i = 0; i < segments; i++)
        {
            for (int j = 1; j <= _resolutionPerSegment; j++)
            {
                float t = j / (float)_resolutionPerSegment;
                Vector3 p = GetPoint(i + t);
                Gizmos.DrawLine(prev, p);
                prev = p;
            }
        }
    }
}