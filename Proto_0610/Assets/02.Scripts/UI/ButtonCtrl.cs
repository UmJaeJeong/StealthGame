using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour {

    public Transform[] bullet;

	public void NormalBulletBtn()
    {
        GameManager.Instance.m_cShooterCtrl.MyGun.Bullet = bullet[(int)ItemManager.ItemType.Normal_Bullet];
        GameManager.Instance.m_cShooterCtrl.Shooting = true;
        GameManager.Instance.m_cUIManager.OffButton();
    }
    public void AggroBulletBtn()
    {
        GameManager.Instance.m_cShooterCtrl.MyGun.Bullet = bullet[(int)ItemManager.ItemType.Aggro_Bullet];
        GameManager.Instance.m_cShooterCtrl.Shooting = true;
        GameManager.Instance.m_cUIManager.OffButton();
    }
    public void WhorfBulletBtn()
    {
        GameManager.Instance.m_cShooterCtrl.MyGun.Bullet = bullet[(int)ItemManager.ItemType.Whorf_Bullet];
        GameManager.Instance.m_cShooterCtrl.Shooting = true;
        GameManager.Instance.m_cUIManager.OffButton();
    }
}
