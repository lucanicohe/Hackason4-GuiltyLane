using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class TrainSplineMover : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool switchToSecondSpline;
    [SerializeField] private float3 posOffset;

    private int currentSplineIndex;
    private float t;

    private void Update()
    {
        if (splineContainer == null) return;
        if (t > 1f && currentSplineIndex == 1)
        {
            currentSplineIndex = 0;
            t = 0f;
        }
        if (switchToSecondSpline && currentSplineIndex == 0)
        {
            currentSplineIndex = 1;
            t = 0f; // 重設進度
            Debug.Log("Switch to spline1");
        }

        Spline spline = null;
        var num = splineContainer.Splines.Count;
        if (currentSplineIndex < num)
        {
            spline = splineContainer[currentSplineIndex];
        }
        if (spline == null) return;

        float length = spline.GetLength();
        t += Time.deltaTime * moveSpeed / length;

        if (t > 1f)
        {
            t = 0f;
        }

        // 移動位置
        float3 position = spline.EvaluatePosition(t);
        position += posOffset;
        transform.position = position;

        // (可選) 轉向
        float3 tangent = spline.EvaluateTangent(t);
        if (!math.all(tangent == 0))
        {
            transform.rotation = Quaternion.LookRotation(tangent);
        }
    }
    
}
