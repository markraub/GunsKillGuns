using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroller : MonoBehaviour
{

    [SerializeField] Vector2 Speed;

    private SpriteRenderer cloudSR;
    void Start()
    {
        cloudSR = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        //cloudSR.sprit += Speed * Time.deltaTime;


        
    }
}
