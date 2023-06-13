using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using System.IO;
using System;
using Random = UnityEngine.Random;

public class MetronomeSoundPlayer : MonoBehaviour
{
    // public AudioSource F3,G3, A3,B3, C4, D4, E4, F4, G4, A4, B4, C5, D5, E5, F5, G5, A5, B5, C6;
    public List<AudioSource> Source;
    private AudioSource obj;
    public double bpm;
    private double bpmInSeconds;
    private bool metronomeEnabled;
    private bool recEnabled;
    private int recCount = 0;
    private bool playEnabled;
    private int playCount = 0;
    private int channel = 0;
    private NoteInfo ttt = new NoteInfo();
    private NoteInfo data;
    private Data_ tt;

    private WaitForSeconds wait;
    private WaitForSeconds wait1;
    private WaitForSeconds wait2;
    // Start is called before the first frame update

    private float startTime;
    private int tttlen;

    private float displacementAomunt;
    public MeshRenderer meshRenderer;
    private float colorRandom = 0.0f;
    private float h = 0.0f, s = 0.6f, v = 0.5f, hcons = 0.0f;
    private float colorTime;
    private Dictionary<string, int> dict;
    private int tmpnote = 0;
    private bool firstkey;
    void Start()
    {
        obj = gameObject.GetComponent<AudioSource>();
        
        bpmInSeconds = 60.0f / bpm;
        wait = new WaitForSeconds((float)bpmInSeconds);

        metronomeEnabled = false;
        recEnabled = false;
        playEnabled = false;
        
        
        dict = new Dictionary<string, int>(){
            {"F3", 0}, {"G3", 1}, {"A3", 2}, {"B3", 3},
            {"C4", 4}, {"D4", 5}, {"E4", 6}, {"F4", 7}, {"G4", 8}, {"A4", 9}, {"B4", 10},
            {"C5", 11}, {"D5", 12}, {"E5", 13}, {"F5", 14}, {"G5", 15}, {"A5", 16}, {"B5", 17},
            {"C6", 18},
        };


        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;
            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;
            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                switch (note.shortDisplayName)
                {
                    case "A#3":
                        ToggleMetronome();
                        break;
                    case "C#4":
                        ToggleRec();
                        break;
                    case "D#4":
                        firstkey = true;
                        colorTime = Time.time;
                        Toggleplay();
                        break;
                    case "F#4":
                        ttt = new NoteInfo();
                        Save();
                        channel = 0;
                        hcons = 0.0f;
                        s = 1.0f;
                        v = 0.5f;

                        break;
                    default:
                        Rec(note.shortDisplayName, velocity, tttlen++);
                        
                        coolBallRT(note.shortDisplayName);
                        // Debug.Log(Time.time);
                        break;
                }
                

                
                
            };
        };
        
    }

    void Update()
    {
        displacementAomunt = Mathf.Lerp(displacementAomunt, 0, Time.deltaTime*4);
        // s = Mathf.Lerp(s, 0, Time.deltaTime);
        // v = Mathf.Lerp(v, 0, Time.deltaTime);
        meshRenderer.material.SetFloat("_Amount", displacementAomunt);
        
        
        // Debug.Log(h);
        meshRenderer.material.SetColor("_Color", Color.HSVToRGB(h, s, v));

    }
    private void coolBallDrum()
    {
        displacementAomunt += 0.2f;
        colorRandom = Random.Range(0.0f, 1.0f);
        meshRenderer.material.SetFloat("_Random", colorRandom);
    }

    private void coolBallPiano()
    {
        float colorT = Time.time - colorTime;
        if (colorT > bpmInSeconds){
            colorT = 0.25f;
        }else if(colorT < bpmInSeconds/4.0f){
            colorT = 0.8f;
        }else{
            colorT = -colorT/(float)bpmInSeconds + 0.8f;
        }
        colorTime = Time.time;
        
        // s = colorT;
        s = colorT;
    }
    private void coolBallRT(string note)
    {
        if(!recEnabled){
            if(firstkey){
                firstkey = !firstkey;
            }else{
                switch (Mathf.Abs(tmpnote - dict[note]))
                {    
                    case 0:
                        // hcons 
                        h = 0.1f;
                        break;
                    case 1:
                        // hcons 
                        h = 0.3f;
                        break;
                    case 2: 
                        // hcons
                        h = 0.5f;
                        break;
                    case 3:
                        // hcons
                        h = 0.7f;
                        break;
                    default:
                        // hcons
                        h = 0.9f;
                        break;
                }
                // h += hcons;
                // h -= (int)h;
            }
            
            // Debug.Log(Mathf.Abs(tmpnote - dict[note])+ "--" + hcons);
            tmpnote = dict[note];
        }
    }
    private void ToggleMetronome()
    {
        
        metronomeEnabled = !metronomeEnabled;
        if (metronomeEnabled)
        {
            
            StartCoroutine(Tick());
        }
        else
        {
            StopCoroutine(Tick());
        }
    }

    private void Rec(string note, float vel, int listlen){
        if (recEnabled){         
            tt = new Data_();
            tt.note = note;
            tt.velocity = vel;
            if(channel == 0){
                ttt.drum[listlen-1].time = Time.time - startTime;
                startTime = Time.time;
                ttt.drum.Add(tt);  
            }else{
                ttt.piano[listlen-1].time = Time.time - startTime;
                startTime = Time.time;
                ttt.piano.Add(tt);  

            }
            
        }
    }

    private void ToggleRec(){
        recEnabled = !recEnabled;
        if (recEnabled)
        {
            startTime = Time.time;
            tttlen = 1;

            tt = new Data_();
            tt.note = "start";
            Debug.Log("Channel: " + channel);
            if(channel == 0){
                ttt.drum.Add(tt);
            }else{
                ttt.piano.Add(tt);
            }
            
            StartCoroutine(Tick());
        }
        else
        {
            StopCoroutine(Tick());                       
            Save();
            channel++;
        }
    }

    private void Toggleplay(){
        playEnabled = !playEnabled;
        if (playEnabled)
        {
            Debug.Log("Play");
            Load(out data);
            StartCoroutine(Tick());
            StartCoroutine(Play1());
            StartCoroutine(Play2());
            
        }
        else
        {
            StopCoroutine(Tick());
            StopCoroutine(Play1());
            StopCoroutine(Play2());
        }
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            if (metronomeEnabled||recEnabled||playEnabled) 
            {
                // Debug.Log("Tick");
                if(playEnabled){
                    ++playCount;
                }else if(recEnabled){
                    ++recCount;
                }
                obj.Play();
                yield return wait;
            }
            else
            {
                if(playEnabled){
                    playCount = 0;
                }else if(recEnabled){
                    recCount = 0;
                }
                yield return null;
            }
        }
    }


    private IEnumerator Play2()
    {
        bool flag = true;
        while(flag){     
            if (playEnabled && playCount > 4) {
                foreach(var i in data.piano){
                    if(!playEnabled){
                        break;
                    }
                    wait2 = new WaitForSeconds((float)i.time);
                    if(i.note != "start"){
                        foreach(AudioSource piano in Source){
                            if (piano.name == i.note){
                                // Debug.Log(i.note);
                                coolBallPiano();
                                piano.GetComponent<AudioSource>().Play();
                                yield return wait2;
                            }
                        }
                    }else{
                        yield return wait2;
                    }
        
                }
                flag = false;
            }else{
                yield return null;
            }

        }
    }
    private IEnumerator Play1()
    {
        bool flag = true;
        while(flag){     
            if (playEnabled && playCount > 4) {
                foreach(var j in data.drum){
                    if(!playEnabled){
                        break;
                    }
                    wait1 = new WaitForSeconds((float)j.time);
                    if(j.note != "start"){
                        foreach(AudioSource drum in Source){
                            if (drum.name == j.note){
                                // Debug.Log(i.note);
                                coolBallDrum();
                                drum.GetComponent<AudioSource>().Play();
                                yield return wait1;
                            }
                        }
                    }else{
                        yield return wait1;
                    }
        
                }
                flag = false;
            }else{
                yield return null;
            }

        }
    }
    
    public void Save()
    {
        string saveFile = "D:/musicvirtual/musicvisual/MusicVisualize/Assets/metronome/rec.json";
        if(File.Exists(saveFile))
        {  
            // File exists!
            string jsonInfo = JsonUtility.ToJson(ttt, true);
            // jsonInfo = JsonUtility.ToJson(data, true);
            // Debug.Log("Save..."+ jsonInfo.ToString());
            File.WriteAllText(saveFile, jsonInfo);
        }
        else
        {
        // MKDIR
            Debug.Log("Can't find file");
        }
    }

    void Load(out NoteInfo data)
    {
        data = new  NoteInfo();
        var loadFile = "D:/musicvirtual/musicvisual/MusicVisualize/Assets/metronome/rec.json";;

        if(File.Exists(loadFile))
        {
            // File exists!
            data = JsonUtility.FromJson<NoteInfo>(File.ReadAllText(loadFile));
        }
        else
        {
        // MKDIR
            Debug.Log("Can't find file: " + "PositionConfig.json");
        }

    }

}



