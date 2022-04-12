using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    Ball cs_Ball;

    // Start is called before the first frame update
    void Start()
    {
        cs_Ball = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InflictDamage()
    {
        cs_Ball.Hp -= 10;
        Debug.Log("HP : " + cs_Ball.Hp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Equals("Ball"))
        {
            InflictDamage();
            cs_Ball.AddScore(10);
        }
    }
}
