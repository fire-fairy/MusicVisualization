using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NoteJS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class NoteInfo{
    // public string channelName = "";
    public List<Data_> piano = new List<Data_>();
    public List<Data_> drum = new List<Data_>();
    // public string getChannel(){
    //     return channelName;
    // }
    // public string getNote(){
    //     return info.Item1;
    // }
    // public int getVeloclty(){
    //     return info.velocity;
    // }

}

[Serializable]
public class Data_{
    
    public string note = "123";
    public float velocity = 0.0f;
    public float time = 0.0f;
}
