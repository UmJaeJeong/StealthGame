using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour {
    public enum EnemyState
    { Idle, Aggro, Trace}

    public float speed = 10f;
    public Transform target;
    private int MovePoinIdx = 0;
    private bool dead;

    public EnemyState m_eState;
    //어그로 폭탄 주변에 도착여부 검사
    bool Reach = false;

    public float Attack_Dist;
    public Vector3 AttackRange;

    public Transform[] MovePoint;

    //적발견
    Material Skin;

    //노말 폭탄
    public bool Stop;
    //
    public float TraceTime = 3.0f;

    void Start()
    {
        //
        Stop = false;
         m_eState = EnemyState.Idle;
        Attack_Dist = 10.0f;
         Skin = GetComponent<Renderer>().material;
        dead = false;
        MovePoinIdx = MovePoint.Length-1;
        target = MovePoint[0];
        AttackRange = transform.localScale;
    }
    void Update()
    {
        if (!Stop)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            transform.LookAt(target.position);
            float dist = Vector3.Distance(transform.position, target.position);


            if (dist <= 6.5f)
            {
                //Debug.Log(m_eState);

                //Debug.Log("거리");
                if (m_eState == EnemyState.Aggro)
                {
                    //Debug.Log("ㅁㅁㅁ");
                    if (!Reach)
                    {// 카운트를 한번만 올려줄 수 있도록 bool 변수로 도착 여부 검사
                        Debug.Log("도착");
                        target.GetComponent<BulletCtrl>().curCount++;
                        Reach = true;
                        Stop = true;
                    }
                }
                else if (m_eState == EnemyState.Idle)
                {
                    GetNextWaypoint();
                }
            }
        }
    
    }

    public void GetNextWaypoint()
    {
        if (MovePoinIdx == MovePoint.Length - 1)
        {
            MovePoinIdx = -1;
        }
        MovePoinIdx++;
        target = MovePoint[MovePoinIdx];
    }
    public IEnumerator Trace()
    {
        while(m_eState == EnemyState.Trace)
        {
            TraceTime -= Time.deltaTime;
            if(TraceTime <= 0)
            {
                GameManager.Instance.m_cUIManager.Die_Text.enabled = true;
                GetNextWaypoint();
                m_eState = EnemyState.Idle;
            }
            yield return null;
        }
        TraceTime = 3.0f;
        yield return new WaitForSeconds(3.0f);
        GetNextWaypoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("PLAYER"))
        //{
        //    Destroy(this.gameObject);
        //}
    }

    public void UnStop()
    {
        Stop = false;
    }
}
