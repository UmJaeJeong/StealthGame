using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    public Transform Target;

    public bool ShootMode = false;

    public float dist = 7.4f;
    public float height = 31.2f;
    public float shootheight = 20.0f;

    void Start()
    {
        Target = GameObject.Find("Shooter").transform;
    }

    void FixedUpdate()
    {
        if(!ShootMode)
        {
            FollowTarget();
        }
        else
        {
            ShootCamera();
        }
    }

    void FollowTarget()
    {
        transform.position = Target.position - (Vector3.forward * dist) + (Vector3.up * height);
        transform.LookAt(Target);

    }
    void ShootCamera()
    {
        transform.position = Target.position + (Vector3.up * shootheight);
        transform.LookAt(Target);
    }
}
