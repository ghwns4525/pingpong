using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 touchPoint = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            // UI버튼을 누를때 이펙트가 안나오도록 설정
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                Instantiate(effect, touchPoint, effect.transform.rotation);
                effect.Play();
            }
        }
    }
}
