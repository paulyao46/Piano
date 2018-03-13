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
    private bool clockCanRun;
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
        clockCanRun = false;
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
                    
                    if(i!=4)
                    {
                        var obj = Instantiate(nodes[createInfo[i].Dequeue()], createPos[i].transform.position, createPos[i].transform.rotation);
                        piano.setTimeOfTrack(i, timer[i]+ delayTime, obj.GetComponent<Node>());
                    }
                    else
                    {
                        var obj = Instantiate(nodes[createInfo[i].Dequeue()], createPos[10].transform.position, createPos[10].transform.rotation);
                    }
                    isCreate[i] = false;
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
                        if (i != 4)
                        {
                            var obj = Instantiate(nodes[createInfo[i].Dequeue()], createPos[i].transform.position, createPos[i].transform.rotation);
                            
                            piano.setTimeOfTrack(i, timer[i]+ delayTime, obj.GetComponent<Node>());
                        }
                        else
                        {
                            var obj = Instantiate(nodes[createInfo[i].Dequeue()], createPos[10].transform.position, createPos[10].transform.rotation);
                        }
                        isCreate[i] = false;
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
            Debug.Log("time: " + clock);
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
}
