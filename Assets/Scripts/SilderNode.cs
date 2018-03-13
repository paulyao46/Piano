using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilderNode : Node
{
    private List<int> nodeIndex;
    public GameObject plane;
    public Transform[] pos;
    private float startTime;
    private float endTime;
    
    void Start()
    {
        nodeIndex = new List<int>();
        nodeIndex.Add(0);
        nodeIndex.Add(1);
        nodeIndex.Add(2);
        nodeIndex.Add(3);
        nodeIndex.Add(4);
        nodeIndex.Add(4);
        show();
    }
    void show()
    {
        for(int i =0;i<6;i++)
        {
            Instantiate(plane, pos[nodeIndex[i]].position, pos[nodeIndex[i]].rotation);
        }
       
    }
    public override void determination(KeyState keyState,int track)
    {
       
    }
}
