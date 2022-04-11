using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private GameObject popup_Setting;
    private bool isSetting = false;

    public bool IsSetting { get => isSetting; set => isSetting = value; }

    // Start is called before the first frame update
    void Start()
    {
        popup_Setting = GameObject.Find("Popup_Settings");
        popup_Setting.GetComponent<RectTransform>().localPosition = new Vector3(0, -1050);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSetting)
        {
            popup_Setting.SetActive(false);
            if(popup_Setting.GetComponent<RectTransform>().localPosition.y > -1050)
            {
                popup_Setting.transform.Translate(new Vector3(0, -0.1f, 0), Space.World);
            }
        }
        else
        {
            popup_Setting.SetActive(true);
            if (popup_Setting.GetComponent<RectTransform>().localPosition.y < 0)
            {
                popup_Setting.transform.Translate(new Vector3(0, 0.1f, 0), Space.World);
            }            
        }
    }

    #region 버튼이벤트
    public void btn_Setting()
    {
        isSetting = true;
    }
    public void btn_SettingClose()
    {
        isSetting = false;        
    }
    #endregion
}
