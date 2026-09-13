using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    /// <summary>
    /// 当前选项组件
    /// </summary>
    public Toggle thisToggle;
    /// <summary>
    /// 选项的内容文本
    /// </summary>
    public Text optionText;
    /// <summary>
    /// 选项对应的分数
    /// </summary>
    public int score;
    /// <summary>
    /// 选项的状态
    /// </summary>
    public bool IsSelect = false;

    public void Init(AnswerData answerData)
    {
        optionText.text = answerData.option;
        score = answerData.Score;
        thisToggle.onValueChanged.AddListener((isSelect) => { IsSelect =isSelect; });
    }
}

