using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("對話介面")]
    public GameObject dialog;
    public Text textDialog;
    [Header("對話內容")]
    public string dialogStart = "陌生人你好，請問可以幫我找到十個零件嗎？";
    public string dialogNotComplete = "請問你還沒找到十個零件嗎？";
    public string dialogComplete = "陌生人，謝謝你幫我找到十個零件。";
    [Header("對話速度"), Range(0.001f, 5.5f)]
    public float dialogSpeed = 0.05f;

    // 定義列舉
    public enum npcState
    {
        start, notComplete, complete
    }
    // 實例化列舉
    public npcState _npcState;

    /// <summary>
    /// 對話開始
    /// </summary>
    private void DialogStart()
    {
        dialog.SetActive(true);
        StopAllCoroutines();                        // 停止所有協同程序

        switch (_npcState)
        {
            case npcState.start:
                StartCoroutine(ShowDialog(dialogStart));
                break;
            case npcState.notComplete:
                StartCoroutine(ShowDialog(dialogNotComplete));
                break;
            case npcState.complete:
                StartCoroutine(ShowDialog(dialogComplete));
                break;
        }

        /**
        int lv = 1;
        // 判斷式 switch
        switch (lv)
        {
            case 1:
                print("1 等");
                break;
            case 10:
                print("10 等");
                break;
            default:
                print("未定義");
                break;
        }
        */
    }

    /// <summary>
    /// 顯示對話內容：打字效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowDialog(string dialog)
    {
        textDialog.text = "";

        // for (初始值；條件(布林值)；迭代器
        for (int i = 0; i < dialog.Length; i++)
        {
            textDialog.text += dialog[i].ToString();
            yield return new WaitForSeconds(dialogSpeed);
        }
    }

    /// <summary>
    /// 對話結束
    /// </summary>
    private void DialogEnd()
    {
        dialog.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        DialogStart();
    }

    private void OnTriggerExit(Collider other)
    {
        DialogEnd();
    }
}
