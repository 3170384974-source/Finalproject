using UnityEngine;

/// <summary>
/// 全局静态类，用来定义静态字段，方便调用
/// </summary>
public class DataPath
{
    // 打包后从 StreamingAssets 读取（只读，适合发布）
    public static string QuestionData = Application.streamingAssetsPath + "/XML/ConfigFile.xml";

    // 编辑器下也可保留原路径用于调试（可选）
    // public static string QuestionData = "file://" + Application.dataPath + "/XML/ConfigFile.xml";

    public static string QuestionText = "QuestionText";
}