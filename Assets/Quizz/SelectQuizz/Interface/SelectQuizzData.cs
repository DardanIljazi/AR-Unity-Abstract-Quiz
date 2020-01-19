using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * This class act as an "interface" between the ApiData.IndexQuizz (api data structure) and the data passed to the quizz cell (SelectQuizzData)
 * It is done this way so "less code" has to be changed in case there is some modification from data structure
 * 
 *   _title will always represent the "text"/"title" shown into cell views. So if you change ApiData.IndexQuizz and that ApiData.IndexQuizz.title is changed to something else, replace it in the constructor below
 */
public class SelectQuizzData : ApiData.IndexQuizz
{

}
