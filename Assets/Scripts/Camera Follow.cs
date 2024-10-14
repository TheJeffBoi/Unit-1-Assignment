using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public float yBottom, yTop, xLeft, xRight;
    private float smoothTime = 0.25f;
    private Vector3 offset = new Vector3(0, 0, -10);
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 pos = transform.position;
        pos = Vector3.SmoothDamp(pos, targetPosition, ref velocity, smoothTime);

        if (pos.y < yBottom)
        {
            pos.y = yBottom;
        }

        if (pos.y > yTop)
        {
            pos.y = yTop;
        }

        if (pos.x > xLeft)
        {
            pos.x = xLeft;
        }

        if (pos.x < xRight)
        {
            pos.x = xRight;
        }

        transform.position = pos;
    }
}
