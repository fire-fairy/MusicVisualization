using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Satellite : MonoBehaviour
{
    [Header("Orbit Parameter")]
    public float radius = 1.0f;
    public float speed = 1.0f;

    // Renderer Position
    public GameObject renderObj;

    // Animation
    private Tween rotateTween;
    // Start is called before the first frame update

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="radius">����d��</param>
    /// <param name="speed">����t��</param>
    /// <param name="delay">0~1���ȡA����h�[�[�J�`��(EX 0.5�� delay 0.5*time��}�l�[�J)</param>
    public void Revolution(float radius, float speed, float delay)
    {
        if (rotateTween != null)
        {
            rotateTween.Kill();
            this.transform.rotation = Quaternion.identity;
        }
        this.transform.rotation = transform.parent.rotation;
        float time = radius * 2 * Mathf.PI / speed;
        renderObj.transform.localPosition = new Vector3(-radius, 0, 0);
        renderObj.GetComponent<Renderer>().material.DOFade(0, 0);

        renderObj.GetComponent<Renderer>().material.DOFade(1f, time).SetDelay(delay * time);
        rotateTween = this.transform.DOLocalRotate(new Vector3(0, 180, 0), time / 2).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetDelay(delay * time);
    }
}
