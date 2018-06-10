using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    //Aggro Bullet 용 총알주변 적 검사
    List<EnemyCtrl> enemies = new List<EnemyCtrl>();
    int maxCount = 0;
    public int curCount = 0;
    bool Coll = false;
    //Shoot 변수
    private float MoveSpeed = 0f;
    //Player Move
    private Vector3 BeforePos = Vector3.zero;

    public ItemManager.ItemType itemType;

    Item item;

    private void Start()
    {
        MoveSpeed = 20.0f;
        BeforePos = this.gameObject.transform.position;
        item = GameManager.Instance.m_cItemManager.items[(int)itemType];
        SelectEffect();
    }
    private void Update()
    {
        float distanceThisFrame = MoveSpeed * Time.deltaTime;
   
        transform.Translate(transform.forward * distanceThisFrame, Space.World);
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        item.Effect(collision);
    }

    public void NormalBullet(Collision collision)
    {
        Vector3 incomingVec = transform.position - BeforePos; // 입사각 => 충돌지점 - 출발지점
        Vector3 normalVec = collision.contacts[0].normal; // 법선벡터
        Vector3 reflecVec = Vector3.Reflect(incomingVec, normalVec); //반사각

        transform.rotation = Quaternion.LookRotation(reflecVec);

        BeforePos = transform.position; // 이전 위치 갱신

        item.hitCount--;
        if (collision.collider.CompareTag("ENEMY"))
        {
            //Destroy(collision.collider.gameObject);
            EnemyCtrl enemy = collision.collider.GetComponent<EnemyCtrl>();
            enemy.Stop = true;
            //Transform StopPosition = this.gameObject.transform;
            //enemy.target = StopPosition;
            enemy.Invoke("UnStop", 2.0f);

            //Invoke("enemy.GetNextWaypoint()", 2.0f);
            Destroy(transform.gameObject); 
            GameManager.Instance.m_cItemManager.DestroyBullet(this.transform, item);
        }

        if (item.hitCount == 0)
        {
            Destroy(transform.gameObject);    //총알로 사용할 경우 4번째 충돌시 파괴
            GameManager.Instance.m_cItemManager.DestroyBullet(this.transform, item);
        }


    }
    public void AggroBullet(Collision collision)
    {
        if (!Coll)
        {
            MoveSpeed = 0;// 제 자리 고정
            Collider[] colls = Physics.OverlapSphere(transform.position, 20.0f);

            foreach (Collider coll in colls)
            {
                if (coll.CompareTag("ENEMY"))
                {//어그로 당한 적들을 enemies 변수에 넣어줌
                    enemies.Add(coll.GetComponent<EnemyCtrl>());
                    enemies[maxCount].target = transform;
                    enemies[maxCount].m_eState = EnemyCtrl.EnemyState.Aggro;
                    enemies[maxCount].GetComponent<ConeCollider>().enabled = false;
                    maxCount++;
                }
            }
            Coll = true;
            StartCoroutine(Aggroing());
        }
    }
    IEnumerator Aggroing()
    {
        bool end = false;
        while(!end)// 어그로 당한 적이 모두 폭탄 주변으로 왔나 검사
        {
            if (curCount == maxCount)
            {
                
                yield return new WaitForSeconds(item.time);// 2초후 적들 어그로 상태 해제
                for(int i = 0; i < maxCount; i++)
                {
                    enemies[i].target = enemies[i].MovePoint[0];// 타겟을 총알 에서 다른 것으로 바꿔줌
                    enemies[i].m_eState = EnemyCtrl.EnemyState.Idle;
                    enemies[i].GetComponent<ConeCollider>().enabled = true;
                    enemies[i].Stop = false;
                }
                Destroy(gameObject, 1.0f);
                GameManager.Instance.m_cCamera.Target = GameManager.Instance.m_cShooterCtrl.transform;
                GameManager.Instance.m_cCamera.ShootMode = false;
                GameManager.Instance.m_cShooterCtrl.Shooting = false;
                end = true;
            }
            yield return null;
        }
    }
    public void WhorfBullet(Collision collsision)
    {
        GameManager.Instance.m_cShooterCtrl.transform.position = transform.position;
        GameManager.Instance.m_cCamera.Target = GameManager.Instance.m_cShooterCtrl.transform;
        GameManager.Instance.m_cCamera.ShootMode = false;
        GameManager.Instance.m_cShooterCtrl.Shooting = false;
        Destroy(gameObject);
    }
    public void SelectEffect()
    {
        switch(itemType)
        {
            case ItemManager.ItemType.Normal_Bullet:
                NewMethod(NormalBullet);
                break;
            case ItemManager.ItemType.Aggro_Bullet:
                NewMethod(AggroBullet);
                break;
            case ItemManager.ItemType.Whorf_Bullet:
                NewMethod(WhorfBullet);
                break;
            default:
                break;
        }
    }

    private void NewMethod(System.Action<Collision> effect)
    {
        item.Effect = effect;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 1.0f);
    //}
}
