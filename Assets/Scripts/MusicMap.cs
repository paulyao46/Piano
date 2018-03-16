using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NoteDatas
{
    public int Tick;
    public int ContTick;
    public int Param1;

}
[System.Serializable]
public class bands
{
    public List<ChangeBPM> ChangeBPM;
    public List<Track> Track;
}
[System.Serializable]
public class ChangeBPM
{
    public int Tick;
    public int Tempo;
    public float TimePerTick;
}
[System.Serializable]
public class Track
{
    public int TrackID;
    public List<NoteDatas> NoteData;
    public Track()
    {
        NoteData = new List<NoteDatas>();
    }
}
public class MusicMap : MonoBehaviour
{
    public GameObject[] nodes;
    public GameObject slideCover;
    public GameObject[] createPos;
    public Queue<float>[] createTime;
    public Queue<int>[] createInfo;
    public Transform start;
    public Transform end;
    public Piano piano;
    public float delayTime;
    private bool startLoad;
    private string path;
    private AudioSource audios;
    private float dist;
    private float[] timer;
    private bool[] isCreate;
    private bool[] fristIn;
    private bool isPlay;
    private float totalTime;
    public float clock;
    // Use this for initialization
    void Start()
    {
        createTime = new Queue<float>[5];
        createInfo = new Queue<int>[5];
        timer = new float[5];
        isCreate = new bool[5];
        fristIn = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            isCreate[i] = false;
            fristIn[i] = true;
            createTime[i] = new Queue<float>();
            createInfo[i] = new Queue<int>();
        }
        isPlay = false;
        startLoad = false;
        clock = 0;
        dist = Vector3.Distance(start.position, end.position);
        audios = GetComponent<AudioSource>();
        path = Application.dataPath + "/Resources/superwings.json";
        StreamReader sr = new StreamReader(path);
        string json = sr.ReadToEnd();
        bands band = new bands();
        band.ChangeBPM = new List<ChangeBPM>();
        band.Track = new List<Track>();
        band = JsonUtility.FromJson<bands>(json);
        var timePerTick = band.ChangeBPM[0].TimePerTick;
        for (int i = 1; i <= 5; i++)
        {
            for (int j = 0; j < band.Track[i].NoteData.Capacity; j++)
            {
                createTime[i - 1].Enqueue((float)band.Track[i].NoteData[j].Tick * timePerTick);
                createInfo[i - 1].Enqueue(band.Track[i].NoteData[j].Param1);
            }

        }
        StartCoroutine(setLoadTime());
    }

    // Update is called once per frame
    void Update()
    {
        if(startLoad)
        {
            loadJasonMusicMap();
            PlayMusic();
        }
    }
    void loadJasonMusicMap()
    {
        if (!audios.isPlaying)
        {
            totalTime = dist / 5;

        }
        for (int i = 0; i < 5; i++)
        {
            if (!isCreate[i])
            {
                if (createTime[i].Count != 0)
                {
                    timer[i] = createTime[i].Dequeue();
                    isCreate[i] = true;
                }
            }
            else
            {
                if (timer[i]+ delayTime - totalTime < audios.time && audios.isPlaying)
                {
                    CreateNode(i);
                }
                else if(timer[i]+ delayTime - totalTime < audios.time)
                {
                    var tempTime = timer[i] + delayTime - totalTime;
                    if(tempTime < clock&& fristIn[i])
                    {
                        clock = tempTime;
                    }
                    
                    if (timer[i] + delayTime - totalTime <= clock+0.00001 && !fristIn[i])
                    {
                        CreateNode(i);
                    }
                    fristIn[i] = false;
                }

            }
        }
    }
    void PlayMusic()
    {
        if(clock>0&& !audios.isPlaying&& !isPlay)
        {
            audios.Play();
            isPlay = true;
        }
        else 
        {
            clock += Time.deltaTime; 
        }
    }
    IEnumerator setLoadTime()
    {
        yield return new WaitForSeconds(1);
        startLoad = true;
    }
    void CreateNode(int i)
    {
        switch (createInfo[i].Dequeue())//Node Type
        {
            case 0:
                var obj = Instantiate(nodes[0], createPos[i].transform.position, createPos[i].transform.rotation);
                var comp = obj.GetComponent<Node>();
                comp.SetType(KeyState.INPRESS);
                comp.SetTimeTrack(timer[i] + delayTime, i);
                piano.setTimeOfTrack(i, timer[i] + delayTime, comp);
                break;
            case 1:
                Instantiate(slideCover, createPos[10].transform.position, createPos[10].transform.rotation);
                var obj1 = Instantiate(nodes[3], createPos[9].transform.position, createPos[9].transform.rotation);
                var comp1 = obj1.GetComponent<SilderNode>();
                comp1.SetType(KeyState.INPRESS);
                comp1.SetTimeTrack(timer[i] + delayTime, 9);
                piano.setTimeOfTrack(9, timer[i] + delayTime, comp1);

                var tempObj = genSildeNode(3, 9, 0, 0, 4.72f);
                var tempComp = tempObj.GetComponent<SilderNode>();
                tempComp.SetType(KeyState.PRESS);
                comp1.NextNode = tempComp;
                piano.setTimeOfTrack(9, timer[i] + delayTime + 1.0f, tempComp);

                tempObj = genSildeNode(3, 8, 0, 0, 4.72f*2);
                comp1 = tempObj.GetComponent<SilderNode>();
                comp1.SetType(KeyState.PRESS);
                tempComp.NextNode = comp1;
                piano.setTimeOfTrack(8, timer[i] + delayTime + 2.0f, comp1);

                tempObj = genSildeNode(3, 7, 0, 0, 4.72f * 3);
                tempComp = tempObj.GetComponent<SilderNode>();
                tempComp.SetType(KeyState.PRESS);
                comp1.NextNode = tempComp;
                piano.setTimeOfTrack(7, timer[i] + delayTime + 3.0f, tempComp);

                tempObj = genSildeNode(3, 6, 0, 0, 4.72f * 4);
                comp1 = tempObj.GetComponent<SilderNode>();
                comp1.SetType(KeyState.PRESS);
                tempComp.NextNode = comp1;
                piano.setTimeOfTrack(6, timer[i] + delayTime + 4.0f, comp1);

                tempObj = genSildeNode(3, 6, 0, 0, 4.72f * 5);
                tempComp = tempObj.GetComponent<SilderNode>();
                tempComp.SetType(KeyState.OUTPRESS);
                comp1.NextNode = tempComp;
                piano.setTimeOfTrack(6, timer[i] + delayTime + 5.0f, tempComp);
                break;
            case 2:
                var obj2 = Instantiate(nodes[2], createPos[10].transform.position, createPos[10].transform.rotation);
                var comp2 = obj2.GetComponent<SilderNode>();
                comp2.SetTimeTrack(timer[i] + delayTime, 6);
                piano.setTimeOfTrack(6, timer[i] + delayTime, comp2);
                break;

        }
        isCreate[i] = false;
    }
    GameObject genSildeNode(int type,int pos,float x,float y,float z)
    {
       return Instantiate(nodes[type], new Vector3(createPos[pos].transform.position.x+x,
                                                                    createPos[pos].transform.position.y+y,
                                                                    createPos[pos].transform.position.z + z),
                                                                    createPos[pos].transform.rotation);
    }
}

