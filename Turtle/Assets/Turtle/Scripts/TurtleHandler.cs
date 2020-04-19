using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleHandler : MonoBehaviour
{
    

    public NavMeshAgent agent;
    public MonsterAby aby;
    public TurtleRaycast tRay;
    private Animator ani;
    private Transform trans;//初始位置
    

    private string name = "刺刺龜";

    private float hp = 100;
    private float speed = 10;
    private float attackNum = 20;
    private float defendNum = 50;

    


    private void Awake()
    {
        
        agent = GetComponent<NavMeshAgent>();
        trans = GetComponent<Transform>();
        ani = GetComponent<Animator>();       
        aby = new TurtleAby(name, hp, speed, attackNum, defendNum, ani, trans,agent);
        tRay = new TurtleRaycast(transform,aby);

    }
    // Start is called before the first frame update
    void Start()
    {             
        aby.Check();
    }

    // Update is called once per frame
    void Update()
    {
        tRay.LookDetect();
        aby.ChangeState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<Player>().hp-=20;
        }
    }
}
