using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionText_S : MonoBehaviour
{
    [SerializeField]
    private GameController_S gameController;
    [SerializeField]
    private float textFlashNum = 3;
    [SerializeField]
    private Color defColor = Color.blue;

    private Text questText;
    private QnA_S QnA;
    


    private void Awake()
    {
        questText = GetComponent<Text>();
        questText.color = defColor;
    }


    // ** Methods **
    public void wrongSequence()
    {
        StartCoroutine(wrongCoroutine());
    }

    public void rightSequence()
    {
        StartCoroutine(rightCoroutine());
    }


    // ** Set Methods **
    public void setQnA(QnA_S newQnA)
    {
        QnA = newQnA;
        questText.text = QnA.question;
    }


    private void setColor(Color newColor)
    {
        defColor = newColor;
    }


    // ** Coroutine **
    IEnumerator wrongCoroutine()
    {
        questText.text = "wrong";

        for (int i = 0; i < textFlashNum; i++)
        {
            questText.color = Color.red;
            yield return new WaitForSecondsRealtime(0.2f);
            questText.color = Color.white;
            yield return new WaitForSecondsRealtime(0.2f);
        }
        questText.color = defColor;
        questText.text = QnA.question;
    }


    IEnumerator rightCoroutine()
    {
        questText.text = "correct";

        for (int i = 0; i < textFlashNum; i++)
        {
            questText.color = Color.white;
            yield return new WaitForSecondsRealtime(0.2f);
            questText.color = Color.green;
            yield return new WaitForSecondsRealtime(0.2f);
        }
        questText.color = defColor;
        questText.text = "";
    }
}
