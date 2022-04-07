using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private GameObject popup_Setting;
    private bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        popup_Setting = GameObject.Find("Popup_Settings");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOpen)
        {
            popup_Setting.SetActive(false);
        }
        else
        {
            popup_Setting.SetActive(true);
        }
    }

    #region 버튼이벤트
    public void btn_Setting()
    {
        isOpen = true;
    }
    public void btn_SettingClose()
    {
        isOpen = false;
    }
    #endregion
}
