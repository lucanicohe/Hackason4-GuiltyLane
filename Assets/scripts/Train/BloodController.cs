using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    [SerializeField] private  Sprite[] _bloodsprites;
    private SpriteRenderer _spriterenderer;
    private int currentIndex = 0;

    private void Start()
    {
       _spriterenderer = GetComponent<SpriteRenderer>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blood"))
        {
            if (_spriterenderer != null && _bloodsprites.Length > 0)
            {
                _spriterenderer.sprite = _bloodsprites[currentIndex];
                currentIndex++;
                
                if (currentIndex >= _bloodsprites.Length)
                    currentIndex = 0;
            }
        }
    }
}