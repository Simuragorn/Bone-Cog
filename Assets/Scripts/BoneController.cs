using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HingeJoint2D))]
public class BoneController : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private Vector2 forceDirection;
    [SerializeField] private float maxAngle;
    [SerializeField] private float minAngle;
    private Rigidbody2D rigidbody;
    private HingeJoint2D joint;
    private KeyCode minStateKey = KeyCode.A;
    private KeyCode maxStateKey = KeyCode.D;

    private float currentForce;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        joint = GetComponent<HingeJoint2D>();
        //joint.limits = new JointAngleLimits2D { max = maxAngle, min = minAngle };
    }

    public void SetKeys(KeyCode keyCodeForMin, KeyCode keyCodeForMax)
    {
        minStateKey = keyCodeForMin;
        maxStateKey = keyCodeForMax;
    }

    void Update()
    {
        int direction = 0;
        if (Input.GetKey(minStateKey))
            direction = -1;
        if (Input.GetKey(maxStateKey))
            direction = 1;
        currentForce = force * direction;
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        float negativeAngle = currentRotation.z - 360;
        if (Mathf.Abs(negativeAngle) < currentRotation.z)
            currentRotation.z = negativeAngle;

        if (currentRotation.z < minAngle || currentRotation.z > maxAngle)
        {
            currentForce = 0;
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0;
        }

        if (currentForce != 0)
        {
            Vector2 forceVector = forceDirection * currentForce;
            rigidbody.AddForce(forceVector, ForceMode2D.Impulse);
        }
    }
}
