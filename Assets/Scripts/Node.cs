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
    protected float time;
    protected int track;
    public State state;
    public Level level;
    // Use this for initialization
    void Start () {
        level = Level.MISS;

    }
	
	// Update is called once per frame
	public virtual void Update () {
        transform.Translate(0, 0, -Time.deltaTime * speed);
    }
    public virtual void needDestory()
    {
        Instantiate(particle, new Vector3(this.transform.position.x, this.transform.position.y+0.3f, this.transform.position.z), this.transform.rotation);
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
}
