using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : MonoBehaviour {

    public Transform FirePos;
    public Transform Bullet;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void CreatBullet()
    {
        GameManager.Instance.m_cCamera.Target = Instantiate(Bullet, FirePos.position, FirePos.rotation);
    }
}
