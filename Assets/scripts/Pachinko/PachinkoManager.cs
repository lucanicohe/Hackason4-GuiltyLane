using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class PachinkoManager : MonoBehaviour
{
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private Button _pachinkoHandleButton;
    [SerializeField] private GameObject _Ball;
    [SerializeField] private GameObject _lancher;
    
    [SerializeField] private float _spawnInterval = 0.3f;

    Vector2 _lancherPos;
    
    // Speed levels: 0=stop, 1=low, 2=medium, 3=high
    private int _currentSpeedLevel = 0;
    private int _direction = 1; // 1 = increasing (clockwise), -1 = decreasing (counter-clockwise)
    private float[] _speedLevels = { 0f, 0.3f, 0.6f, 0.9f }; // Speed multipliers - bigger gaps
    private float[] _handleAngles = { 0f, -15f, -30f, -45f }; // Handle angles for each level
    
    private Coroutine _ballSpawnCoroutine = null;
    private const float NEUTRAL_DEADZONE = 0.05f;

    void Start()
    {
        _lancherPos = _lancher.transform.position;
        
        // Add click listener to button
        if (_pachinkoHandleButton != null)
        {
            _pachinkoHandleButton.onClick.AddListener(OnHandleClicked);
        }
        
        Debug.Log("PachinkoManager started. Click the handle button to cycle through speeds.");
    }

    void OnHandleClicked()
    {
        // Determine next speed level
        int nextLevel = _currentSpeedLevel + _direction;
        
        // If we reach max (3) or min (0), reverse direction
        if (nextLevel > 3)
        {
            _direction = -1;
            nextLevel = 2; // Go back to speed 2
        }
        else if (nextLevel < 0)
        {
            _direction = 1;
            nextLevel = 1; // Go back to speed 1
        }
        
        _currentSpeedLevel = nextLevel;
        UpdateHandle();
        Debug.Log($"Clicked handle! Speed level: {_currentSpeedLevel}, Direction: {(_direction == 1 ? "increasing" : "decreasing")}");
    }

    void UpdateHandle()
    {
        // Update handle rotation based on current speed level
        _pachinkoHandleButton.transform.localRotation = Quaternion.Euler(0, 0, _handleAngles[_currentSpeedLevel]);
        
        // Start/stop ball spawning based on speed level
        float speedMultiplier = _speedLevels[_currentSpeedLevel];
        bool isStopped = speedMultiplier < NEUTRAL_DEADZONE;
        
        if (!isStopped && _ballSpawnCoroutine == null)
        {
            _ballSpawnCoroutine = StartCoroutine(SpawnBallsCoroutine());
            Debug.Log("Started ball spawning at speed level: " + _currentSpeedLevel);
        }
        else if (isStopped && _ballSpawnCoroutine != null)
        {
            StopCoroutine(_ballSpawnCoroutine);
            _ballSpawnCoroutine = null;
            Debug.Log("Stopped ball spawning");
        }
    }

    IEnumerator SpawnBallsCoroutine()
    {
        while (_currentSpeedLevel > 0 && _speedLevels[_currentSpeedLevel] > NEUTRAL_DEADZONE)
        {
            // Check if we have balls
            if (GameManager.Instance.Balls <= 0)
            {
                Debug.Log("No balls left!");
                yield return new WaitForSeconds(0.2f);
                continue;
            }
            
            // Deduct a ball
            GameManager.Instance.AddBalls(-1);
            Debug.Log($"Spawned ball at speed level {_currentSpeedLevel}! Remaining: {GameManager.Instance.Balls}");
            
            // Spawn ball at launcher with slight random offset
            float offsetX = UnityEngine.Random.Range(-0.1f, 0.1f);
            GameObject ballObj = Instantiate(
                _Ball,
                new Vector2(_lancherPos.x + offsetX, _lancherPos.y), 
                quaternion.identity,
                this.transform
            );

            // Configure ball
            var pachinkoBall = ballObj.GetComponent<PachinkoBall>();
            if (pachinkoBall != null)
            {
                // Use CURRENT speed level, not captured parameter
                float currentSpeedMultiplier = _speedLevels[_currentSpeedLevel];
                pachinkoBall._speed = 45f + (currentSpeedMultiplier * 150f);
                
                Debug.Log($"Ball speed set to: {pachinkoBall._speed} (level {_currentSpeedLevel})");
                
                // Subscribe to ball entry event
                pachinkoBall._onEnter
                    .Subscribe(x =>
                    {
                        slotManager._canReelCount++;
                        Debug.Log("中央ポケットに入った: " + slotManager._canReelCount);
                    })
                    .AddTo(this);
            }
            
            // Constant spawn interval
            yield return new WaitForSeconds(_spawnInterval);
        }
        
        _ballSpawnCoroutine = null;
    }
}
