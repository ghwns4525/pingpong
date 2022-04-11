using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParticlaManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;

    public static ParticlaManager instance;
    // Start is called before the first frame update
    void Start()
    {
        ParticlaManager.instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void ClearParticle(Vector3 pos)
    {
        Vector3 touchPoint = new Vector3(pos.x, pos.y);
        Instantiate(particles[0], touchPoint, particles[0].transform.rotation);
        particles[0].Play();
    }

    public void HitParticle(Vector3 pos)
    {
        // 이펙트가 3개있는데 랜덤으로 나오도록
        int num = Random.Range(1,4);
        Vector3 touchPoint = new Vector3(pos.x, pos.y);
        Instantiate(particles[num], touchPoint, particles[num].transform.rotation);
        particles[num].Play();
    }

    public void TouchParticle(Vector3 pos)
    {
        Vector3 touchPoint = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, Camera.main.ScreenToWorldPoint(pos).y);
        // UI버튼을 누를때 이펙트가 안나오도록 설정
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Instantiate(particles[4], touchPoint, particles[4].transform.rotation);
            particles[4].Play();
        }
    }
}
