using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakU;


public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DanmakuCollider>().OnDanmakuCollision += OnDanmakuCollision;
    }

    void OnDanmakuCollision(DanmakuCollisionList collisionList)
    {
        Debug.Log("Player collided with Danmaku bullet");
        Debug.Log(collisionList[0].ToString());
        //your code here

    }
    
}
