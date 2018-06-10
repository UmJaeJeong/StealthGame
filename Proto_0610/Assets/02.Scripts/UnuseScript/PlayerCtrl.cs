using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    // 플레이어 이동 시에만 사용하는 클래스 // 전투, 효과 등은 다른 클래스에서
    private Vector2 MousePos = Vector2.zero;
    private Vector2 StartPos = Vector2.zero;
    Vector3 TargetPos;
    float TargetDist = 0;
    //회전 컨트롤
    private Vector2 Dir = Vector2.zero;
    private float dist = 0;
    private float Degree = 0;
    // 궤적
    private LineRenderer Line;
    public Transform LineColl;
    //Shoot 변수
    public bool MouseDown = false;
    public bool Shooting = false;
    public bool Moving = false;
    private float MoveSpeed = 5.0f;
    //Player Move
    private Vector3 BeforePos = Vector3.zero;

    private int m_nHitCount = 4;

    public GameObject pos;

    public bool Btn_Click;




    private void Start()
    {
        Btn_Click = false;
        Line = GetComponent<LineRenderer>();
        Line.enabled = false;

        MoveSpeed = 20.0f;
    }
    private void Update()
    {
        if (!Btn_Click) {
            if (!Shooting)// 슛 중이 아닐 때
            {
                ShootingSling();
                TouchMove();
            }
            if (MouseDown)
            {
                RotatePlayer();
            }
        }
    }
    void TouchMove()
    {
        if (Input.GetMouseButtonDown(0) && !MouseDown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                if (hit.collider.gameObject.CompareTag("GROUND")){
                    Debug.Log("땅 클릭");
                    Moving = true;
                    TargetPos = hit.point;
                    TargetPos.y = 1.25f;
                }
                
            }
        }

        if (Moving)
        {
            MoveSpeed = 3.0f;
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * MoveSpeed);
            transform.LookAt(TargetPos);
            TargetDist = Vector3.Distance(transform.position, TargetPos);
            if(TargetDist < 0.1f)
            {
                Moving = false;
            }
        }
    }
    void SetFirst()
    {
        Degree = 0;
        Dir = Vector2.zero;
        BeforePos = Vector3.zero;
        dist = 0;
        m_nHitCount = 4;
    }
    void ShootingSling()
    {
           // Debug.Log("플레이 준비");
            if (Input.GetMouseButtonDown(0) && !MouseDown)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    if (hit.collider.CompareTag("PLAYER"))
                    { 
                        //변수초기화
                        SetFirst();

                        StartPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);// StartPos = Stick.position;
                        MouseDown = true;
                    transform.position = transform.position;
                        //GameManager.Instance.m_cCamera.ShootMode = true;
                        //궤적
                        StartCoroutine(DrawLine());
                    }
                }
            }
            else if (Input.GetMouseButton(0) && MouseDown)
            {
                MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                dist = Vector3.Distance(StartPos, MousePos);
            }
            else if (Input.GetMouseButtonUp(0) && MouseDown)
            {
                MouseDown = false;
                StartCoroutine(MovePlayer());
            }
    }
    void RotatePlayer()
    {
        if (dist > 1.0f)
        {
            Dir = MousePos - StartPos;
            Degree = (Mathf.Atan2(Dir.x, Dir.y) * Mathf.Rad2Deg + 360f) % 360;

            transform.rotation = Quaternion.Euler(0, Degree + 180f, 0);
        }
    }
    IEnumerator MovePlayer()
    {
        MoveSpeed = 20f;
        Shooting = true;
        BeforePos = transform.position;// 출발전에 이전위치 저장

        while (m_nHitCount != 0) //  쓰리쿠션
        {
            transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed, Space.Self);

            yield return null;
        }
        Shooting = false;
    }
    IEnumerator DrawLine()
    {
        Line.enabled = true;
        Vector3 incomingVec;
        Vector3 reflecVec;

        Line.positionCount = 3;

        while (MouseDown)
        {
            Line.SetPosition(0, transform.position);
            RaycastHit hitpoint;
            if (Physics.SphereCast(transform.position, transform.localScale.x/2, transform.forward, out hitpoint, 100.0f))
            {
                LineColl.position = hitpoint.point;
                incomingVec = LineColl.position - transform.position;
                reflecVec = Vector3.Reflect(incomingVec, hitpoint.normal);
                LineColl.rotation = Quaternion.LookRotation(reflecVec);
                Line.SetPosition(1, LineColl.position);

                RaycastHit hitpoint_2;
                if (Physics.SphereCast(LineColl.position, transform.localScale.x / 2, LineColl.forward, out hitpoint_2, 100.0f))
                {
                    Line.SetPosition(2, hitpoint_2.point);
                }

            }

            //while (MouseDown)
            //{
            //    Line.SetPosition(0, transform.position);
            //    RaycastHit hitpoint;
            //    if (Physics.Raycast(transform.position, transform.forward, out hitpoint, 100.0f))
            //    {                
            //        LineColl.position = hitpoint.point;
            //        incomingVec = LineColl.position - transform.position;
            //        reflecVec = Vector3.Reflect(incomingVec, hitpoint.normal);
            //        LineColl.rotation = Quaternion.LookRotation(reflecVec);
            //        Line.SetPosition(1, LineColl.position);

            //        RaycastHit hitpoint_2;
            //        if (Physics.Raycast(LineColl.position, LineColl.forward, out hitpoint_2, 100.0f))
            //        {
            //            Line.SetPosition(2, hitpoint_2.point);
            //        }

            //    }
            yield return null;
        }
        Line.enabled = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 incomingVec = transform.position - BeforePos; // 입사각 => 충돌지점 - 출발지점
        Vector3 normalVec = collision.contacts[0].normal; // 법선벡터
        Vector3 reflecVec = Vector3.Reflect(incomingVec, normalVec); //반사각

        if (reflecVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(reflecVec);
        }
        BeforePos = transform.position; // 이전 위치 갱신

        m_nHitCount--;
        if(m_nHitCount == 0)
        {
           // Destroy(this.gameObject);    //총알로 사용할 경우 4번째 충돌시 파괴
           
        }

        if (collision.collider.CompareTag("WALL"))
        {
            Debug.Log("벽 충돌");
            Moving = false;
        }

    }


}
