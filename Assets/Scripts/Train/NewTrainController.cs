
using System.Collections;
using UnityEngine;

public class NewTrainController : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    private float _currentSpeed;
    [SerializeField] private float accelerationTime = 1f;
    
    public Direction lastDirection = Direction.Right;

    [SerializeField] private Transform directionLever;
    private const float RightLeverPosX = 0.46f;
    private const float LeftLeverPosX = -0.469f;

    [SerializeField] private Transform throttleLever;
    private const float SpeedOnPosZ = 0.1875f;
    private const float SpeedOffPosZ = -0.266f;

    private Coroutine _trainCoroutine;
    private Coroutine _effectsCoroutine;
    private bool _isrunning;
    
    private void Awake()
    {   
        if (directionLever != null)
            SwitchDirectionLeverDirection(lastDirection);

        if (throttleLever != null)
            SwitchThrottleLeverDirection(false);
        _currentSpeed = 0f;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (_currentSpeed * Time.deltaTime));
        // Change direction with keyboard
        if (Input.GetKeyDown(KeyCode.A))
        {
            SwitchDirectionLeverDirection(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SwitchDirectionLeverDirection(Direction.Right);
        }
        // Change speed with keyboard
        // if (Input.GetKeyDown(KeyCode.W)&& !_isrunning)
        // {
        //     StartTrain();
        //     _isrunning = true;
        // }
        // else if (Input.GetKeyDown(KeyCode.S))
        // {
        //     StopTrain();
        //     _isrunning = false; 
        // }
    }
    
    private IEnumerator TrainDepature()
    {
        float elapsedTime = 0f;
        while (elapsedTime < accelerationTime)
        {
            elapsedTime += Time.deltaTime;
            float per = elapsedTime / accelerationTime;
            _currentSpeed = Mathf.Lerp(0f, speed, per * per);

            yield return null;
        }
    }

    private IEnumerator TrainStop()
    {
        float elapsedTime = 0f;
        while (elapsedTime < accelerationTime)
        {
            elapsedTime += Time.deltaTime;
            float per = elapsedTime / accelerationTime;
            _currentSpeed = Mathf.Lerp(speed, 0f, per * per);

            yield return null;
        }
    }
    
    private void StartTrain()
    {
        if (_trainCoroutine != null)
            StopCoroutine(_trainCoroutine);
        _trainCoroutine = StartCoroutine(TrainDepature());
        SwitchThrottleLeverDirection(true);
    }

    private void StopTrain()
    {
        if (_trainCoroutine != null)
            StopCoroutine(_trainCoroutine);
        _trainCoroutine = StartCoroutine(TrainStop());
        SwitchThrottleLeverDirection(false);
    }

    private void SwitchDirectionLeverDirection(Direction dir)
    {   
        var levDir = directionLever.localPosition;
        levDir.x = dir == Direction.Right ? RightLeverPosX : LeftLeverPosX;
        directionLever.localPosition = levDir;
    }

    private void SwitchThrottleLeverDirection(bool speedOn)
    {
        var levDir = throttleLever.localPosition;
        levDir.z = speedOn ? SpeedOnPosZ : SpeedOffPosZ;
        throttleLever.localPosition = levDir;
    }
    
}
