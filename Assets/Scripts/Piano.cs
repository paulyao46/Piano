using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Playtype
{
    KEYBOARD,
    DEVICE
}
public enum KeyState
{
    press,
    idle
}
public class Piano : MonoBehaviour
{
    private MeshRenderer[] mesh;
    public text showLevel;
    public Score score;
    public AudioSource audios;
    private GyroObj protocol;
    private List<float>[] timeOfTrack;
    public Material[] materials;
    private KEY[] key;
    public GameObject[] keyObjs;
    public GameObject[] tracks;
    private TextMesh[] text;
    private List<int> trackIndex;
    public List<Node>[] Nodes;
    public Playtype type;
    // Use this for initialization
    void Start()
    {

        protocol = GameObject.Find("rs232obj").GetComponent<GyroObj>();
        mesh = new MeshRenderer[20];
        text = new TextMesh[10];
        timeOfTrack = new List<float>[5];
        Nodes = new List<Node>[5];
        trackIndex = new List<int>();
        for (int i = 0; i < keyObjs.Length; i++)
        {
            mesh[i] = keyObjs[i].GetComponent<MeshRenderer>();
            text[i] = keyObjs[i].transform.GetChild(0).GetComponent<TextMesh>();
            mesh[i + 10] = tracks[i].GetComponent<MeshRenderer>();
        }
        for (int i = 0; i < timeOfTrack.Length; i++)
        {
            timeOfTrack[i] = new List<float>();
            Nodes[i] = new List<Node>();
            trackIndex.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkPitch();
        switch (type)
        {
            case Playtype.DEVICE:
                key = protocol.GetKeysState();
                for (int i = 0; i < keyObjs.Length; i++)
                {
                    if (key[i].isPress == 1)
                    {
                        mesh[i].material = materials[1];
                        determination(i);
                        mesh[i + 10].material = materials[1];
                        text[i].text = key[i].proximity.ToString();
                    }
                    else
                    {
                        mesh[i].material = materials[0];
                        mesh[i + 10].material = materials[2];
                        text[i].text = key[i].proximity.ToString();
                    }
                }
                break;
            case Playtype.KEYBOARD:
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    mesh[0].material = materials[1];
                    mesh[10].material = materials[1];
                    determination(0);
                }
                if (Input.GetKeyUp(KeyCode.Q))
                {
                    mesh[0].material = materials[0];
                    mesh[10].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    mesh[1].material = materials[1];
                    mesh[11].material = materials[1];
                    determination(1);
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    mesh[1].material = materials[0];
                    mesh[11].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    mesh[2].material = materials[1];
                    mesh[12].material = materials[1];
                    determination(2);
                }
                if (Input.GetKeyUp(KeyCode.E))
                {
                    mesh[2].material = materials[0];
                    mesh[12].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    mesh[3].material = materials[1];
                    mesh[13].material = materials[1];
                    determination(3);
                }
                if (Input.GetKeyUp(KeyCode.R))
                {
                    mesh[3].material = materials[0];
                    mesh[13].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.T))
                {
                    mesh[4].material = materials[1];
                    mesh[14].material = materials[1];
                }
                if (Input.GetKeyUp(KeyCode.T))
                {
                    mesh[4].material = materials[0];
                    mesh[14].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    mesh[5].material = materials[1];
                    mesh[15].material = materials[1];
                }
                if (Input.GetKeyUp(KeyCode.Y))
                {
                    mesh[5].material = materials[0];
                    mesh[15].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    mesh[6].material = materials[1];
                    mesh[16].material = materials[1];
                }
                if (Input.GetKeyUp(KeyCode.U))
                {
                    mesh[6].material = materials[0];
                    mesh[16].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    mesh[7].material = materials[1];
                    mesh[17].material = materials[1];
                }
                if (Input.GetKeyUp(KeyCode.I))
                {
                    mesh[7].material = materials[0];
                    mesh[17].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    mesh[8].material = materials[1];
                    mesh[18].material = materials[1];
                }
                if (Input.GetKeyUp(KeyCode.O))
                {
                    mesh[8].material = materials[0];
                    mesh[18].material = materials[2];
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    mesh[9].material = materials[1];
                    mesh[19].material = materials[1];
                }
                if (Input.GetKeyUp(KeyCode.P))
                {
                    mesh[9].material = materials[0];
                    mesh[19].material = materials[2];
                }

                break;
        }
    }
    public void setTimeOfTrack(int track, float time,Node node)
    {
        timeOfTrack[track].Add(time);
        Nodes[track].Add(node);
    }
    private void determination(int track)
    {
        if (track < 5&& timeOfTrack[track].Count != 0 && trackIndex[track] < timeOfTrack[track].Count )
        {
            var time = timeOfTrack[track][trackIndex[track]];
            time += 0.17f;
            var audioTime = audios.time;

            if (time >= audioTime - 0.05f && time <= audioTime + 0.05f)
            {
                showLevel.setText("Prefect",Color.cyan);
                score.setCombo();
                Nodes[track][trackIndex[track]].needDestory();
                trackIndex[track] += 1;
            }
            else if (time >= audioTime - 0.1f && time <= audioTime + 0.1f)
            {
                showLevel.setText("Good",Color.green);
                score.setCombo();
                Nodes[track][trackIndex[track]].needDestory();
                trackIndex[track] += 1;
            }
            else if (time >= audioTime - 0.2f && time <= audioTime + 0.2f)
            {
                showLevel.setText("Bad",Color.yellow);
                score.setCombo();
                Nodes[track][trackIndex[track]].needDestory();
                trackIndex[track] += 1;
            }
        }

    }
    void checkPitch()
    {
        var audioTime = audios.time;
        for (int i = 0; i < 5; i++)
        {
            if (timeOfTrack[i].Count != 0 && trackIndex[i] < timeOfTrack[i].Count)
            {
                if (timeOfTrack[i][trackIndex[i]]+0.17f < audioTime - 0.2f)
                {
                    Debug.Log("Miss");
                    showLevel.setText("Miss",Color.red);
                    score.resetCombo();

                    Nodes[i][trackIndex[i]].missDestory();
                    trackIndex[i] += 1;
                }
            }

        }
    }
}
