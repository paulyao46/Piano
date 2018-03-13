using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text : MonoBehaviour
{
    float size;
    TextMesh com;
    // Use this for initialization
    void Start()
    {
        com = GetComponent<TextMesh>();
        size = com.characterSize;
    }

    // Update is called once per frame
    void Update()
    {

        if (com.characterSize < 0.11f)
            com.characterSize += 0.01f;
    }
    public void setText(string str, Color color)
    {
        com.text = str;
        com.characterSize = size;
        com.color = color;
    }
}
