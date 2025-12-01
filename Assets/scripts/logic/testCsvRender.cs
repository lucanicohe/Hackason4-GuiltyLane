using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCsvRender : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log($"{CSVRender.Instance.TryGetTxtByID(1)}");
        }
    }
}
