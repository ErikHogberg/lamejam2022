using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerScript : MonoBehaviour
{
    public float speed = 1;
    public Transform mousebox;
    public SpriteShapeController line;
    public float lineSegmentLength = 10;

    public Rigidbody hook;

    Queue<Vector3> hookHistory;

    float hookRecordTimer = -1f;

    void Start()
    {
        hookHistory = new Queue<Vector3>(10);
    }

    void Update()
    {
        float x = 0;
        float y = 0;

        Vector2 mouseposition = Pointer.current.position.ReadValue();
        var mouseboxposition = new Vector3(mouseposition.x, mouseposition.y, 0);
        mouseboxposition = Camera.main.ScreenToWorldPoint(mouseboxposition);
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

        line.spline.Clear();
        line.spline.InsertPointAt(0, transform.localPosition);
        foreach (var item in hookHistory)
        {
            line.spline.InsertPointAt(0, transform.InverseTransformPoint(item));
        }
        line.spline.InsertPointAt(0, transform.InverseTransformPoint(hook.position));

    }
}
