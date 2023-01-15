using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShadow : MonoBehaviour
{

    static RaycastHit2D floor;
    SpriteRenderer shadowSprite;


    void Awake()
    {
        shadowSprite = GetComponent<SpriteRenderer>();

    }


    void Start()
    {
        floor = Physics2D.Linecast(transform.position, new Vector2(transform.position.x, -20));


        
    }

    // Update is called once per frame

    void Update()
    {
        float ShadowX = 1 - transform.parent.position.y/3f;
        float ShadowY = 0.3f - transform.parent.position.y/3f;
        if (ShadowY < 0.1f){ShadowY = 0.1f;}
        transform.localScale = new Vector3(ShadowX, ShadowY, 1);

        if (floor)
        {
            transform.position = new Vector2(transform.position.x, floor.point.y);


        }

    }




}
