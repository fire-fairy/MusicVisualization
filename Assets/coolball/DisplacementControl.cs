using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplacementControl : MonoBehaviour
{

    public float displacementAomunt;
    MeshRenderer meshRenderer;
    public float colorRandom = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        displacementAomunt = Mathf.Lerp(displacementAomunt, 0, Time.deltaTime*4);
        //colorRandom = Mathf.Lerp(colorRandom, 0, Time.deltaTime);

        meshRenderer.material.SetFloat("_Amount", displacementAomunt);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            colorRandom = Random.Range(0.0f, 1.0f);
            meshRenderer.material.SetFloat("_Random", colorRandom);

            displacementAomunt += 0.2f;
        }
    }
}
