using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSin : MonoBehaviour
{

    float sinCenterY;
    [SerializeField] float amplitude = 2;
    [SerializeField] float frequency = 2;
    public bool inverted = false;

    // Start is called before the first frame update
    void Start()
    {
        sinCenterY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate() 
    {
        Vector2 pos = transform.position;

        float sin = Mathf.Sin(pos.x * frequency) * amplitude;
        if(inverted)
        {
            sin *= -1;
        }
        pos.y = sinCenterY + sin;

        transform.position = pos;    
    }
}
