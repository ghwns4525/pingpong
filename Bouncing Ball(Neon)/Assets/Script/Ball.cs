using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ball : MonoBehaviour
{
    Enemy[] cs_Enemy;
    Clear[] cs_Clear;

    private Rigidbody rig;

    // ���� �߻��Ҷ� ���
    private Vector3 initPos;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 direction;
    private bool isFire = false;
    [SerializeField] private float speed = 1;
    [SerializeField] private float maxSpeed = 3;
    private Vector3 crnt_Force = new Vector3(0,0,0);

    // ���� ƨ�涧 ���
    Vector3 lastVelocity;

    // �� �������ͽ�
    [SerializeField] private float maxHp = 100;
    [SerializeField] private float damage = 10;
    private float hp = 100;
    private bool isGameOver = false;
    [SerializeField] private int life = 1;
    private int currentLife = 0;
    private bool isDie = false;

    // ����
    private int score = 0;
    private bool isgoal = false;
    private bool clear = false;
    private Vector3 clearPos = new Vector3(0,0,0);

    // ���ư� ���� Ȱ��ǥ
    [SerializeField]private GameObject img_Arrow;
    private float arrow_Angle = 0;
    private bool isArrow = false;

    private GameObject txt_Life;

    public float Hp { get => hp; set => hp = value; }
    public float Damage { get => damage; set => damage = value; }
    public int Score { get => score; set => score = value; }
    public bool IsFire { get => isFire; set => isFire = value; }
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public int CurrentLife { get => currentLife; set => currentLife = value; }
    public Vector3 Crnt_Force { get => crnt_Force;}
    public bool Isgoal { get => isgoal; set => isgoal = value; }

    // Start is called before the first frame update
    void Start()
    {
        cs_Clear = FindObjectsOfType<Clear>();
        cs_Enemy = FindObjectsOfType<Enemy>();
        rig = GetComponent<Rigidbody>();
        initPos = gameObject.transform.position;
        img_Arrow = GameObject.Find("Arrow");
        txt_Life = GameObject.Find("txt_Life");
        currentLife = life;
    }

    // Update is called once per frame
    void Update()
    {
        // �� �߻�
        if(!IsFire && !isDie)
        {
            Fire();
            txt_Life.SetActive(true);
        }
        else
        {
            txt_Life.SetActive(false);
        }
        // �� ����
        if (Input.GetKeyDown(KeyCode.Space) || GameManager.instance.IsRetry)
        {
            game_Reset();
        }
        // �� �ݻ� ����
        lastVelocity = rig.velocity;
        // ��� ����
        if(!isDie)
        {
            LifeDecrease();
        }
        // ���� ����
        GameOver();
        // ���� Ŭ����
        GameClear();
        
        // ���ư� ������ ȭ��ǥ �̹���
        img_Arrow.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        if(!isArrow)
        {
            img_Arrow.SetActive(false);
        }

        if(clear)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, clearPos, Time.deltaTime);
        }        
    }
    
    public void AddScore(int increaseScore)
    {
        Score += increaseScore;
    }

    public void ball_Reset()
    {
        rig.velocity = Vector3.zero;
        gameObject.transform.position = initPos;
        hp = maxHp;
        IsFire = false;
        isDie = false;
        isgoal = false;
        clear = false;
    }

    private void Fire()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = gameObject.transform.position;
            }
            if (Input.GetMouseButton(0))
            {
                // ���콺 ������ ���ȿ��� ȭ��ǥ ������ ����
                isArrow = true;
                img_Arrow.SetActive(true);

                // ���� ���ư� ���� ����
                ball_Way();

                // ���� ���� ������ ���� ȭ��ǥ ����
                Arrow_Way();
            }
            if (Input.GetMouseButtonUp(0))
            {
                ball_Trajectory();
                IsFire = true;
                isArrow = false;
            }
        }
    }

    private void game_Reset()
    {
        ball_Reset();
        for (int i = 0; i < cs_Enemy.Length; i++)
        {
            cs_Enemy[i].EnemyReset();
            Debug.Log(i + "��° Enemy Reset");
        }
        for (int i = 0; i < cs_Clear.Length; i++)
        {
            cs_Clear[i].ClearReset();
            Debug.Log(i + "��° Clear Reset");
        }
        Score = 0;
        currentLife = life;
        isGameOver = false;
        GameManager.instance.IsClear = false;
        GameManager.instance.IsRetry = false;
        GameManager.instance.CurrentClearCount = GameManager.instance.ClearCount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Clear")
        {
            clearPos = collision.transform.position;
            clear = true;
        }
        else
        {
            var dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            var speed = lastVelocity.magnitude;

            rig.velocity = dir * Mathf.Max(0, speed);
        }

        // collision.contacts[0].point => ó�� �΋H�� ����
        ParticlaManager.instance.HitParticle(collision.contacts[0].point);
    }

    private void LifeDecrease()
    {
        if(Hp <= 0 || isgoal)
        {
            rig.velocity = new Vector3(0, 0, 0);
            currentLife--;            
            isDie = true;
            ball_Reset();
        }
    }
    
    private void GameOver()
    {
        if(currentLife <= 0)
        {
            rig.velocity = new Vector3(0, 0, 0);
            isGameOver = true;
        }
    }

    private void GameClear()
    {
        if(GameManager.instance.IsClear)
        {
            rig.velocity = new Vector3(0, 0, 0);
            HighScore();
        }
    }

    private void ball_Trajectory()
    {
        if (-direction.x < 0 && -direction.y < 0)
        {
            rig.velocity = new Vector2(Mathf.Max(-maxSpeed, -ball_Way().x) * speed, Mathf.Max(-maxSpeed, -ball_Way().y) * speed);
        }
        else if (-direction.x > 0 && -direction.y < 0)
        {
            rig.velocity = new Vector2(Mathf.Min(maxSpeed, -ball_Way().x) * speed, Mathf.Max(-maxSpeed, -ball_Way().y) * speed);
        }
        else if (-direction.x < 0 && -direction.y > 0)
        {
            rig.velocity = new Vector2(Mathf.Max(-maxSpeed, -ball_Way().x) * speed, Mathf.Min(maxSpeed, -ball_Way().y) * speed);
        }
        else if (-direction.x > 0 && -direction.y > 0)
        {
            rig.velocity = new Vector2(Mathf.Min(maxSpeed, -ball_Way().x) * speed, Mathf.Min(maxSpeed, -ball_Way().y) * speed);
        }
        crnt_Force = rig.velocity;
    }

    private Vector3 ball_Way()
    {
        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        direction = endPos - startPos;
        if(direction.x >= 3)
        {
            direction.x = 3.0f;
        }
        else if(direction.x <= -3)
        {
            direction.x = -3.0f;
        }
        if(direction.y >= 3)
        {
            direction.y = 3.0f;
        }
        else if (direction.y <= -3)
        {
            direction.y = -3.0f;
        }
        return direction;        
    }

    private void Arrow_Way()
    {
        arrow_Angle = Mathf.Atan2(ball_Way().y, ball_Way().x) * Mathf.Rad2Deg;        
        img_Arrow.transform.rotation = Quaternion.AngleAxis(arrow_Angle - 180, Vector3.forward);

        float dx = 0;
        float dy = 0;

        if (direction.x < 0)
        {
            dx = -direction.x/20;            
        }
        else if(direction.x > 0)
        {
            dx = direction.x / 20;
        }
        
        if(direction.y < 0)
        {
            dy = -direction.y / 20;
        }
        else if(direction.y > 0)
        {
            dy = direction.y / 20;
        }

        // ���� ���콺�� ���� �Ÿ����
        float d = Mathf.Sqrt(Mathf.Pow(this.transform.position.x - (float)Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 2) + Mathf.Pow(this.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 2)) / 5;
        if(d > 0.3)
        {
            d = 0.3f;
        }

        img_Arrow.transform.localScale = new Vector3(d, 0.3f);
    }

    private void HighScore()
    {
        if(ScoreManager.instance.HighScore[StageManager.instance.ChapterNum, StageManager.instance.StageNum] < score)
        {
            ScoreManager.instance.HighScore[StageManager.instance.ChapterNum, StageManager.instance.StageNum] = score;
            PlayerPrefs.SetInt("HighScore_"+ StageManager.instance.ChapterNum + "_" + StageManager.instance.StageNum, ScoreManager.instance.HighScore[StageManager.instance.ChapterNum, StageManager.instance.StageNum]);
            PlayerPrefs.Save();
        }
    }
}
