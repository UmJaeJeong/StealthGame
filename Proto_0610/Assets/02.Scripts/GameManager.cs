using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    protected static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
                if (instance == null)
                {
                    Debug.LogError("Error Creating Intance.");
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CameraCtrl m_cCamera;
    public UIManager m_cUIManager;
    public ShooterCtrl m_cShooterCtrl;
    public ItemManager m_cItemManager;

    private int Quset = 0;
    

    public void AddQuest()
    {
        Quset++;

    }

    public void Clear()
    {
        if(Quset == 2)
        {
            m_cUIManager.Clear_Text.enabled = true;
            Debug.Log("GameClear");
        }
        else
        {
            Debug.Log("조건이 만족되지 않았습니다.");
        }
    }
}


