using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(del());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator del()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
