using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera displayCamera;

    private int currentDisplay = 0; // 0 = main, 1 = display1, 2 = display2, 3 = display3

    void Start()
    {
        
        mainCamera.gameObject.SetActive(true);
        displayCamera.gameObject.SetActive(false);
    }

    private Vector3 _targetPosition;
    [SerializeField] private float _moveSpeed = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ToggleDisplay(1, new Vector3(-6.42f, 6.9f, 1.870422f));

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ToggleDisplay(2, new Vector3(-4f, 6.9f, 1.870422f));

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ToggleDisplay(3, new Vector3(-1.785f, 6.9f, 1.870422f));
    }

    private void ToggleDisplay(int displayIndex, Vector3 position)
    {
        
        if (currentDisplay == displayIndex)
        {
            displayCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            currentDisplay = 0;
        }
        else
        {
            mainCamera.gameObject.SetActive(false);
            displayCamera.gameObject.SetActive(true);
            displayCamera.transform.localPosition = position;
            currentDisplay = displayIndex;
        }

        // === MoveTowards === //
        // _displayCamera.transform.localPosition =
        //     Vector3.MoveTowards(
        //         _displayCamera.transform.localPosition,
        //         _targetPosition,
        //         _moveSpeed * Time.deltaTime
        //     );
    }

    // public void MainCamPos()
    // {
    //     _display1Activated = false;
    //     _display2Activated = false;
    //     _display3Activated = false;

    //     _targetPosition = new Vector3(-3.089999f,
    //             9.9f,
    //             -4.359997f);
    //     _displayCamera.transform.localPosition = _targetPosition;
    // }
}
