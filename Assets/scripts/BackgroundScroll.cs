using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private Renderer _spr;

    [SerializeField] private float _speedPerSecond = 0.05f;
    [SerializeField] private float _offsetX = 0f;
    [SerializeField] private Rect _moveTerritory = new Rect(0f, 0f, 2f, 2f);
    public Rect MoveTerritory => _moveTerritory; 
    //public Rect MoveT
    //{
        //get { return _moveTerritory; }
    //}



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpeedUp(float scale)
    {
        _speedPerSecond += scale; 
    }

    // Update is called once per frame
    void Update()
    {
        _offsetX += _speedPerSecond * Time.deltaTime;
        if (_offsetX > 1f) _offsetX -= 2f;
        if (_offsetX < -1f) _offsetX += 2f;
        _spr.material.SetTextureOffset("_MainTex", new Vector2(_offsetX, 0f));

        
        
    }
}
