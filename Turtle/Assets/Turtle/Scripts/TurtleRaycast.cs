using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleRaycast : Property
{
    private Transform transformSelf;
    private MonsterAby tAby;
    public TurtleRaycast(Transform trans,MonsterAby tby)
    {
        
        lookAngle = 160f;
        lookAccurate = 40;
        subAngle = (lookAngle / 2) / lookAccurate;
        transformSelf = trans;
        tAby = tby;
    }
    //放射线检测
    public bool Look()
    {
        
        //一条向前的射线
        if (LookAround(Quaternion.identity, Color.green))
            return true;

        //多一个精确度就多两条对称的射线,每条射线夹角是总角度除与精度
        
        for (int i = 0; i < lookAccurate; i++)
        {
            if (LookAround( Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), Color.green)
                || LookAround( Quaternion.Euler(0, subAngle * (i + 1), 0), Color.green))
                return true;
        }

        return false;
    }

     public bool LookAround( Quaternion eulerAnger, Color DebugColor)
    {
        Debug.DrawRay(transformSelf.position, eulerAnger * transformSelf.forward.normalized * tAby.alertRadius, DebugColor);

        RaycastHit hit;
        if (Physics.Raycast(transformSelf.position, eulerAnger * transformSelf.forward, out hit, tAby.alertRadius) && hit.collider.CompareTag("Player"))
        {
            //controller.chaseTarget = hit.transform;
            return true;
        }
        return false;
    }


    public void LookDetect() 
    {
        if (Look() == true)       
            tAby.detectPlayer = true;       
        else
            tAby.detectPlayer = false;
    }



   
}
