using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Ball cs_Ball;
    [Header("�������ͽ�")]
    [SerializeField] private float enemyMaxHp = 0;
    [SerializeField] private float damage = 0;
    private float enemyHp = 0;
    [Header("��ü�� ����")][SerializeField] private float mass = 1000;
    private Rigidbody rb;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        cs_Ball = FindObjectOfType<Ball>();
        enemyHp = enemyMaxHp;
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        startPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        OnDestruction();
    }

    public void EnemyReset()
    {
        enemyHp = enemyMaxHp;
        rb.angularVelocity = new Vector3(0, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        this.gameObject.transform.position = startPos;
        this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        this.gameObject.SetActive(true);
    }

    private void OnDestruction()
    {
        if(enemyHp <= 0)
        {
            // ���⿡ �ı��Ǵ� ��ƼŬ �߰��ϸ� �ɵ�
            this.gameObject.SetActive(false);
            cs_Ball.AddScore(100);
        }
    }

    private void InflictDamage()
    {
        cs_Ball.Hp -= damage;
        Debug.Log("Ball HP : " + cs_Ball.Hp);
    }

    private void TakeDamage()
    {
        enemyHp -= cs_Ball.Damage;
        Debug.Log("Enemy HP : " + enemyHp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Ball"))
        {
            InflictDamage();
            TakeDamage();
            cs_Ball.AddScore(10);
        }
        else if(collision.gameObject.tag.Equals("Enemy"))
        {
            rb.angularVelocity = new Vector3(0, 0, 0);
        }
    }
}
