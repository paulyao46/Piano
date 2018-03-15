using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilderNode : Node
{
    
    public GameObject[] nodeIndex;
    public List<float> times;
    public List<int> Tracks;
    private int index;
    void Start()
    {
        level = Level.MISS;
        state = State.IDLE;
        index = 0;
        for (int i = 0; i < 6; i++)
        {
            times.Add(time + 0.25f * i);

        }
        Tracks.Add(track);
        Tracks.Add(track);
        Tracks.Add(track - 1);
        Tracks.Add(track - 2);
        Tracks.Add(track - 3);
        Tracks.Add(track - 3);
    }
    public override void Update()
    {
        transform.Translate(0, -Time.deltaTime * speed, 0);
        
    }
    public override Level determination(KeyState keyState,int track, float audioTime)
    {
      switch(state)
        {
            case State.IDLE:
                if (time >= audioTime - 0.05f && time <= audioTime + 0.05f)
                {
                    index++;
                    state = State.IN;
                    level = Level.PREFECT;
                    return Level.CONTINUE;
                }
                else if (time >= audioTime - 0.1f && time <= audioTime + 0.1f)
                {
                    index++;
                    state = State.IN;
                    level = Level.GOOD;
                    return Level.CONTINUE;
                }
                else if (time >= audioTime - 0.2f && time <= audioTime + 0.2f)
                {
                    index++;
                    state = State.IN;
                    level = Level.BAD;
                    return Level.CONTINUE;
                }
                else
                {
                    return Level.UNABLE;
                }
                
            case State.IN:
                if(index< times.Count&& times[index] >=audioTime-0.1f && times[index]<=audioTime+0.1f)
                {
                    if(Tracks[index]!= Tracks[index-1])
                    {
                        if(keyState==KeyState.PRESS&& track== Tracks[index])
                        {
                            index++;
                            if(index== times.Count-1)
                            {
                                state = State.OUT;
                            }
                            return Level.CONTINUE;
                        }
                    }
                    else
                    {
                        if(track == Tracks[index])
                        {
                            index++;
                            if (index == times.Count-1)
                            {
                                state = State.OUT;
                            }
                            return Level.CONTINUE;
                        }
                    }
                }
                else if(index < times.Count && times[index] >= audioTime + 0.15f)
                {
                    return Level.MISS;
                }
                break;
            case State.SLIDE:
                break;
            case State.OUT:
                if (times[index] >= audioTime - 0.05f && times[index] <= audioTime + 0.05f)
                {
                    
                    state = State.IN;
                    if(level == Level.PREFECT)
                    {
                        level = Level.PREFECT;
                    }
                    return Level.CONTINUE;
                }
                else if (times[index] >= audioTime - 0.1f && times[index] <= audioTime + 0.1f)
                {
                    
                    state = State.IN;
                    level = Level.GOOD;
                    return Level.CONTINUE;
                }
                else if (times[index] >= audioTime - 0.2f && times[index] <= audioTime + 0.2f)
                {
                    
                    state = State.IN;
                    level = Level.BAD;
                    return Level.CONTINUE;
                }
                else
                {
                    return Level.UNABLE;
                }
           
        }

        return Level.CONTINUE;
    }
    public override void needDestory()
    {
        Destroy(this.gameObject);
    }
    public override void missDestory()
    {
        base.missDestory();
    }
    public void isPress()
    {
        Instantiate(particle, new Vector3(nodeIndex[0].transform.position.x, nodeIndex[0].transform.position.y + 0.3f, nodeIndex[0].transform.position.z), nodeIndex[0].transform.rotation);
    }

}
