using UnityEngine;
using System.IO;
using System.Xml;
public class XmlTest : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        CreateXml();
    }
    /// 
    /// Creates the xml.
    /// 
    private void CreateXml()
    {
        //设置保存路径
        string path = Application.dataPath + "/XML/" + "ConfigFile.xml";
        //判断文件是否存在
        if (File.Exists(path) == false)
        {
            //创建一个xml文件
            XmlDocument xml = new XmlDocument();
            //创建最上层节点
            XmlElement root = xml.CreateElement("Root");
            //创建子节点
            XmlElement element = xml.CreateElement("Question");
            element.SetAttribute("SelectType", "True");

            //创建子节点的第一个子节点，设置属性并添加内容
            XmlElement Child1 = xml.CreateElement("Problem");
            Child1.InnerText = "这里输入您的题目";

            //创建子节点的第二个子节点，设置属性并添加内容
            XmlElement Child2 = xml.CreateElement("Answer");

            //创建三级子节点
            XmlElement item1 = xml.CreateElement("Item");
            item1.SetAttribute("option", "A.答案一");
            item1.InnerText = "0";

            XmlElement item2 = xml.CreateElement("Item");
            item2.SetAttribute("option", "B.答案一");
            item2.InnerText = "0";

            XmlElement item3 = xml.CreateElement("Item");
            item3.SetAttribute("option", "C.答案一");
            item3.InnerText = "1";

            XmlElement item4 = xml.CreateElement("Item");
            item4.SetAttribute("option", "D.答案一");
            item4.InnerText = "0";

            //二级子节点
            XmlElement Child3 = xml.CreateElement("Analysis");
            Child3.InnerText = "这里输入解析";

            //创建子节点
            XmlElement element2 = xml.CreateElement("Question");
            element2.SetAttribute("SelectType", "True");

            //创建子节点的第一个子节点，设置属性并添加内容
            XmlElement Child2_1 = xml.CreateElement("Problem");
            Child2_1.InnerText = "这里输入您的题目gvfrebr";

            //创建子节点的第二个子节点，设置属性并添加内容
            XmlElement Child2_2 = xml.CreateElement("Answer");

            //创建三级子节点
            XmlElement item2_1 = xml.CreateElement("Item");
            item2_1.SetAttribute("option", "A.答案一");
            item2_1.InnerText = "0";

            XmlElement item2_2 = xml.CreateElement("Item");
            item2_2.SetAttribute("option", "B.答案44");
            item2_2.InnerText = "0";

            XmlElement item2_3 = xml.CreateElement("Item");
            item2_3.SetAttribute("option", "C.答案3");
            item2_3.InnerText = "1";

            XmlElement item2_4 = xml.CreateElement("Item");
            item2_4.SetAttribute("option", "D.答案一");
            item2_4.InnerText = "0";

            //二级子节点
            XmlElement Child2_3 = xml.CreateElement("Analysis");
            Child2_3.InnerText = "这里输入解析";

            //把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
            element2.AppendChild(Child2_1);
            Child2_2.AppendChild(item2_1);
            Child2_2.AppendChild(item2_2);
            Child2_2.AppendChild(item2_3);
            Child2_2.AppendChild(item2_4);
            element2.AppendChild(Child2_2);
            element2.AppendChild(Child2_3);
            root.AppendChild(element2);

            element.AppendChild(Child1);
            Child2.AppendChild(item1);
            Child2.AppendChild(item2);
            Child2.AppendChild(item3);
            Child2.AppendChild(item4);
            element.AppendChild(Child2);
            element.AppendChild(Child3);
            root.AppendChild(element);
            xml.AppendChild(root);
            //保存XML文档
            xml.Save(path);
            Debug.Log("Xml 创建成功!");
        }
    }
}
