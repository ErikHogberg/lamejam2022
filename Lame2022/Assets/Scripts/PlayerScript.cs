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

    public Rigidbody2D hook;
    public Transform Rod;
    public Transform RodEnd;

    public float RodAngle = 20;

    Queue<Vector3> hookHistory;

    float hookRecordTimer = 0f;

    void Start()
    {
        hookHistory = new Queue<Vector3>(10);
    }

    void Update()
    {

        hookRecordTimer -= Time.deltaTime;

        if (hookRecordTimer < 0)
        {
            // if (hookHistory.Count < 1 || Vector3.Distance(hook.position, hookHistory.Peek()) > .01f)
            hookHistory.Enqueue(hook.position);
            hookRecordTimer += .2f;
        }

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
            hook.AddForce(Vector2.up*10);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            x -= speed * Time.deltaTime;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            y -= speed * Time.deltaTime;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            x += speed * Time.deltaTime;
        }

        var position = transform.position;
        position.x += x;
        position.y += y;
        transform.position = position;

        line.spline.Clear();
        Vector3 lastPos = RodEnd.position;
        line.spline.InsertPointAt(0, lastPos);

        while (hookHistory.Count > 10)
        {
            hookHistory.Dequeue();
        }
        foreach (var item in hookHistory)
        {
            if (Vector3.Distance(lastPos, item) > .1f)
                line.spline.InsertPointAt(0, item);

            lastPos = item;
        }
        if (Vector3.Distance(lastPos, hook.position) > .1f)
            line.spline.InsertPointAt(0, hook.position);
        //float randomheight =Random.RandomRange(0.8f, 1.2f);
        // var scale = transform.localScale;
        // scale.y = randomheight;
        // transform.localScale = scale;

    }
}
