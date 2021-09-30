using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "9", menuName = "QuestionandAnswer")]
public class QnA_S : ScriptableObject
{
    public int id = 0;

    public int difficulty = 0;

    public string question = "";

    public string answer = "";

    public string fakeAnswer1 = "";

    public string fakeAnswer2 = "";

    public string fakeAnswer3 = "";
}
