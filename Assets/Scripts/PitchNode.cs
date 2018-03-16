using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchNode : Node
{
    public override Level determination(KeyState keyState, int track, float audioTime)
    {
        if(type == keyState && !hasDeterminate)
        {
            
            if (audioTime >= time - 0.05f && audioTime <= time + 0.05f)
            {
                hasDeterminate = true;
                level = Level.PREFECT;
                return Level.PREFECT;
            }
            else if (audioTime >= time - 0.1f && audioTime <= time + 0.1f)
            {
                hasDeterminate = true;
                level = Level.GOOD;
                return Level.GOOD;
            }
            else if (audioTime >= time - 0.2f && audioTime <= time + 0.2f)
            {
                hasDeterminate = true;
                level = Level.BAD;
                return Level.BAD;
            }
            else
            {
                hasDeterminate = false;
                return Level.UNABLE;
            }
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
