using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Orbit : MonoBehaviour
{
    [Header("Orbit Revolution Parameter")]
    public float speed = 20;
    public float radius = 2f;
    public float fadeOutTime = 1.5f;
    public Material m_Material;
    public List<Satellite> satelliteList;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(AddSatellite());
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(KillSatellite());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetSatellite(5);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            AddOrbitRotate(new Vector3(0, 0, 10));
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SetOrbitRotate(new Vector3(0, 0, 70));
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            CoolOrbitRotate();
        }

        
    }

    #region Orbit
    // Orbit Control
    public void AddOrbitRotate(Vector3 degree)
    {
        this.transform.Rotate(degree);
    }

    public void SetOrbitRotate(Vector3 rotateAngle)
    {
        this.transform.rotation = Quaternion.Euler(rotateAngle);
    }

    public void CoolOrbitRotate()
    {
        this.transform.DOLocalRotate(new Vector3(0, 80, 180), 5f).SetLoops(-1, LoopType.Incremental);
    }

    #endregion

    #region Satellite
    // Satellite

    /// <summary>
    /// Add one satellite
    /// </summary>
    /// <returns></returns>
    IEnumerator AddSatellite()
    {
        FadeOutOrbit();

        yield return new WaitForSeconds(fadeOutTime);

        GameObject newTemp = ObjectPooling.Instance.GetPooledInstance(this.transform);
        satelliteList.Add(newTemp.GetComponent<Satellite>());

        RearrangeOrbit();
    }

    /// <summary>
    /// Kill one satellite
    /// </summary>
    /// <returns></returns>
    IEnumerator KillSatellite()
    {
        if (satelliteList.Count <= 0)
        {
            yield return null;
        }
        else
        {

            FadeOutOrbit();

            yield return new WaitForSeconds(fadeOutTime);

            GameObject killTemp = satelliteList[0].gameObject;
            ObjectPooling.Instance.BackToPool(killTemp);
            satelliteList.Remove(satelliteList[0]);

            RearrangeOrbit();
        }
    }

    /// <summary>
    /// Set satellite of count
    /// </summary>
    /// <param name="count"></param>
    void SetSatellite(int count)
    {
        if (satelliteList.Count > count)
        {
            for (int i = 0; i < satelliteList.Count - count; i++)
                StartCoroutine(KillSatellite());
        }
        else if (satelliteList.Count < count)
        {
            for (int i = 0; i < count - satelliteList.Count; i++)
                StartCoroutine(AddSatellite());
        }
    }

    void FadeOutOrbit()
    {
        for (int i = 0; i < satelliteList.Count; i++)
        {
            satelliteList[i].renderObj.GetComponent<Renderer>().material.DOFade(0f, fadeOutTime).SetEase(Ease.OutQuad);
        };
    }

    void RearrangeOrbit()
    {
        float count = satelliteList.Count;

        for (int i = 0; i < satelliteList.Count; i++)
        {
            Debug.LogWarning(i / count);
            satelliteList[i].Revolution(radius, speed, (i / count));
        }
    }
    #endregion
}
