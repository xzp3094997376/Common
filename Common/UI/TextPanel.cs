using UnityEngine;

public class TextPanel : MonoBehaviour
{
    public static TextPanel instance { get; private set; } = null;

    string[] str;
    TipPanel tipPanel;
    private void Start()
    {
        string path = Application.streamingAssetsPath + "/step.txt";
        str = FileReader.ReadFileLines(path);
        tipPanel = GetComponentInChildren<TipPanel>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //SetTip();
        }
    }
    /// <summary>
    ///设置黑板文字
    /// </summary>
    public void ShowBlackBoardStr()
    {

    }
    private void Awake()
    {
        instance = this;
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

}
