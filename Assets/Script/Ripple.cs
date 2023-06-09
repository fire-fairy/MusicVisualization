using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ripple : MonoBehaviour
{
    public ParticleSystem ripple;
    private ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
    public GameObject RippleCam;

    [SerializeField]
    private float velocityXZ;
    [SerializeField]
    private float velocityY;

    private Vector3 lastPosition;

    [Header("Player attribute")]
    public float speed = 10.0f;
    public float rotateSpeed = 5.0f;
    public float jumpSpeed = 3.0f;
    public float gravity = 40.0f;

    private float horizontal;
    private float vertical;
    private float gravityForce = 0;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController cc;
    

    // Start is called before the first frame update
    void Start()
    {

        cc = transform.GetComponent<CharacterController>();

        emitParams.startColor = Color.white;
        emitParams.startSize = 3f;
        emitParams.startLifetime = 5f;

        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CCmove();
        CCRotate();

        velocityXZ = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(lastPosition.x, 0, lastPosition.z));
        velocityY = Vector3.Distance(new Vector3(0, transform.position.y, 0), new Vector3(0, lastPosition.y, 0));
        lastPosition = transform.position;

        Shader.SetGlobalVector("_Player", transform.position);
    }

    void CCmove()
    {
        if (cc.isGrounded)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection = transform.TransformDirection(moveDirection);

            cc.Move(moveDirection * speed);
        }


        gravityForce -= gravity * Time.deltaTime * 5;

        if (cc.isGrounded && gravityForce < -2)
            gravityForce = -2;
        cc.Move(new Vector3(0, gravityForce * Time.deltaTime, 0));
    }

    public void CCRotate()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    void RippleEmit(int start, int End, int Delta, float speed, Vector3 contact)
    {
        Vector3 forward = ripple.transform.eulerAngles;
        forward.y = start;
        ripple.transform.eulerAngles = forward;

        for (int i = start; i < End; i += Delta)
        {
            Debug.Log(ripple.transform.position);
            ripple.Emit(contact + ripple.transform.forward * 0.5f + Vector3.up * 0.5f, ripple.transform.forward * speed, 0.2f, 1, Color.white);
            ripple.transform.eulerAngles += Vector3.up * Delta;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4 && velocityY > 0.015f)
        {
            RippleEmit(-180, 180, 3, 2, this.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 contact = other.ClosestPoint(transform.position);
        if (other.gameObject.layer == 4 && velocityXZ > 0.005f && Time.renderedFrameCount % 5 == 0)
        {
            Quaternion rippleDir = transform.rotation;
            rippleDir.SetLookRotation(moveDirection);
            int y = (int)rippleDir.eulerAngles.y;
            RippleEmit(y - 110, y + 110, 2, 1.5f, contact);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4 && velocityY > 0.015f)
        {
            RippleEmit(-180, 180, 3, 2, this.transform.position);
        }
    }
}
