using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.x>0.56f)
        transform.Translate(new Vector3(-0.007f, 0, 0));
	}
}
