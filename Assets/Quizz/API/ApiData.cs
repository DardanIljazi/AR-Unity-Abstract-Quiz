using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * ApiData define classes that represents: API data classes, Classes used over application logic from those API data classes
 * 
 * The 2 types of classes in this file:
 *      [+] Classes that contains the word "Data" at the end (like ConnectionData, QuizzesData, ...Data)
 *              - They represent the exact structure of json API data (response).
 *              --> If they are not the same as the API response, the serialization won't work
 *      
 *      [+] The other classes (like ApiToken, Quizzes, Questions, Question...)
 *              - Those classes inherits from "Data" classes and are used in application logic
 *              --> This ensures that if the API changes, only the "Data" classes must be changed and the application logic stay the same
 */

public static class ApiData
{
    
}
