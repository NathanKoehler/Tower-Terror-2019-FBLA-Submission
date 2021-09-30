using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAnswer_S : Collectable_S
{
    [SerializeField]
    private int answerID = 0;
    [SerializeField]
    private string answerText = "";


    private Color defColor;
    private TextMesh collectableText;

    private void Start()
    {
        collectableText = GetComponent<TextMesh>();
        collectableText.text = answerText;

        defColor = collectableText.color;
    }


    // ** Methods **
    public void ColorFlash(int flashes, Color flashColor)
    {
        StartCoroutine(ColorCoroutine(flashes, flashColor));
    }

    
    // ** Get Methods **
    public int getID()
    {
        return answerID;
    }


    // ** Set Methods **
    public void setID(int id, string text)
    {
        answerID = id;
        answerText = text;
    }


    // ** Coroutine **
    IEnumerator ColorCoroutine(int numOfFlashes, Color newColor)
    {
        for (int i = 0; i < numOfFlashes; i++)
        {
            collectableText.color = newColor;
            yield return new WaitForSecondsRealtime(0.2f);
            collectableText.color = defColor;
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
