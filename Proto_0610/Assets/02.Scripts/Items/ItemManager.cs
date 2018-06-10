using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public enum ItemType
    { Normal_Bullet, Aggro_Bullet, Whorf_Bullet }

    public List<Item> items = new List<Item>();

    private void Start()
    {
        items.Add(new Item(0, 0, 4));//normalBullet
        items.Add(new Item(0, 2, 1));//aggroBullet
        items.Add(new Item(0, 0, 0));//WhorfBullet
    }

    public void DestroyBullet(Transform tr, Item item)
    {
        Destroy(tr.gameObject);
        GameManager.Instance.m_cCamera.Target = GameManager.Instance.m_cShooterCtrl.transform;
        GameManager.Instance.m_cCamera.ShootMode = false;
        GameManager.Instance.m_cShooterCtrl.Shooting = false;
        item.hitCount = 4;
    }
}
