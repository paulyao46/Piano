using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilderNode : Node
{
    public SilderNode NextNode;

    public override void Update()
    {
        transform.Translate(0, 0, -Time.deltaTime * speed);

    }
    public override Level determination(KeyState keyState, int track, float audioTime)
    {
        return Level.MISS;
    }
    public override void needDestory(int track)
    {

        Destroy(this.gameObject);

    }
    public override void missDestory()
    {

        base.missDestory();
    }
    public override void closePitch()
    {

        //GetComponent<LineRenderer>().enabled = false;
    }


}
