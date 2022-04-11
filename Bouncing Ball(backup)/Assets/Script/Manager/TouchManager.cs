using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField]SettingManager Sc_SettingManager;

    // Start is called before the first frame update
    void Start()
    {
        Sc_SettingManager = FindObjectOfType<SettingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Sc_SettingManager.IsSetting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ParticlaManager.instance.TouchParticle(Input.mousePosition);
            }
        }

    }
}
