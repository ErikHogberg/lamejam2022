using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float speed = 1;
    public Transform mousebox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;

        Vector2 mouseposition = Mouse.current.position.ReadValue();
        var mouseboxposition = Vector3.zero;
        mouseboxposition.x = mouseposition. x;
        mouseboxposition.y = mouseposition. y;
        mouseboxposition = Camera.main.ScreenToWorldPoint ( mouseboxposition);
        mouseboxposition.z = 0;
        mousebox.position = mouseboxposition;
        if (Keyboard.current.wKey.isPressed)
        {
            y += speed * Time.deltaTime;
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            x -= speed * Time.deltaTime;
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            y -= speed * Time.deltaTime;
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            x += speed * Time.deltaTime;
        }
        var position = transform.position;
        position.x += x;
        position.y += y;
        transform.position = position;


    }
}
