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
    private int nowTrack;
    private bool startCheck;
    public GameObject Mask;
    private Piano piano;
    public GameObject particles;
    void Start()
    {
        Mask = GameObject.Find("Mask");
        Mask.SetActive(false);
        piano = GameObject.Find("Piano").GetComponent<Piano>();
        preLevel = Level.MISS;
        level = Level.MISS;
        state = State.IDLE;
        index = 0;
        startCheck = false;
        for (int i = 0; i < 6; i++)
        {
            times.Add(time + 1.0f * i);

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
        if (startCheck)
        {
            checkKey(nowTrack);
        }
    }
    public override Level determination(KeyState keyState, int track, float audioTime)
    {
        switch (state)
        {
            case State.IDLE:
                if (time >= audioTime - 0.05f && time <= audioTime + 0.05f)
                {
                    particles = Instantiate(particles, showParticlePos[9].transform.position, showParticlePos[9].transform.rotation);
                    startCheck = true;
                    nowTrack = track;
                    Mask.SetActive(true);
                    index++;
                    state = State.IN;
                    preLevel = Level.PREFECT;
                    level = Level.CONTINUE;
                    return Level.CONTINUE;
                }
                else if (time >= audioTime - 0.1f && time <= audioTime + 0.1f)
                {
                    particles = Instantiate(particles, showParticlePos[9].transform.position, showParticlePos[9].transform.rotation);
                    startCheck = true;
                    nowTrack = track;
                    Mask.SetActive(true);
                    index++;
                    state = State.IN;
                    preLevel = Level.GOOD;
                    level = Level.CONTINUE;
                    return Level.CONTINUE;
                }
                else if (time >= audioTime - 0.2f && time <= audioTime + 0.2f)
                {
                    particles = Instantiate(particles, showParticlePos[9].transform.position, showParticlePos[9].transform.rotation);
                    startCheck = true;
                    nowTrack = track;
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
                if (index < times.Count && audioTime >= times[index] - 0.1f && audioTime <= times[index] + 0.1f && level != Level.MISS)
                {
                    index++;
                    nowTrack = Tracks[index];
                    if (index == times.Count - 1)
                    {
                        state = State.OUT;
                    }
                    return Level.CONTINUE;

                }
                break;
            case State.SLIDE:
                break;
            case State.OUT:
                if (index < times.Count && audioTime >= times[index] - 0.2f && audioTime <= times[index] + 0.3f && level != Level.MISS)
                {
                    if (keyState == KeyState.PRESS)
                    {
                        return Level.CONTINUE;
                    }

                    if (track == Tracks[index])
                    {
                        if (times[index] >= audioTime - 0.05f && times[index] <= audioTime + 0.05f)
                        {

                            if (preLevel != Level.PREFECT)
                            {
                                level = preLevel;
                                return preLevel;
                            }
                            return Level.PREFECT;
                        }
                        else if (times[index] >= audioTime - 0.1f && times[index] <= audioTime + 0.1f)
                        {

                            if (preLevel < Level.GOOD)
                            {
                                level = preLevel;
                                return preLevel;
                            }
                            return Level.GOOD;
                        }
                        else if (times[index] >= audioTime - 0.2f && times[index] <= audioTime + 0.2f)
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
                }
                break;
        }
        return Level.CONTINUE;
    }
    public override void needDestory(int track)
    {
        Destroy(particles);
        Destroy(this.gameObject);

    }
    public override void missDestory()
    {
        Destroy(particles);
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
    KeyState checkKey(int track)
    {
        var sta = piano.GetKeyState(track);
        if (sta == KeyState.PRESS || sta == KeyState.INPRESS)
        {

        }
        else
        {
            level = Level.MISS;
            startCheck = false;
        }

        return sta;
    }

}
