using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Panel_Question : MonoBehaviour
{
    [Header("root界面：开始界面的root 答题界面的root 结束界面的root  提示界面的root")]
    [SerializeField] GameObject startRoot;
    [SerializeField] GameObject answerRoot;
    [SerializeField] GameObject endRoot;
    [SerializeField] GameObject hintRoot;

    [Header("按钮：开始答题 上一题，提交，下一题")]
    [SerializeField] Button startBtn;
    [SerializeField] Button previousBtn;
    [SerializeField] Button submitBtn;
    [SerializeField] Button nextBtn;
    [SerializeField] Button backToMainBtn;

    [Header("按钮：确认提交  取消提交")]
    [SerializeField] Button affirmSubBtn;
    [SerializeField] Button cancelSubBtn;

    [Header("文本：题目序号 解析内容 得分 倒计时  最后得分")]
    [SerializeField] Text questionID;
    [SerializeField] Text analysisData;
    [SerializeField] Text scoreTxt;
    [SerializeField] Text countDownTxt;
    [SerializeField] Text endTxt;

    [Header("内容scroll的content 单选scroll的content 选项scroll的content")]
    [SerializeField] Transform contentScrollContent;
    [SerializeField] Transform questionBtnRoot;
    [SerializeField] Transform selectContent;
    [SerializeField] Transform scrollView;

    [SerializeField] ToggleGroup questionGroup;

    // 答题界面数据内容
    private QuestionPanelData mQuestionPanelData;
    // 每一道题的题目内容
    private QuestionData mQuestionData;
    // 题目内容物体
    private GameObject mQuestion;
    // 选项的链表
    private List<Options> options = new List<Options>();

    //倒计时的总时间
    public float secound;

    [SerializeField] GameObject prefab;

    static Panel_Question instance;

    public static Panel_Question GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        Init();
        instance = this;
    }

    /// <summary>
    /// 按钮监听
    /// </summary>
    private void Init()
    {
        startBtn.onClick.AddListener(StartAnswer);
        previousBtn.onClick.AddListener(previousClick);
        submitBtn.onClick.AddListener(submitClick);
        nextBtn.onClick.AddListener(nextClick);
        backToMainBtn.onClick.AddListener(BackToMain);
        affirmSubBtn.onClick.AddListener(AffirmSub);
        cancelSubBtn.onClick.AddListener(CancelSub);
    }

    private void Start()
    {
        // 开始加载XML（使用协程）
        StartCoroutine(LoadingQuesiton(DataPath.QuestionData));

        scoreTxt.text = "得分: 0";
        countDownTxt.text = "答题时间还剩：" + secound;
        //界面的显示与隐藏
        startRoot.SetActive(true);
        answerRoot.SetActive(false);
        endRoot.SetActive(false);
        hintRoot.SetActive(false);
    }

    private void Update()
    {
        if (endRoot.activeSelf)
        {
            //如果倒计时结束前答题完成，关闭倒计时的协程,时间重新赋值
            StopCoroutine(TimeChange());
            secound = 0;
        }
    }

    #region 读取xml文件（使用 UnityWebRequest 兼容所有平台）
    IEnumerator LoadingQuesiton(string path)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(path))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string xmlText = www.downloadHandler.text;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlText);
                new QuestionPanel(doc.FirstChild);

                // 加载完成后，自动初始化第一题（可选，也可等用户点击开始）
                // 此处保留原逻辑：点击开始按钮后才显示题目
                Debug.Log("XML 加载成功，共 " + (QuestionPanel.questionPanelData?.Count ?? 0) + " 个面板数据。");
            }
            else
            {
                Debug.LogError("加载XML失败: " + www.error + "\n路径: " + path);
            }
        }
    }
    #endregion

    #region 初始化第一道题
    public void InitQuestionPanel(QuestionPanelData questionPanelData)
    {
        mQuestionPanelData = questionPanelData;
        CreateQuestion(questionPanelData.questionData[index]);
    }
    #endregion

    #region 创建题目
    bool isFirst = false;
    public void CreateQuestion(QuestionData questionData)
    {
        //数据赋值
        analysisData.text = "";
        mQuestionData = questionData;
        questionID.text = string.Format("第{0}题(共" + mQuestionPanelData.questionData.Count + "题)", index + 1);
        if (mQuestion != null)
        {
            Destroy(mQuestion);
        }

        //实例化题目预制体
        mQuestion = Instantiate(Resources.Load<GameObject>(DataPath.QuestionText));
        mQuestion.transform.SetParent(contentScrollContent);
        mQuestion.transform.localScale = Vector3.one;
        mQuestion.GetComponent<Text>().text = questionData.problem;

        if (options.Count > 0)
        {
            for (int i = 0; i < options.Count; i++)
            {
                Destroy(options[i].gameObject);
            }
        }
        options = new List<Options>();

        //实例化按钮选项组序列
        if (!isFirst)
            for (int i = 0; i < mQuestionPanelData.questionData.Count; i++)
            {
                Instantiate(prefab, scrollView);
                isFirst = true;
            }

        //当前题目的按钮序列的标识变大
        for (int i = 0; i < mQuestionPanelData.questionData.Count; i++)
        {
            if (i != index)
                scrollView.GetChild(i).gameObject.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            else
                scrollView.GetChild(i).gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.5f, 1.5f);
        }

        //实例化选项预制体
        for (int i = 0; i < questionData.answerData.Count; i++)
        {
            Options option = Instantiate(Resources.Load<Options>("Options"));
            option.Init(questionData.answerData[i]);
            option.transform.SetParent(selectContent);
            //如果是单选则设置为一个toggle组
            if (questionData.isSingleChoice)
            {
                option.thisToggle.group = questionGroup;
            }
            options.Add(option);
        }
    }
    #endregion

    #region 开始答题 上一题 下一题  提交按钮事件

    // 开始答题
    public void StartAnswer()
    {
        // 确保数据已加载
        if (QuestionPanel.questionPanelData == null || QuestionPanel.questionPanelData.Count == 0)
        {
            Debug.LogWarning("题目数据尚未加载完毕，请稍后再试。");
            return;
        }

        for (int i = 0; i < QuestionPanel.questionPanelData.Count; i++)
        {
            InitQuestionPanel(QuestionPanel.questionPanelData[i]);
        }
        startRoot.SetActive(false);
        answerRoot.SetActive(true);
        secound = 30;
        //开启倒计时
        StartCoroutine(TimeChange());
    }

    // 上一题点击事件
    private void previousClick()
    {
        if (index > 0)
        {
            index--;
            CreateQuestion(mQuestionPanelData.questionData[index]);
        }

        if (scrollView.GetChild(index).GetComponent<Button>().interactable == false)
            foreach (var item in options)
                item.thisToggle.interactable = false;
    }

    // 下一题点击事件
    private void nextClick()
    {
        if (index < mQuestionPanelData.questionData.Count - 1)
        {
            index++;
            CreateQuestion(mQuestionPanelData.questionData[index]);
            isFirstClick = false;
        }

        if (scrollView.GetChild(index).GetComponent<Button>().interactable == false)
            foreach (var item in options)
                item.thisToggle.interactable = false;
    }

    int score = 0;
    bool isFirstClick = false;      //是否是第一次答对，只有点击第一次提交的时候加分，再点提交不加分
    // 题目提交事件
    private void submitClick()
    {
        //遍历当前题目的选项，有选择的就可以提交核验答案，并显示解析内容
        foreach (var item in options)
        {
            if (item.thisToggle.isOn)
            {
                //回答正确加一分
                if (item.score > 0)
                {
                    analysisData.text = "回答正确";
                    if (!isFirstClick)
                    {
                        score += 1;
                        isFirstClick = true;
                    }
                    scoreTxt.text = "得分:" + score.ToString();
                }
                else
                    analysisData.text = "回答错误，解析：" + mQuestionData.Analysis;
            }
            //选择一个选项之后不能在选择其他选项
            item.thisToggle.interactable = false;
        }

        //如果当前题目是最后一题，提交之后，出现提示界面
        if (index + 1 == mQuestionPanelData.questionData.Count)
        {
            startRoot.SetActive(false);
            answerRoot.SetActive(true);
            endRoot.SetActive(false);
            hintRoot.SetActive(true);
        }

        //提交后，该题目不可再选择修改
        scrollView.GetChild(index).GetComponent<Image>().color = Color.green;
        scrollView.GetChild(index).GetComponent<Button>().interactable = false;
    }

    //返回主界面
    void BackToMain()
    {
        startRoot.SetActive(true);
        answerRoot.SetActive(false);
        endRoot.SetActive(false);
        score = 0;
        scoreTxt.text = "得分：" + score.ToString();

        for (int i = 0; i < scrollView.childCount; i++)
        {
            scrollView.GetChild(i).GetComponent<Image>().color = Color.white;
            scrollView.GetChild(i).GetComponent<Button>().interactable = true;
            Debug.Log(456789);
        }

        index = 0;
    }

    int index = 0;
    //缩列按钮事件
    public void ThisBtn(ButtonItem item)
    {
        for (int i = 0; i < scrollView.childCount; i++)
        {
            CreateQuestion(mQuestionPanelData.questionData[item.transform.GetSiblingIndex()]);
            index = item.transform.GetSiblingIndex();
        }
    }

    //确认提交 显示结束界面
    void AffirmSub()
    {
        startRoot.SetActive(false);
        answerRoot.SetActive(false);
        endRoot.SetActive(true);
        hintRoot.SetActive(false);
        endTxt.text = "答题结束\n 您的得分是：" + score.ToString();
    }

    //取消提交 返回原来的界面
    void CancelSub()
    {
        startRoot.SetActive(false);
        answerRoot.SetActive(true);
        endRoot.SetActive(false);
        hintRoot.SetActive(false);
    }
    #endregion

    #region 倒计时的协程
    IEnumerator TimeChange()
    {
        while (secound > 0)
        {
            yield return new WaitForSeconds(1);
            countDownTxt.text = "答题时间还剩：" + secound.ToString() + "秒";
            secound--;
        }
        if (secound <= 0)
        {
            //界面的显示与隐藏
            startRoot.SetActive(false);
            answerRoot.SetActive(false);
            endRoot.SetActive(true);
            endTxt.text = "答题结束\n 您的得分是：" + score.ToString();
        }
    }
    #endregion
}

/// <summary>
/// 答题panel数据类
/// </summary>
public class QuestionPanel
{
    public static List<QuestionPanelData> questionPanelData;
    public QuestionPanel(XmlNode node)
    {
        questionPanelData = new List<QuestionPanelData>();
        for (int i = 0; i < node.ChildNodes.Count; i++)
        {
            questionPanelData.Add(new QuestionPanelData(node.ChildNodes[i]));
        }
    }
}

/// <summary>
/// 答题界面数据类
/// </summary>
public class QuestionPanelData
{
    public List<QuestionData> questionData;
    public QuestionPanelData(XmlNode node)
    {
        questionData = new List<QuestionData>();
        for (int i = 0; i < node.ChildNodes.Count; i++)
        {
            questionData.Add(new QuestionData(node.ChildNodes[i]));
        }
    }
}

/// <summary>
/// 题目数据类
/// </summary>
public class QuestionData
{
    // 是否为单选，true为单选，false为多选
    public bool isSingleChoice;
    // 解析内容
    public string Analysis;
    // 题目内容
    public string problem;
    public List<AnswerData> answerData;
    public QuestionData(XmlNode node)
    {
        isSingleChoice = bool.Parse(node.Attributes["SelectType"].InnerText);
        Analysis = node["Analysis"].InnerText;
        problem = node["Problem"].InnerText;
        answerData = new List<AnswerData>();
        XmlNodeList nodelist = node["Answer"].ChildNodes;
        for (int i = 0; i < nodelist.Count; i++)
        {
            answerData.Add(new AnswerData(nodelist[i]));
        }
    }
}

/// <summary>
/// 答案数据类
/// </summary>
public class AnswerData
{
    // 选项的内容
    public string option;
    // 选项对应的分数
    public int Score;
    public AnswerData(XmlNode node)
    {
        option = node.Attributes["option"].InnerText;
        Score = int.Parse(node.InnerText);
    }
}