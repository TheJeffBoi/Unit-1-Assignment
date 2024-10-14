using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using UnityEngine;

public class HelperScript : MonoBehaviour
{
    LayerMask groundLayerMask;
    private float delay;

    // Start is called before the first frame update
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
        delay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Player Jump Raycast
    public bool ExtendedRayCollisionCheck(float xoffs, float yoffs, float rayLength)
    {
        bool hitSomething = false;

        Vector3 offset = new Vector3(xoffs, yoffs, 0);
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position + offset, -Vector3.up, rayLength, groundLayerMask);
        Color hitColor = Color.red;

        if (hit.collider != null)
        {
            hitColor = Color.green;
            hitSomething = true;
        }
        Debug.DrawRay(transform.position + offset, -Vector3.up * rayLength, hitColor);

        return hitSomething;
    }

    public bool ExtendedRayEdgeCheck(float xoffs, float yoffs, float rayLength)
    {
        bool offEdge = false;

        Vector3 offset = new Vector3(xoffs, yoffs, 0);
        RaycastHit2D off;

        off = Physics2D.Raycast(transform.position + offset, -Vector3.up, rayLength, groundLayerMask);
        Color offColor = Color.red;

        if (off.collider != null)
        {
            offColor = Color.green;
            offEdge = true;
        }
        Debug.DrawRay(transform.position + offset, -Vector3.up * rayLength, offColor);

        return offEdge;
    }

    public bool Delay(float delayTime)
    {
        bool timeFinished = false;

        delay += Time.deltaTime;

            if (delay > delayTime)
            {
                delay = 0;
            }

        timeFinished = true;
        return timeFinished;
    }
}
