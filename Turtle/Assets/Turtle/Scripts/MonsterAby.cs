using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAby
{
    protected string name;
    public Animator ani;
    float _hp;
    public float hp
    {
        get { return _hp; }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                _hp = 0;
            }
        }
    }
    float _speed;
    public float speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    float _attackNum;
    public float attackNum
    {
        get { return attackNum; }
        set { _attackNum = value; }
    }
    float _defendNum;
    public float defendNum
    {
        get { return _defendNum; }
        set { _defendNum = value; }
    }


    public enum MonsterState
    {
        Idle,//閒置
        Check,//盯著玩家
        Walk,//移動
        Warn,//盯著玩家
        Chase,//追擊玩家
        Return,//超出追擊範圍後返回
        Attack,//攻擊
        Deffend//防禦
    }

    public abstract void AttackAct();
    public abstract void DefendAct();

    public abstract void Idle();

    public abstract void Move();
    /// <summary>
    /// 初始隨機閒置狀態
    /// </summary>
    /// <param name="lastActTime">更新行动时间</param>
    /// <param name="actionWeight">動作權重</param>
    /// <param name="targetRotation">怪物的目標朝向</param>
    /// <param name="currentState">目前狀態</param>
    public abstract void RandomAction();
/// <summary>
/// 更改狀態
/// </summary>
/// <param name="currentState">目前狀態</param>
    public abstract void ChangeState();
    public abstract void Check();


}
