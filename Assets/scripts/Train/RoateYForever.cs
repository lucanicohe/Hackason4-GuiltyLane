using UnityEngine;

public class RotateYForever : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 60f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}