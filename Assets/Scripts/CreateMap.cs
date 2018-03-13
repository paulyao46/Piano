using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreateMap : MonoBehaviour {
    public List<float> keyframe;
    public AudioSource audios;
    public GameObject mapobj;
	// Use this for initialization
	void Start () {
        audios = mapobj.GetComponent<AudioSource>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.E)|| Input.GetKeyDown(KeyCode.Q))
        {
            keyframe.Add(audios.time);
        }
    }
    public void writerFile()
    {
        FileStream file = new FileStream(@"C:\Users\paulyao\Desktop\key2.txt", FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(file);
        foreach (var s in keyframe)
        {
            sw.WriteLine(s);
        }
        sw.Close();
    }
}
