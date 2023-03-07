using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int MaxHP;
    private int _hp;

    private Transform _fill;

    void Start()
    {
        _hp = MaxHP;
        _fill = transform.GetChild(0);
    }

    public void ChangeHealth(int amt)
    {
        _hp = Mathf.Clamp(_hp + amt, 0, MaxHP);
        _fill.localScale = new Vector3((float)(_hp)/MaxHP, _fill.localScale.y, _fill.localScale.z);
    }
}
