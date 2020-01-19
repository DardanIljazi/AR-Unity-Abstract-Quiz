using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectQuizzNoteScript : MonoBehaviour
{
    [TextArea(10, 50)]
    public string Notes = "If you change the API Struture (for example renaming ApiData.QuizzIndex to something else): \n" +
        "Please find the SelectQuizzData.cs file and modify the inheritance class to the new one. \n\n"+
        "From: \"public class SelectQuizzData : ApiData.IndexQuizz\"\n TO \n ==> \"public class SelectQuizzData : {YOUR_NEW_DATA_STRUCTURE_CLASS}\"";
    
}
