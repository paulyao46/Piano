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
    NOPRESS,
    INPRESS,
    PRESS,
    OUTPRESS
}
public class Piano : MonoBehaviour
{
    public text showLevel;
    public Score score;
    public AudioSource audios;
    public MusicMap mMap;
    public Material[] materials;
    public GameObject[] keyObjs;
    public GameObject[] tracks;
    public List<Node>[] Nodes;
    public Playtype type;
    private MeshRenderer[] mesh;
    private GyroObj protocol;
    private List<float>[] timeOfTrack;
    private KEY[] key;
    private TextMesh[] text;
    private List<int> trackIndex;
    private List<int> checkIndex;
    private KeyState[] keyState;
    // Use this for initialization
    void Start()
    {

        protocol = GameObject.Find("rs232obj").GetComponent<GyroObj>();
        mesh = new MeshRenderer[20];
        text = new TextMesh[10];
        timeOfTrack = new List<float>[10];
        Nodes = new List<Node>[10];
        keyState = new KeyState[10];
        for (int i = 0; i < 10; i++)
        {
            keyState[i] = KeyState.NOPRESS;
        }
        trackIndex = new List<int>();
        checkIndex = new List<int>();
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
            checkIndex.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        Control();
        checkPitch();
    }
    void Control()
    {
        switch (type)
        {
            case Playtype.DEVICE:
                key = protocol.GetKeysState();
                for (int i = 0; i < keyObjs.Length; i++)
                {
                    if (key[i].isPress == 1)
                    {
                        switch (keyState[i])
                        {
                            case KeyState.NOPRESS:
                                keyState[i] = KeyState.INPRESS;
                                mesh[i].material = materials[1];
                                //mesh[i+10].material = materials[1];
                                text[i].text = key[i].proximity.ToString();
                                determination(i);
                                break;
                            case KeyState.INPRESS:
                                keyState[i] = KeyState.PRESS;
                                determination(i);
                                break;
                            case KeyState.PRESS:
                                determination(i);
                                break;
                            case KeyState.OUTPRESS:
                                keyState[i] = KeyState.INPRESS;
                                mesh[i].material = materials[1];
                                //mesh[i+10].material = materials[1];
                                determination(i);
                                break;
                        }
                    }
                    else
                    {
                        switch (keyState[i])
                        {
                            case KeyState.INPRESS:
                                keyState[i] = KeyState.OUTPRESS;
                                mesh[i].material = materials[0];
                                //mesh[i+10].material = materials[2];
                                text[i].text = key[i].proximity.ToString();
                                break;
                            case KeyState.PRESS:
                                keyState[i] = KeyState.OUTPRESS;
                                mesh[i].material = materials[0];
                                //mesh[i+10].material = materials[2];
                                text[i].text = key[i].proximity.ToString();
                                break;
                            case KeyState.OUTPRESS:
                                keyState[i] = KeyState.NOPRESS;
                                text[i].text = key[i].proximity.ToString();
                                break;
                        }
                        
                    }
                }
                break;
            case Playtype.KEYBOARD:
                if (Input.GetKey(KeyCode.Q))
                {
                    switch (keyState[0])
                    {
                        case KeyState.NOPRESS:
                            keyState[0] = KeyState.INPRESS;
                            mesh[0].material = materials[1];
                            mesh[10].material = materials[1];
                            determination(0);
                            break;
                        case KeyState.INPRESS:
                            keyState[0] = KeyState.PRESS;
                            determination(0);
                            break;
                        case KeyState.PRESS:
                            determination(0);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[0] = KeyState.INPRESS;
                            mesh[0].material = materials[1];
                            mesh[10].material = materials[1];
                            determination(0);
                            break;
                    }

                }
                else
                {
                    switch (keyState[0])
                    {
                        case KeyState.INPRESS:
                            keyState[0] = KeyState.OUTPRESS;
                            mesh[0].material = materials[0];
                            mesh[10].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[0] = KeyState.OUTPRESS;
                            mesh[0].material = materials[0];
                            mesh[10].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[0] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.W))
                {
                    switch (keyState[1])
                    {
                        case KeyState.NOPRESS:
                            keyState[1] = KeyState.INPRESS;
                            mesh[1].material = materials[1];
                            mesh[11].material = materials[1];
                            determination(1);
                            break;
                        case KeyState.INPRESS:
                            keyState[1] = KeyState.PRESS;
                            determination(1);
                            break;
                        case KeyState.PRESS:
                            determination(1);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[1] = KeyState.INPRESS;
                            mesh[1].material = materials[1];
                            mesh[11].material = materials[1];
                            determination(1);
                            break;
                    }

                }
                else
                {
                    switch (keyState[1])
                    {
                        case KeyState.INPRESS:
                            keyState[1] = KeyState.OUTPRESS;
                            mesh[1].material = materials[0];
                            mesh[11].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[1] = KeyState.OUTPRESS;
                            mesh[1].material = materials[0];
                            mesh[11].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[1] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.E))
                {
                    switch (keyState[2])
                    {
                        case KeyState.NOPRESS:
                            keyState[2] = KeyState.INPRESS;
                            mesh[2].material = materials[1];
                            mesh[12].material = materials[1];
                            determination(2);
                            break;
                        case KeyState.INPRESS:
                            keyState[2] = KeyState.PRESS;
                            determination(2);
                            break;
                        case KeyState.PRESS:
                            determination(2);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[2] = KeyState.INPRESS;
                            mesh[2].material = materials[1];
                            mesh[12].material = materials[1];
                            determination(2);
                            break;
                    }

                }
                else
                {
                    switch (keyState[2])
                    {
                        case KeyState.INPRESS:
                            keyState[2] = KeyState.OUTPRESS;
                            mesh[2].material = materials[0];
                            mesh[12].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[2] = KeyState.OUTPRESS;
                            mesh[2].material = materials[0];
                            mesh[12].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[2] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.R))
                {
                    switch (keyState[3])
                    {
                        case KeyState.NOPRESS:
                            keyState[3] = KeyState.INPRESS;
                            mesh[3].material = materials[1];
                            mesh[13].material = materials[1];
                            determination(3);
                            break;
                        case KeyState.INPRESS:
                            keyState[3] = KeyState.PRESS;
                            determination(3);
                            break;
                        case KeyState.PRESS:
                            determination(3);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[3] = KeyState.INPRESS;
                            mesh[3].material = materials[1];
                            mesh[13].material = materials[1];
                            determination(3);
                            break;
                    }

                }
                else
                {
                    switch (keyState[3])
                    {
                        case KeyState.INPRESS:
                            keyState[3] = KeyState.OUTPRESS;
                            mesh[3].material = materials[0];
                            mesh[13].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[3] = KeyState.OUTPRESS;
                            mesh[3].material = materials[0];
                            mesh[13].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[3] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.T))
                {
                    switch (keyState[4])
                    {
                        case KeyState.NOPRESS:
                            keyState[4] = KeyState.INPRESS;
                            mesh[4].material = materials[1];
                            mesh[14].material = materials[1];
                            determination(4);
                            break;
                        case KeyState.INPRESS:
                            keyState[4] = KeyState.PRESS;
                            determination(4);
                            break;
                        case KeyState.PRESS:
                            determination(4);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[4] = KeyState.INPRESS;
                            mesh[4].material = materials[1];
                            mesh[14].material = materials[1];
                            determination(4);
                            break;
                    }

                }
                else
                {
                    switch (keyState[4])
                    {
                        case KeyState.INPRESS:
                            keyState[4] = KeyState.OUTPRESS;
                            mesh[4].material = materials[0];
                            mesh[14].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[4] = KeyState.OUTPRESS;
                            mesh[4].material = materials[0];
                            mesh[14].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[4] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.Y))
                {
                    switch (keyState[5])
                    {
                        case KeyState.NOPRESS:
                            keyState[5] = KeyState.INPRESS;
                            mesh[5].material = materials[1];
                            mesh[15].material = materials[1];
                            determination(5);
                            break;
                        case KeyState.INPRESS:
                            keyState[5] = KeyState.PRESS;
                            determination(5);
                            break;
                        case KeyState.PRESS:
                            determination(5);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[5] = KeyState.INPRESS;
                            mesh[5].material = materials[1];
                            mesh[15].material = materials[1];
                            determination(5);
                            break;
                    }

                }
                else
                {
                    switch (keyState[5])
                    {
                        case KeyState.INPRESS:
                            keyState[5] = KeyState.OUTPRESS;
                            mesh[5].material = materials[0];
                            mesh[15].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[5] = KeyState.OUTPRESS;
                            mesh[5].material = materials[0];
                            mesh[15].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[5] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.U))
                {
                    switch (keyState[6])
                    {
                        case KeyState.NOPRESS:
                            keyState[6] = KeyState.INPRESS;
                            //mesh[6].material = materials[1];
                            //mesh[16].material = materials[1];
                            determination(6);
                            break;
                        case KeyState.INPRESS:
                            keyState[6] = KeyState.PRESS;
                            determination(6);
                            break;
                        case KeyState.PRESS:
                            determination(6);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[6] = KeyState.INPRESS;
                            //mesh[6].material = materials[1];
                            //mesh[16].material = materials[1];
                            determination(6);
                            break;
                    }

                }
                else
                {
                    switch (keyState[6])
                    {
                        case KeyState.INPRESS:
                            keyState[6] = KeyState.OUTPRESS;
                            mesh[6].material = materials[0];
                            mesh[16].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[6] = KeyState.OUTPRESS;
                            mesh[6].material = materials[0];
                            mesh[16].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[6] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.I))
                {
                    switch (keyState[7])
                    {
                        case KeyState.NOPRESS:
                            keyState[7] = KeyState.INPRESS;
                            //mesh[7].material = materials[1];
                            //mesh[17].material = materials[1];
                            determination(7);
                            break;
                        case KeyState.INPRESS:
                            keyState[7] = KeyState.PRESS;
                            determination(7);
                            break;
                        case KeyState.PRESS:
                            determination(7);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[7] = KeyState.INPRESS;
                            //mesh[7].material = materials[1];
                            //mesh[17].material = materials[1];
                            determination(7);
                            break;
                    }

                }
                else
                {
                    switch (keyState[7])
                    {
                        case KeyState.INPRESS:
                            keyState[7] = KeyState.OUTPRESS;
                            mesh[7].material = materials[0];
                            mesh[17].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[7] = KeyState.OUTPRESS;
                            mesh[7].material = materials[0];
                            mesh[17].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[7] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.O))
                {
                    switch (keyState[8])
                    {
                        case KeyState.NOPRESS:
                            keyState[8] = KeyState.INPRESS;
                            //mesh[8].material = materials[1];
                            //mesh[18].material = materials[1];
                            determination(8);
                            break;
                        case KeyState.INPRESS:
                            keyState[8] = KeyState.PRESS;
                            determination(8);
                            break;
                        case KeyState.PRESS:
                            determination(8);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[8] = KeyState.INPRESS;
                            //mesh[8].material = materials[1];
                            //mesh[18].material = materials[1];
                            determination(8);
                            break;
                    }

                }
                else
                {
                    switch (keyState[8])
                    {
                        case KeyState.INPRESS:
                            keyState[8] = KeyState.OUTPRESS;
                            mesh[8].material = materials[0];
                            mesh[18].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[8] = KeyState.OUTPRESS;
                            mesh[8].material = materials[0];
                            mesh[18].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[8] = KeyState.NOPRESS;
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.P))
                {
                    switch (keyState[9])
                    {
                        case KeyState.NOPRESS:
                            keyState[9] = KeyState.INPRESS;
                            //mesh[9].material = materials[1];
                            //mesh[19].material = materials[1];
                            determination(9);
                            break;
                        case KeyState.INPRESS:
                            keyState[9] = KeyState.PRESS;
                            determination(9);
                            break;
                        case KeyState.PRESS:
                            determination(9);
                            break;
                        case KeyState.OUTPRESS:
                            keyState[9] = KeyState.INPRESS;
                            //mesh[9].material = materials[1];
                            //mesh[19].material = materials[1];
                            determination(9);
                            break;
                    }

                }
                else
                {
                    switch (keyState[9])
                    {
                        case KeyState.INPRESS:
                            keyState[9] = KeyState.OUTPRESS;
                            mesh[9].material = materials[0];
                            mesh[19].material = materials[2];
                            break;
                        case KeyState.PRESS:
                            keyState[9] = KeyState.OUTPRESS;
                            mesh[9].material = materials[0];
                            mesh[19].material = materials[2];
                            break;
                        case KeyState.OUTPRESS:
                            keyState[9] = KeyState.NOPRESS;
                            break;
                    }
                }
                break;
        }
    }
    public void setTimeOfTrack(int track, float time, Node node)
    {
        timeOfTrack[track].Add(time);
        Nodes[track].Add(node);
    }
    private void determination(int track)
    {
        if (timeOfTrack[track].Count != 0 && trackIndex[track] < timeOfTrack[track].Count)
        {
            var audioTime = audios.time;
            var level = Nodes[track][trackIndex[track]].determination(keyState[track], track, audioTime);
            var inAttack = false;
            switch (level)
            {
                case Level.PREFECT:
                case Level.GOOD:
                case Level.BAD:
                    inAttack = true;
                    break;
                case Level.MISS:
                    Nodes[track][trackIndex[track]].closePitch();
                    Debug.Log("closePitch");
                    break;
                case Level.CONTINUE:
                    break;
                case Level.UNABLE:
                    break;
            }
            if (inAttack == true)
            {
                if(track ==6)
                {
                    trackIndex[track] += 2;
                    trackIndex[track + 1] += 1;
                    trackIndex[track + 2] += 1;
                    trackIndex[track + 3] += 2;
                    Debug.Log("success det");
                }
                else
                {
                    Nodes[track][trackIndex[track]].closePitch();
                    trackIndex[track] += 1;
                }
                
            }


        }

    }
    void checkPitch()
    {
        var audioTime = audios.time;

        for (int track = 0; track < 10; track++)
        {
            if (timeOfTrack[track].Count != 0 && checkIndex[track] < timeOfTrack[track].Count)
            {
                var level = Nodes[track][checkIndex[track]].GetLevel();
                if (timeOfTrack[track][checkIndex[track]] >= audioTime - 0.2f && timeOfTrack[track][checkIndex[track]] <= audioTime + 0.2)
                {
                   
                    var inAttack = false;
                    switch (level)
                    {
                        case Level.PREFECT:
                            showLevel.setText("Prefect", Color.cyan);
                            inAttack = true;
                            break;
                        case Level.GOOD:
                            showLevel.setText("Good", Color.green);
                            inAttack = true;
                            break;
                        case Level.BAD:
                            showLevel.setText("Bad", Color.yellow);
                            inAttack = true;
                            break;
                        case Level.CONTINUE:
                            break;
                        case Level.MISS:
                            break;
                        case Level.UNABLE:
                            break;
                    }
                    if (inAttack == true)
                    {
                        score.setCombo();
                        if (track == 6)
                        {
                            Nodes[track][checkIndex[track]].needDestory(track);
                            checkIndex[track] += 2;
                            checkIndex[track + 1] += 1;
                            checkIndex[track + 2] += 1;
                            checkIndex[track + 3] += 2;
                            Debug.Log("success chk");

                        }
                        else
                        {
                            try
                            {
                                Nodes[track][checkIndex[track]].needDestory(track);
                            }
                            catch
                            {
                                Debug.Log("sometgin");
                            }
                            checkIndex[track] += 1;
                        }
                        
                    }
                }
                if(level ==Level.CONTINUE)
                {
                    var time = Nodes[track][checkIndex[track]].GetTime();
                    if (time < audioTime - 6.2f)
                    {
                        level = Level.MISS;
                    }
                }
                if (level ==Level.MISS && timeOfTrack[track][checkIndex[track]] < audioTime - 0.2f)
                {
                    showLevel.setText("Miss", Color.red);
                    score.resetCombo();

                    try
                    {
                        if(track==6)
                        {
                            Nodes[track][checkIndex[track]].missDestory();
                            checkIndex[track] += 2;
                            checkIndex[track + 1] += 1;
                            checkIndex[track + 2] += 1;
                            checkIndex[track + 3] += 2;
                            trackIndex[track] += 2;
                            trackIndex[track +1] += 1;
                            trackIndex[track + 2] += 1;
                            trackIndex[track+3] += 2;
                            Debug.Log("6");
                        }
                        else if(track == 7)
                        {
                            Nodes[track][checkIndex[track]].missDestory();
                            checkIndex[track] += 1;
                            checkIndex[track + 1] += 1;
                            checkIndex[track + 2] += 2;
                            checkIndex[track -1] += 2;
                            trackIndex[track] += 1;
                            trackIndex[track + 1] += 1;
                            trackIndex[track + 2] += 2;
                            trackIndex[track -1] += 2;
                            Debug.Log("7");
                        }
                        else if(track == 8)
                        {
                            Nodes[track][checkIndex[track]].missDestory();
                            checkIndex[track] += 1;
                            checkIndex[track + 1] += 2;
                            checkIndex[track -1] += 1;
                            checkIndex[track - 2] += 2;
                            trackIndex[track] += 1;
                            trackIndex[track + 1] += 2;
                            trackIndex[track -1] += 1;
                            trackIndex[track -2] += 2;
                            Debug.Log("8");
                        }
                        else if(track == 9)
                        {
                            Nodes[track][checkIndex[track]].missDestory();
                            checkIndex[track] += 2;
                            checkIndex[track -1] += 1;
                            checkIndex[track - 2] += 1;
                            checkIndex[track - 3] += 2;
                            trackIndex[track] += 2;
                            trackIndex[track - 1] += 1;
                            trackIndex[track - 2] += 1;
                            trackIndex[track - 3] += 2;
                            Debug.Log("9");
                        }
                        else
                        {
                            Nodes[track][checkIndex[track]].missDestory();
                            checkIndex[track] += 1;
                            trackIndex[track] += 1;
                            Debug.Log("10");
                        }
                        
                    }
                    catch
                    {
                        Debug.Log("sometgin");
                    }
                    
                }



            }

        }
    }
}
