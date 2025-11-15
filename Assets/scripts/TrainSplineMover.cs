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
    public SplineType switchSplineType;
    
    private SplineType _currentSplineType;
    
    private Spline _currentSpline;
    private float _t;

    private void SwitchSpline(SplineType splineType)
    {
        if (_currentSplineType is SplineType.Main)
        {
            _currentSplineType = splineType;
        }
        else if(_currentSplineType is SplineType.Left or SplineType.Right)
        {
            _currentSplineType = SplineType.Main;
        } 
        _currentSpline =  splineContainer[(int)_currentSplineType];
    }

    private void Awake()
    {
        _currentSpline = splineContainer[0];
        _currentSplineType = SplineType.Main;
        switchSplineType  = SplineType.Right;
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
