using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    IN,
    SLIDE,
    OUT,
    IDLE
}
public enum Level
{
    PREFECT,
    GOOD,
    BAD,
    MISS,
    CONTINUE,
    UNABLE
}

public class Node : MonoBehaviour {
    public float speed;
    public GameObject particle;
    public GameObject[] showParticlePos;
    protected float time;
    protected int track;
    public State state;
    protected Level level;
    // Use this for initialization
    void Start () {
        level = Level.MISS;

    }
	
	// Update is called once per frame
	public virtual void Update () {
        transform.Translate(0, 0, -Time.deltaTime * speed);
    }
    public virtual void needDestory(int track)
    {
        var obj = showParticlePos[track];
        Instantiate(particle, new Vector3(obj.transform.position.x, obj.transform.position.y+0.3f, obj.transform.position.z), obj.transform.rotation);
        Destroy(this.gameObject);
    }
    public virtual void missDestory()
    {
        Destroy(this.gameObject);
    }
    public virtual Level determination(KeyState keyState,int track,float audioTime)
    {
        return Level.UNABLE;
    }
    public virtual void SetTimeTrack(float timer,int tracks)
    {
        time = timer;
        track = tracks;
    }
    public float GetTime()
    {
        return time;
    }
    public int GetTrack()
    {
        return track;
    }
    public Level GetLevel()
    {
        return level;
    }
    public virtual void closePitch()
    {
        GetComponent<MeshRenderer>().enabled=false;
    }
}
