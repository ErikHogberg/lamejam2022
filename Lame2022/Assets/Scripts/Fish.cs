using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour

{

    public Rigidbody2D rigidbody2D;
    public Vector2 speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D.velocity =speed;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
