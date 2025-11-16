using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PachinkoManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _pachinkoHandle;
    [SerializeField]
    private GameObject _Ball;
    [SerializeField]
    GameObject _lancher;
    private float _plusSpeed = 0;
    [SerializeField]
    private float _maxBallPlusSpeed = 10f;
    private float _pachinkoHandleMaxRotate = 45;
    Vector3 _lancherPos;
    Vector3 _firstMousePos = Vector3.zero;
    Vector3 _mousePos;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(_firstMousePos == Vector3.zero)
            {
                _firstMousePos = Input.mousePosition;
                _mousePos = _firstMousePos;
            }

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                
                if(_pachinkoHandle == hit.collider.gameObject)
                    StartRelease(_pachinkoHandle);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(BallInterval != null)
                StopCoroutine(BallInterval);
            BallInterval = null;

            _pachinkoHandle.transform.localRotation = quaternion.identity;
            _plusSpeed = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            _mousePos = Input.mousePosition;

            float deltaX = _mousePos.x - _firstMousePos.x;

            float pulledAmount = Mathf.Max(0f, deltaX / 10);

            float angle = -(Mathf.Clamp(pulledAmount / _maxBallPlusSpeed, 0f, 1f)  * _pachinkoHandleMaxRotate);
            _pachinkoHandle.transform.localRotation = Quaternion.Euler(0, 0, angle);

            if(_pachinkoHandle.transform.localRotation.z <= 0)
                _plusSpeed = Vector3.Distance(_firstMousePos, _mousePos) / _maxBallPlusSpeed;

            
            // if(_firstMousePos.x <= _mousePos.x)
            // {
            //     _mousePos = Input.mousePosition;
            //     _plusSpeed = Vector3.Distance(_firstMousePos, _mousePos) / _maxBallPlusSpeed;
            // }
            // else
            // {
            //     _mousePos = _firstMousePos;
            //     _plusSpeed = 0f;
            //     _pachinkoHandle.transform.localRotation = quaternion.identity;
            // }


            // // ハンドルが回る
            // _pachinkoHandle.transform.localRotation 
            // = Quaternion.Euler(0, 0, -(_plusSpeed * 4 / _maxBallPlusSpeed * _pachinkoHandleMaxRotate));
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
                new Vector3(_lancherPos.x + offsetX, _lancherPos.y, _lancherPos.z), 
                quaternion.identity
            );
            
            // マウスの位置によって初速度が上がる
            if(_plusSpeed < _maxBallPlusSpeed)
            {
                Ball.GetComponent<PachinkoBall>()._speed += _plusSpeed;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
