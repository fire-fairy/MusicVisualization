using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Transform pos;
    public MeshRenderer ball;
    
    private Vector3 dilation ,tmp;
    private Color light_, light_2;

    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<Transform>();
        dilation = new Vector3(0.0f, 0.0f, 0.0f);
        tmp = new Vector3(0.1f, 0.1f, 0.1f);

        light_ = ball.material.GetColor("_FrontColor");
        light_2 = light_;

    }

    // Update is called once per frame
    void Update()
    {
        ShieldCtrl(Time.deltaTime*2);
    }
    void ShieldCtrl(float time)
    {

        if (Input.GetKeyDown(KeyCode.I))
        {// Dilation
            Debug.Log("Dilation");
            dilation += tmp;
        }else{
            //lerp
            dilation = Vector3.Lerp(dilation, new Vector3(0.0f, 0.0f, 0.0f), time);
            ball.material.SetVector("_Pulse", dilation);
        }
        
        
        if (Input.GetKeyDown(KeyCode.K))
        {// Clap
            Debug.Log("Clap" + light_);
            light_ = light_2;
            // light_.a = 0.3f;
        }else{
            //lerp
            if(light_.a > 0.3f)
            {
                light_.a = Mathf.Lerp(light_.a, light_.a*0.3f, time); 

                ball.material.SetColor("_FrontColor", light_);   
            }
            
            
        }

        if (Input.GetKey(KeyCode.L) && !Input.GetKey(KeyCode.J))
        {// Right Rotation
            Debug.Log("Right Rotation");
            pos.eulerAngles = new Vector3(0.0f, pos.rotation.eulerAngles.y - 0.5f, 0.0f);

        }

        if (!Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.J))
        {// Left Rotation
            Debug.Log("Left Rotation");
            pos.eulerAngles = new Vector3(0.0f, pos.rotation.eulerAngles.y + 0.5f, 0.0f);
        }
    }
}
