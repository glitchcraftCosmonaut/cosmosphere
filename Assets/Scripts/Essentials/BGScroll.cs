using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    public float speed = 4f;
    private Vector3 startPosition;
    [SerializeField] float xPosScrolling = -24.09f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if(transform.position.x < xPosScrolling)
        {
            transform.position = startPosition;
        }
    }
}
