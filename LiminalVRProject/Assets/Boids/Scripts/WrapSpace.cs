using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapSpace : MonoBehaviour
{
    private Bounds bounds;

    public Bounds GetBounds()
    {
        return bounds;
    }

    public void SetBounds(Bounds newBounds)
    {
        bounds = newBounds;
    }

    public void SetBoundFromTransform(Transform trans)
    {
        Bounds newBounds = new Bounds();
        newBounds.center = trans.position;
        newBounds.extents = trans.localScale/2;
        bounds = newBounds;
    }

    void Update()
    {
        Vector3 position = transform.position;
        if (position.x < bounds.center.x - bounds.size.x/2)
        {
            position.x = bounds.center.x - bounds.size.x / 2;
        }
        else if(position.x > bounds.center.x + bounds.size.x / 2)
        {
            position.x = bounds.center.x + bounds.size.x / 2;
        }

        if (position.y < bounds.center.y - bounds.size.y / 2)
        {
            position.y = bounds.center.y - bounds.size.y / 2;
        }
        else if (position.y > bounds.center.y + bounds.size.y / 2)
        {
            position.y = bounds.center.y + bounds.size.y / 2;
        }

        if (position.z < bounds.center.z - bounds.size.z / 2)
        {
            position.z = bounds.center.z - bounds.size.z / 2;
        }
        else if (position.z > bounds.center.z + bounds.size.z / 2)
        {
            position.z = bounds.center.z + bounds.size.z / 2;
        }
        transform.position = position;
    }
}
