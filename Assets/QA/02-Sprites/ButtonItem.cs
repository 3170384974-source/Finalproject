using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour
{
    Button Btn;


    // Start is called before the first frame update
    void Start()
    {
        Btn = GetComponent<Button>();
        Btn.onClick.AddListener(ThisBtn);
    }

    public void ThisBtn()
    {
        Panel_Question.GetInstance().ThisBtn(this);
    }

}
