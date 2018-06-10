using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public System.Action<Collision> Effect;

    private int Damage;
    private int Time;
    public int time
    {
        get { return Time; }
    }
    private int HitCount;
    public int hitCount
    {
        get { return HitCount; }
        set { HitCount = value; }
    }


    public Item(int damage, int time, int hitcount)
    {
        Damage = damage;
        Time = time;
        HitCount = hitcount;
    }
}
