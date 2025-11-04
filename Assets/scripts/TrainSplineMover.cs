using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class TrainSplineMover : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float3 posOffset;

    public enum SplineType
    {
        Main,
        Left,
        Right,
        None
    }
    [SerializeField]public SplineType switchSplineType;
    private SplineType _lastChooseSplineType;
    
    private Spline _currentSpline;
    private float _t;

    private void SwitchSpline(SplineType splineType)
    {
        var type = splineType;
        if (type == SplineType.None)
        {
            type = _lastChooseSplineType;
        }
        _currentSpline = splineContainer[(int)type];
        switch (type)
        {
            case SplineType.Main:
                switchSplineType = SplineType.None;
                break;
            case SplineType.Left or SplineType.Right:
                _lastChooseSplineType = switchSplineType;
                switchSplineType = SplineType.Main;
                break;
        }
       
    }

    private void Awake()
    {
        _currentSpline = splineContainer[0];
        switchSplineType = SplineType.Right;
        _lastChooseSplineType = SplineType.Right;
    }

    private void Update()
    {
        SplineMove(_currentSpline);
    }

    private void SplineMove(Spline spline)
    {
        if (spline == null) return;
        float length = spline.GetLength();
        _t += Time.deltaTime * moveSpeed / length;

        if (_t > 1f)
        {
            SwitchSpline(switchSplineType);
            _t = 0;
        }
        
        float3 position = spline.EvaluatePosition(_t);
        position += posOffset;
        transform.position = position;
        
        float3 tangent = spline.EvaluateTangent(_t);
        if (!math.all(tangent == 0))
        {
            transform.rotation = Quaternion.LookRotation(tangent);
        }
    }
}
