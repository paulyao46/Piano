using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchNode : Node
{
    public override Level determination(KeyState keyState, int track, float audioTime)
    {
        if (time >= audioTime - 0.05f && time <= audioTime + 0.05f)
        {
            level = Level.PREFECT;
            return Level.PREFECT;
        }
        else if (time >= audioTime - 0.1f && time <= audioTime + 0.1f)
        {
            level = Level.GOOD;
            return Level.GOOD;
        }
        else if (time >= audioTime - 0.2f && time <= audioTime + 0.2f)
        {
            level = Level.BAD;
            return Level.BAD;
        }
        else
        {
            return Level.UNABLE;
        }
    }
    public override void needDestory(int track)
    {
        base.needDestory(track);
    }
    public override void missDestory()
    {
        base.missDestory();
    }
}
