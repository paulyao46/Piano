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
        checkKeyState();
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
                                break;
                            case KeyState.INPRESS:
                                keyState[i] = KeyState.PRESS;
                                break;
                            case KeyState.PRESS:
                                break;
                            case KeyState.OUTPRESS:
                                keyState[i] = KeyState.INPRESS;
                                mesh[i].material = materials[1];
                                //mesh[i+10].material = materials[1];
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
                    KeyboardCtrlIn(0);

                }
                else
                {
                    KeyboardCtrlOut(0);
                }
                if (Input.GetKey(KeyCode.W))
                {
                    KeyboardCtrlIn(1);

                }
                else
                {
                    KeyboardCtrlOut(1);
                }
                if (Input.GetKey(KeyCode.E))
                {
                    KeyboardCtrlIn(2);

                }
                else
                {
                    KeyboardCtrlOut(2);
                }
                if (Input.GetKey(KeyCode.R))
                {
                    KeyboardCtrlIn(3);

                }
                else
                {
                    KeyboardCtrlOut(3);
                }
                if (Input.GetKey(KeyCode.T))
                {
                    KeyboardCtrlIn(4);

                }
                else
                {
                    KeyboardCtrlOut(4);
                }
                if (Input.GetKey(KeyCode.Y))
                {
                    KeyboardCtrlIn(5);

                }
                else
                {
                    KeyboardCtrlOut(5);
                }
                if (Input.GetKey(KeyCode.U))
                {
                    KeyboardCtrlIn(6);

                }
                else
                {
                    KeyboardCtrlOut(6);
                }
                if (Input.GetKey(KeyCode.I))
                {
                    KeyboardCtrlIn(7);

                }
                else
                {
                    KeyboardCtrlOut(7);
                }
                if (Input.GetKey(KeyCode.O))
                {
                    KeyboardCtrlIn(8);

                }
                else
                {
                    KeyboardCtrlOut(8);
                }
                if (Input.GetKey(KeyCode.P))
                {
                    KeyboardCtrlIn(9);

                }
                else
                {
                    KeyboardCtrlOut(9);
                }
                break;
        }
    }
    public void setTimeOfTrack(int track, float time, Node node)
    {
        timeOfTrack[track].Add(time);
        Nodes[track].Add(node);
    }
    private void checkKeyState()
    {
        for (int track = 0; track < 10; track++)
        {
            KeyState state = keyState[track];
            determination(track, state);
        }
    }
    private void determination(int track, KeyState state)
    {
        if (timeOfTrack[track].Count != 0 && trackIndex[track] < timeOfTrack[track].Count)
        {
            var audioTime = audios.time;
            var level = Nodes[track][trackIndex[track]].determination(state, track, audioTime);
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
                    break;
                case Level.CONTINUE:
                    break;
                case Level.UNABLE:
                    break;
            }
            if (inAttack == true)
            {
                Nodes[track][trackIndex[track]].closePitch();
                trackIndex[track] += 1;

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
                        Nodes[track][checkIndex[track]].needDestory(track);
                        checkIndex[track] += 1;
                    }
                }
                if (level == Level.MISS && timeOfTrack[track][checkIndex[track]] < audioTime - 0.2f)
                {
                    showLevel.setText("Miss", Color.red);
                    score.resetCombo();

                    Nodes[track][checkIndex[track]].missDestory();
                    checkIndex[track] += 1;
                    trackIndex[track] += 1;

                }



            }

        }
    }
    void KeyboardCtrlIn(int index)
    {
        switch (keyState[index])
        {
            case KeyState.NOPRESS:
                keyState[index] = KeyState.INPRESS;
                mesh[index].material = materials[1];
                mesh[index + 10].material = materials[1];
                break;
            case KeyState.INPRESS:
                keyState[0] = KeyState.PRESS;
                break;
            case KeyState.PRESS:
                break;
            case KeyState.OUTPRESS:
                keyState[index] = KeyState.INPRESS;
                mesh[index].material = materials[1];
                mesh[index + 10].material = materials[1];
                break;
        }
    }
    void KeyboardCtrlOut(int index)
    {
        switch (keyState[index])
        {
            case KeyState.INPRESS:
                keyState[index] = KeyState.OUTPRESS;
                mesh[index].material = materials[0];
                mesh[index + 10].material = materials[2];
                break;
            case KeyState.PRESS:
                keyState[1] = KeyState.OUTPRESS;
                mesh[index].material = materials[0];
                mesh[index + 10].material = materials[2];
                break;
            case KeyState.OUTPRESS:
                keyState[index] = KeyState.NOPRESS;
                break;
        }
    }
    public KeyState GetKeyState(int num)
    {
        return keyState[num];
    }
}
