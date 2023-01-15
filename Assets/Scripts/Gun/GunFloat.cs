using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFloat : MonoBehaviour
{

    [SerializeField] float FloatDistance;
    [SerializeField] float Speed;

    private Rigidbody2D pistolRigidBody;

    void Awake()
    {
        pistolRigidBody = GetComponent<Rigidbody2D>();
        FloatDistance += Random.Range(-0.3f, 0.3f);
        Speed += Random.Range(-0.3f, 0.3f);
    }

    void Update()
    {
        
        
        

        float floatpos = Mathf.PingPong(Time.time * Speed, FloatDistance) - 1;
        transform.position = new Vector2(transform.position.x, floatpos);
       
    }
}
