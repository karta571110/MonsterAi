using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Property
{
    protected string name;

    public Animator ani;
    public GameObject playerUnit;//獲取玩家單位
    public NavMeshAgent agent;
    public Transform trans;//初始位置


    public Vector3 initialPosition;
    public float wanderRadius;          //游走半径，移动状态下，如果超出游走半径会返回出生位置
    public float alertRadius;         //警戒半径，玩家进入后怪物会发出警告，并一直面朝玩家
    public float defendRadius;          //自卫半径，玩家进入后怪物会追击玩家，当距离<攻击距离则会发动攻击（或者触发战斗）
    public float chaseRadius;            //追击半径，当怪物超出追击半径后会放弃追击，返回追击起始位置

    public float attackRange;            //攻击距离
    public float walkSpeed;          //移动速度
    public float runSpeed;          //跑动速度
    public float turnSpeed;         //转身速度，建议0.1


    public MonsterAby.MonsterState currentState = MonsterAby.MonsterState.Idle;//閒置時間
    public float[] actionWeight;//设置待机时各种动作的权重，顺序依次为呼吸、观察、移动
    public float actRestTime;//更換待機指令的間隔時間
    public float lastActTime;//最近一次指令時間


    public float distanceToPlayer;//與玩家距離
    public float distanceToInitial;//怪物與初始位置的距離
    public Quaternion targetRotation;//怪物的目標朝向

    public bool is_Warned = false;
    public bool is_Running = false;
    public bool detectPlayer = false;

    public float subAngle;
    public float lookAngle;
    public int lookAccurate;
}
