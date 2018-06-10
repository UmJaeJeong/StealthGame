using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public List<Transform> bullet_btn = new List<Transform>();

    public Canvas Canvas_world;
    public Text Die_Text;
    public Text Clear_Text;

    private float right = 0;
    //bilboard를 위한 카메라 Target;
    public Transform Target;

    private void Start()
    {
        Target = Camera.main.gameObject.transform;
    }

    public void OnButton()
    {
        for (int i = 0; i < bullet_btn.Count; i++)
        {

            bullet_btn[i].position = GameManager.Instance.m_cShooterCtrl.transform.position + Vector3.up * 10.0f + Vector3.right * right;
            //bilboard 기능 카메라 향함
            bullet_btn[i].rotation = Quaternion.LookRotation(Target.forward, Target.up);

            right += 5.0f;

        }
        right = 0;
        for (int i = 0; i < bullet_btn.Count; i++)
        {
            bullet_btn[i].gameObject.SetActive(true);
        }
    }
    public void OffButton()
    {
        for (int i = 0; i < bullet_btn.Count; i++)
        {
            bullet_btn[i].gameObject.SetActive(false);
        }
    }
}
