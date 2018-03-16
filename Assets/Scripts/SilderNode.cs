using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilderNode : Node
{
    public GameObject[] nodeIndex;
    public List<float> times;
    public List<int> Tracks;
    private Level preLevel;
    private int index;
    public GameObject Mask;
    void Start()
    {
        Mask = GameObject.Find("Mask");
        Mask.SetActive(false);
        preLevel = Level.MISS;
        level = Level.MISS;
        state = State.IDLE;
        index = 0;
        for (int i = 0; i < 5; i++)
        {
            times.Add(time + 1.0f * i);

        }
        times.Add(time + 4.1f);
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
    public override Level determination(KeyState keyState, int track, float audioTime)
    {
        switch (state)
        {
            case State.IDLE:
                if (time >= audioTime - 0.05f && time <= audioTime + 0.05f)
                {
                    Mask.SetActive(true);
                   index++;
                    state = State.IN;
                    preLevel = Level.PREFECT;
                    level = Level.CONTINUE;
                    return Level.CONTINUE;
                }
                else if (time >= audioTime - 0.1f && time <= audioTime + 0.1f)
                {
                    Mask.SetActive(true);
                    index++;
                    state = State.IN;
                    preLevel = Level.GOOD;
                    level = Level.CONTINUE;
                    return Level.CONTINUE;
                }
                else if (time >= audioTime - 0.2f && time <= audioTime + 0.2f)
                {
                    Mask.SetActive(true);
                    index++;
                    state = State.IN;
                    preLevel = Level.BAD;
                    level = Level.CONTINUE;
                    return Level.CONTINUE;
                }
                else
                {
                    return Level.UNABLE;
                }

            case State.IN:
                if (index < times.Count && audioTime >= times[index] && level!=Level.MISS)
                {
                    if (Tracks[index] == Tracks[index - 1])
                    {
                        if (keyState == KeyState.PRESS && track == Tracks[index])
                        {
                            index++;
                            if (index == times.Count - 1)
                            {
                                state = State.OUT;
                            }
                            return Level.CONTINUE;
                        }
                        else
                        {
                            state = State.OUT;
                            level = Level.MISS;
                            return Level.MISS;
                        }
                    }
                    else
                    {
                        if (track == Tracks[index])
                        {
                            index++;
                            if (index == times.Count - 1)
                            {
                                state = State.OUT;
                            }
                            return Level.CONTINUE;
                        }
                        else
                        {
                            state = State.OUT;
                            level = Level.MISS;
                            return Level.MISS;
                        }
                    }
                }
                break;
            case State.SLIDE:
                break;
            case State.OUT:
                if(level == Level.MISS)
                {
                    return Level.MISS;
                }
                if (times[index-1] >= audioTime - 0.05f && times[index-1] <= audioTime + 0.05f)
                {

                    if (preLevel != Level.PREFECT)
                    {
                        level = preLevel;
                        return preLevel;
                    }
                    return Level.PREFECT;
                }
                else if (times[index-1] >= audioTime - 0.1f && times[index-1] <= audioTime + 0.1f)
                {

                    if (preLevel < Level.GOOD)
                    {
                        level = preLevel;
                        return preLevel;
                    }
                    return Level.GOOD;
                }
                else if (times[index-1] >= audioTime - 0.2f && times[index-1] <= audioTime + 0.2f)
                {

                    if (preLevel < Level.BAD)
                    {
                        level = preLevel;
                        return preLevel;
                    }
                    return Level.BAD;
                }
                else
                {
                    level = Level.MISS;
                    return Level.MISS;
                }

        }

        return Level.CONTINUE;
    }
    public override void needDestory(int track)
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
    public override void closePitch()
    {
        GetComponent<LineRenderer>().enabled = false;
    }

}
