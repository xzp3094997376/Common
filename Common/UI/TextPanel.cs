using UnityEngine;
using UnityEngine.UI;

public class TextPanel : MonoBehaviour
{
    //public static TextPanel instance { get; private set; } = null;

    string[] str;
    TipPanel tipPanel;
    private void Start()
    {
        string path = Application.streamingAssetsPath + "/step.txt";
        str = FileReader.ReadFileLines(path);
        tipPanel = transform.GetChild(0).GetComponent<TipPanel>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //SetTip();
            SetBlackBoardStr("1", "漏斗口");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //SetTip();
            SetBlackBoardStr("2", "泥沙搅拌");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //SetTip();
            SetBlackBoardStr("3", "倾倒");
        }
        //
        if (Input.GetKeyDown(KeyCode.F))
        {
            //SetTip();
            SetBlackBoardStr("1", "倒置");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SetTip();
            SetBlackBoardStr("2", "隔开");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            //SetTip();
            SetBlackBoardStr("3", "烧杯");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            //SetTip();
            SetBlackBoardStr("4", "现象");
        }
    }
    /// <summary>
    ///设置黑板文字
    /// </summary>
    public void SetBlackBoardStr(string context)
    {
        Text str = transform.Find("black").GetComponent<Text>();
        if (str.text.Contains(context))
        {
            return;
        }
        str.text += "\n" + context;
    }

    string content;
    public string parentName="black";
    public void SetBlackBoardStr(string _start,string _end)
    {
        Text text= transform.Find(parentName)?.GetComponent<Text>();

        if (text==null)
        {
            return;
        }

        content =text .text;
        text.text= string.Empty;   
        //移除开头标记
        int headIndex= content.IndexOf(fontHead);
        if (headIndex!=-1)
        {
          content= content.Remove(headIndex,fontHead.Length);
        }

        //移除结尾标记
        int endInedx= content.IndexOf(fontEnd);
        if (endInedx!=-1)
        {
          content= content.Remove(endInedx,fontEnd.Length);
        }

        //在步骤开头插入标记
        int startIndex= content.IndexOf(_start);            
        content=content.Insert(startIndex,fontHead);

        //在步骤结尾标记
        int endIndex = content.IndexOf(_end);
        content =content.Insert(endIndex+_end.Length, fontEnd);

        //Debug.LogError(content);

        text.text = content;
    }
    /// <summary>
    /// 设置提示 1-7
    /// </summary>
    public void SetTip(int index = 1)
    {
        if (tipPanel == null)
        {
            Start();
        }
        index -= 1;
        string text = str[index % 7] + "\n" + str[(index + 1) % 7];
        tipPanel.SetText(text);
    }

    public void SetTip(string str, float delay = 2f)
    {
        if (tipPanel == null)
        {
            Start();
        }
        tipPanel.SetText(str);

        CancelInvoke("Hide");
        Invoke("Hide", delay);
    }
    void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }


    private string fontHead = "<color=yellow><size=70>";

    private string fontEnd = "</size></color>";

    private string returnLine = "\n";
}
