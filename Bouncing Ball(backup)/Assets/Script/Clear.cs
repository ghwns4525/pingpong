using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    Ball cs_Ball;
    private Rigidbody rb;
    private Vector3 startPos;
    private bool turn = false;

    private float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        cs_Ball = FindObjectOfType<Ball>();
        rb = GetComponent<Rigidbody>();
        startPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(turn)
        {
            rb.angularVelocity = new Vector3(0,180,0);
            if (timer < 1)
            {
                timer += Time.deltaTime;
            }
            else
            {
                this.gameObject.SetActive(false);
                cs_Ball.Isgoal = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Equals("Ball"))
        {
            ParticlaManager.instance.ClearParticle(this.transform.position);
            GameManager.instance.CurrentClearCount--;
            GetComponent<SphereCollider>().isTrigger = true;
            turn = true;
        }
    }

    public void ClearReset()
    {
        turn = false;
        timer = 0;
        rb.angularVelocity = new Vector3(0, 0, 0);
        this.gameObject.transform.position = startPos;
        this.gameObject.transform.rotation = new Quaternion(0,0,0,0);
        GetComponent<SphereCollider>().isTrigger = false;
        this.gameObject.SetActive(true);
    }
}
