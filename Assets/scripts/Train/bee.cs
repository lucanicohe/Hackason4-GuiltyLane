using UnityEngine;

public class bee : MonoBehaviour
{
    
    [SerializeField] private float rotationSpeed = 90f; // degrees per second

    
    [SerializeField] private float riseAmount = 20f;
    [SerializeField] private float riseSpeed = 5f; // units per second

    private bool _isRising;
    private float _targetY;

    private void Update()
    {
        // Rotate endlessly on Y axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // Gradual rise
        if (_isRising)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.MoveTowards(pos.y, _targetY, riseSpeed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Approximately(pos.y, _targetY))
            {
                _isRising = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isRising) return;

        _targetY = transform.position.y + riseAmount;
        _isRising = true;
    }
}