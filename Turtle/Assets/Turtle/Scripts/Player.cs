using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp;
    // Start is called before the first frame update
    void Start()
    {
        hp = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hp);
    }
}
