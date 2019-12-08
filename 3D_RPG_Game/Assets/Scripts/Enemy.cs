﻿using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("基本屬性")]
    public float atk = 20f;
    public float hp = 250f;
    [Range(0f, 100f)]
    public float speed = 1.5f;
    [Range(0f, 100f), Tooltip("攻擊距離")]
    public float distanceATK = 3f;
    [Range(0f, 100f), Tooltip("追蹤距離")]
    public float distanceTrack = 12f;
    [Header("攻擊間隔"), Range(0f, 5f)]
    public float cd = 2.5f;
    [Header("攻擊距離"), Range(0f, 5f)]
    public float rangeAttack = 3f;
    [Header("傷害間隔"), Range(0f, 5f)]
    public float delayAttack = 1f;

    public int attack = 20;

    private float timer;


    Transform target;
    Animator ani;           // 動畫控制器
    NavMeshAgent agent;     // 代理器

    private void Start()
    {
        target = GameObject.Find("玩家").transform;
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;
    }

    private void Update()
    {
        Track();
    }

    /// <summary>
    /// 敵人行動範圍
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);                // 圖示.顏色
        Gizmos.DrawSphere(transform.position, distanceATK);     // 圖示.繪製球體範圍(中心點，半徑)

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, distanceTrack);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * rangeAttack);    // forward : 前方 Z、right : 右方 X、up : 上方 Y
    }

    private void Track()
    {
        float dis = Vector3.Distance(target.position, transform.position);
        //print("距離 : " + dis);

        if (dis <= distanceATK)
        {
            Attack();
        }
        else if (dis <= distanceTrack)
        {
            agent.isStopped = false;
            ani.SetBool("走路開關", !agent.isStopped);
            agent.SetDestination(target.position);               // 代理器.設定目標位置
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        agent.isStopped = true;
        ani.SetBool("走路開關", !agent.isStopped);
    }

    private void Attack()
    {
        if (timer >= cd)
        {
            timer = 0;
            agent.isStopped = true;
            ani.SetTrigger("攻擊觸發");
            Invoke("DelayAttack", delayAttack);
        }
        else
        {
            timer += Time.deltaTime;
            Idle();
        }
    }

    private void DelayAttack()
    {
        RaycastHit hit;    // 射線碰撞資訊
        // 物理.射線碰撞(起點，方向，射線碰撞資訊，長度)
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward,out hit, rangeAttack))
        {
            if (hit.collider.gameObject.name == "遊俠")
            {
                hit.collider.GetComponent<PlayerController>().Hit(attack);
            }
            //print(hit.collider.gameObject);
        }
    }

    private void Hit()
    {

    }

    private void Dead()
    {

    }
}
