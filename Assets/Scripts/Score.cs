using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {
    private int combo;
    private TextMesh text;
	// Use this for initialization
	void Start () {
        combo = 0;
        text = GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void setCombo()
    {
        combo += 1;
        text.text = combo.ToString();
    }
    public void resetCombo()
    {
        combo = 0;
        text.text = " ";
    }

}
