using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    IN,
    OUT,
    SLIDE
}
public class Node : MonoBehaviour {
    public float speed;
    public GameObject particle;
    private float time;
    State state;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, -Time.deltaTime * speed);
    }
    public void needDestory()
    {
        Instantiate(particle, new Vector3(this.transform.position.x, this.transform.position.y+0.3f, this.transform.position.z), this.transform.rotation);
        Destroy(this.gameObject);
    }
    public void missDestory()
    {
        Destroy(this.gameObject);
    }
    public virtual void determination(KeyState keyState,int track)
    {

    }
}
