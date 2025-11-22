using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UniRx;
using System;

public class PachinkoManager : MonoBehaviour
{
    [SerializeField]
    private SlotManager slotManager;
    [SerializeField]
    private GameObject _pachinkoHandle;
    [SerializeField]
    private GameObject _Ball;
    [SerializeField]
    GameObject _lancher;
    private float _plusSpeed = 0;
    [SerializeField]
    private float _maxBallPlusSpeed = 100f;
    [SerializeField]
    private float _plusSpeedFactor = 1f;
    private float _pachinkoHandleMaxRotate = 45;
    
    Vector2 _lancherPos;
    Vector2 _firstMousePos = Vector2.zero;
    Vector2 _mousePos;
    Vector2 _firstMouseScreenPos;
    Vector2 _currentMouseScreeenPos;
    private Coroutine BallInterval = null;

    void Start()
    {
        _lancherPos = _lancher.transform.position;
    }
    void Update()
    {
        // 玉が出るマウス操作によって速度が変わる
        if (Input.GetMouseButtonDown(0))
        {
            _firstMouseScreenPos = Input.mousePosition;
            _currentMouseScreeenPos = _firstMouseScreenPos;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(_currentMouseScreeenPos);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null)
            {
                if (_pachinkoHandle == hit.collider.gameObject)
                    StartRelease(_pachinkoHandle);
            }
        }


        // 左クリックしているあいだは、Handleが回る
        if (Input.GetMouseButton(0))
        {
            _currentMouseScreeenPos = Input.mousePosition;

            float deltaX = _currentMouseScreeenPos.x - _firstMouseScreenPos.x;

            float pulledAmount = Mathf.Max(0f, deltaX / 2);

            float angle = -(Mathf.Clamp01(pulledAmount / _maxBallPlusSpeed)  * _pachinkoHandleMaxRotate);
            _pachinkoHandle.transform.localRotation = Quaternion.Euler(0, 0, angle);

            _plusSpeed = Mathf.Clamp(pulledAmount, 0f, _maxBallPlusSpeed) * _plusSpeedFactor;
        }

        

        if (Input.GetMouseButtonUp(0))
        {
            if(BallInterval != null)
                StopCoroutine(BallInterval);
            BallInterval = null;

            _pachinkoHandle.transform.localRotation = quaternion.identity;
            _plusSpeed = 0f;
        }        
    }


    private void StartRelease(GameObject pachinkoHandle)
    {
        // BallIntervalがnullの場合StartCoroutine(StartBallInterval())がBallIntervalに代入される
        BallInterval ??= StartCoroutine(StartBallInterval());
    }

    // PachinkoBallを生み出す
    IEnumerator StartBallInterval()
    {
        while (true)
        {
            float offsetX = UnityEngine.Random.Range(-0.1f, 0.1f);
            GameObject Ball = Instantiate
            (
                _Ball,
                new Vector2(_lancherPos.x + offsetX, _lancherPos.y), 
                quaternion.identity
            );
            Debug.Log("PachinkoBall生成");


            var pachinkoBall = Ball.GetComponent<PachinkoBall>();
            pachinkoBall
                ._onEnter
                .Subscribe(x =>
                {
                    StartCoroutine(slotManager.Lottery());
                })
                .AddTo(this);

            // マウスの位置によって初速度が上がる
            if(_plusSpeed <= _maxBallPlusSpeed)
            {
                pachinkoBall._speed += _plusSpeed;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    
}
