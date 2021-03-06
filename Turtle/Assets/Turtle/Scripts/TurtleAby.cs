﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleAby : MonsterAby
{
    
    public TurtleAby(string Name, float Hp, float Speed, float AttackNum, float DefendNum, Animator animator, Transform Trans, NavMeshAgent handleAgent)
    {
        name = Name;
        hp = Hp;
        speed = Speed;
        attackNum = AttackNum;
        defendNum = DefendNum;
        ani = animator;
        trans = Trans;
        agent = handleAgent;
        wanderRadius = 9;
        alertRadius = 15;
        defendRadius = 9;
        chaseRadius = 30;
        attackRange = 2f;
        walkSpeed = 1;
        runSpeed = 2;
        turnSpeed = 0.1f;
        actionWeight = new float[] { 3000, 3000, 4000 };
        actRestTime = 5f;
    }
    public override void AttackAct()
    {

    }

    public override void ChangeState()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            //待机状态，等待actRestTime后重新随机指令
            case MonsterState.Idle:
                if (Time.time - lastActTime > actRestTime)
                {
                    RandomAction();       //随机切换指令
                }
                //该状态下的检测指令
                EnemyDistanceCheck();
                break;

            //待机状态，由于观察动画时间较长，并希望动画完整播放，故等待时间是根据一个完整动画的播放长度，而不是指令间隔时间
            case MonsterState.Check:
                if (Time.time - lastActTime > ani.GetCurrentAnimatorStateInfo(0).length)
                {
                    RandomAction();         //随机切换指令
                }
                //该状态下的检测指令
                EnemyDistanceCheck();
                break;
            //待机状态，由于观察动画时间较长，并希望动画完整播放，故等待时间是根据一个完整动画的播放长度，而不是指令间隔时间
            case MonsterState.Deffend:
                if (Time.time - lastActTime > ani.GetCurrentAnimatorStateInfo(0).length)
                {
                    RandomAction();         //随机切换指令
                }
                //该状态下的检测指令
                EnemyDistanceCheck();
                break;

            //游走，根据状态随机时生成的目标位置修改朝向，并向前移动
            case MonsterState.Walk:
                trans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
                trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, turnSpeed);

                if (Time.time - lastActTime > actRestTime)
                {
                    RandomAction();         //随机切换指令
                }
                //该状态下的检测指令
                WanderRadiusCheck();
                break;

            //警戒状态，播放一次警告动画和声音，并持续朝向玩家位置
            case MonsterState.Warn:
                if (!is_Warned)
                {
                    ani.SetTrigger("Warn");
                    is_Warned = true;
                }
                //持续朝向玩家位置
                targetRotation = Quaternion.LookRotation(playerUnit.transform.position - trans.position, Vector3.up);
                trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, turnSpeed);
                //该状态下的检测指令
                WarningCheck();
                break;

            //追击状态，朝着玩家跑去
            case MonsterState.Chase:
                if (!is_Running)
                {
                    ani.SetTrigger("Run");
                    is_Running = true;
                }
                agent.SetDestination(playerUnit.transform.position);
                //trans.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                //朝向玩家位置
                targetRotation = Quaternion.LookRotation(playerUnit.transform.position - trans.position, Vector3.up);
                trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, turnSpeed);
                //该状态下的检测指令
                ChaseRadiusCheck();
                EnemyDistanceCheck();
                break;
            case MonsterState.Attack:
                if (Time.time - lastActTime > ani.GetCurrentAnimatorStateInfo(0).length)
                {
                    lastActTime = Time.time;
                    if (distanceToPlayer < 1.3f)
                    {
                        ani.SetTrigger("Attack01");
                        Debug.Log("Ack1");
                    }
                    else if (distanceToPlayer < 2f)
                    {
                        ani.SetTrigger("Attack02");
                        Debug.Log("Ack2");
                    }

                }
                is_Running = false;
                is_Warned = false;
                EnemyDistanceCheck();
                break;
            //返回状态，超出追击范围后返回出生位置
            case MonsterState.Return:
                //朝向初始位置移动
                targetRotation = Quaternion.LookRotation(initialPosition - trans.position, Vector3.up);
                trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, turnSpeed);
                trans.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                //该状态下的检测指令
                ReturnCheck();
                break;
        }
    }

    /// <summary>
    /// 原地呼吸、观察状态的检测
    /// </summary>
    void EnemyDistanceCheck()
    {
        distanceToPlayer = Vector3.Distance(playerUnit.transform.position, trans.position);
        Debug.Log(distanceToPlayer);
        if (currentState == MonsterState.Chase && detectPlayer)
            agent.enabled = true;
        else
            agent.enabled = false;
        if (distanceToPlayer < attackRange && detectPlayer)
        {
            currentState = MonsterState.Attack;
        }
        else if (distanceToPlayer < defendRadius && detectPlayer)
        {
            currentState = MonsterState.Chase;
        }
        else if (distanceToPlayer < alertRadius && detectPlayer)
        {
            currentState = MonsterState.Warn;
        }
    }
    /// <summary>
    /// 警告状态下的检测，用于启动追击及取消警戒状态
    /// </summary>
    void WarningCheck()
    {
        is_Running = false;
        distanceToPlayer = Vector3.Distance(playerUnit.transform.position, trans.position);
        if (distanceToPlayer < defendRadius && detectPlayer)
        {
            is_Warned = false;
            currentState = MonsterState.Chase;
        }

        if (distanceToPlayer > alertRadius )
        {
            is_Warned = false;
            RandomAction();
        }
    }
    /// <summary>
    /// 游走状态检测，检测敌人距离及游走是否越界
    /// </summary>
    void WanderRadiusCheck()
    {
        distanceToPlayer = Vector3.Distance(playerUnit.transform.position, trans.position);
        distanceToInitial = Vector3.Distance(trans.position, initialPosition);

        if (distanceToPlayer < attackRange && detectPlayer)
        {
            currentState = MonsterState.Attack;
        }
        else if (distanceToPlayer < defendRadius && detectPlayer)
        {
            currentState = MonsterState.Chase;
        }
        else if (distanceToPlayer < alertRadius && detectPlayer)
        {
            currentState = MonsterState.Warn;
        }

        if (distanceToInitial > wanderRadius)
        {
            //朝向调整为初始方向
            targetRotation = Quaternion.LookRotation(initialPosition - trans.position, Vector3.up);
        }
    }

    /// <summary>
    /// 追击状态检测，检测敌人是否进入攻击范围以及是否离开警戒范围
    /// </summary>
    void ChaseRadiusCheck()
    {
        distanceToPlayer = Vector3.Distance(playerUnit.transform.position, trans.position);
        distanceToInitial = Vector3.Distance(trans.position, initialPosition);

        if (distanceToInitial > chaseRadius || distanceToPlayer > alertRadius )
        {
            currentState = MonsterState.Return;
        }
        else if (distanceToPlayer < attackRange && detectPlayer)
        {
            currentState = MonsterState.Attack;
        }
        //如果超出追击范围或者敌人的距离超出警戒距离就返回

    }

    /// <summary>
    /// 超出追击半径，返回状态的检测，不再检测敌人距离
    /// </summary>
    void ReturnCheck()
    {
        distanceToInitial = Vector3.Distance(trans.position, initialPosition);
        //如果已经接近初始位置，则随机一个待机状态
        if (distanceToInitial < 0.5f)
        {
            is_Running = false;
            RandomAction();
        }
    }

    public override void DefendAct()
    {
        throw new System.NotImplementedException();
    }

    public override void Idle()
    {
        throw new System.NotImplementedException();
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }
    public override void RandomAction()
    {
        //更新行动时间
        lastActTime = Time.time;

        //根据权重随机
        float number = Random.Range(0, actionWeight[0] + actionWeight[1] + actionWeight[2]);
        if (number <= actionWeight[0])
        {
            currentState = MonsterState.Idle;
            ani.SetTrigger("Idle");
        }
        else if (actionWeight[0] < number && number <= actionWeight[0] + actionWeight[1])
        {
            currentState = MonsterState.Check;
            ani.SetTrigger("Check");
        }
        else if (actionWeight[0] + actionWeight[1] < number && number <= actionWeight[0] + actionWeight[1] + actionWeight[2])
        {
            currentState = MonsterState.Walk;
            //随机一个朝向
            targetRotation = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
            ani.SetTrigger("Walk");
        }
        Debug.Log(number);
    }

    public override void Check()
    {
        playerUnit = GameObject.FindGameObjectWithTag("Player");
        //保存初始位置信息
        initialPosition = trans.position;
        //1. 自卫半径不大于警戒半径，否则就无法触发警戒状态，直接开始追击了
        defendRadius = Mathf.Min(alertRadius, defendRadius);
        //2. 攻击距离不大于自卫半径，否则就无法触发追击状态，直接开始战斗了
        attackRange = Mathf.Min(defendRadius, attackRange);
        //3. 游走半径不大于追击半径，否则怪物可能刚刚开始追击就返回出生点
        wanderRadius = Mathf.Min(chaseRadius, wanderRadius);
        //随机一个待机动作
        RandomAction();
    }


}
