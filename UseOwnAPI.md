# Use your own API in Abstract Quiz AWA

To implement your API, you have to implement:

- A class that inherits from `ApiManager`
- A class that sets the structure of the api (known as `Api Model`) and is defined as a member in ApiManager

### Where to find examples ?

You can find examples in the project under `Assets/Quizz/API/01Apis/` folder.

### Type of data structure

Before you begin, you have to know how your API is structured in order to make it work with the code. There is 3 possible values:

- `Each resource has en endpoint`
- `Partially nested`
- `Fully nested`

#### Each resource has an endpoint

This is the way the code will work better and faster. It has originally been thought to work like this. So if you have an API that is RESTful, you will have to do less job, for example:

| Resource  | Endpoint                                                     |
| --------- | ------------------------------------------------------------ |
| Quizzes   | http://dardi.ch/each_resource_has_endpoint/api/quizzes       |
| Questions | http://dardi.ch/each_resource_has_endpoint/api/quizzes/{quizzId}/questions |
| Answers   | http://dardi.ch/each_resource_has_endpoint/api/quizzes/{quizzId}/questions/{questionid}/answers |

In the project, `DardiEachResourceHasAnEndpoint` api is structured in this way.

#### Partially nested

Happens when some resources have their own endpoint and others are nested/inside an other resource endpoint, for example:

| Resource            | Endpoint                               |
| ------------------- | -------------------------------------- |
| Quizzes             | www.example.com/api/quizzes            |
| Questions + Answers | (www.example.com/api/quizzes/{quizzId} |

In the project, `Heroku` api is structured in this way.

#### Fully nested

Happens when all resources are in the same place. This is not recommended because more code will have to be written and it will be slower, but it works if needed, for example:

| Resource                      | Endpoint                                |
| ----------------------------- | --------------------------------------- |
| Quizzes + Questions + Answers | https://dardi.ch/nested_api/api/quizzes |

In the project, `DardiNested` api is structured in this way.

Now that you have an idea of the structure, let's go and see what is Api Manager and how it works.

## ApiManager

The Api Manager is an abstract class that defines all basics methods and possible values/parameters that will be used from the application logic and to access API. For example it contains: `GetQuizzes`, `GetQuestions`, `GetAnswers` that return the list of corresponding datas.



## Api Model

The structure of the data returned by API has to be defined in a class with exactly the same structure and types returned by the API. The reason for that is that data will be parsed with `JsonUtility.FromJson<T>()`. For that to work, `[Serializable]` has to be used before the class.

For example the data returned for any `Quizz` from API:

```json
{
    "id": 1,
    "title": "Additions Quizz"
 }
```

Will have to be defined in API Model has the class below:

```c#
[Serializable]
public class QuizzInApi {
    public int id;
    public string title;
}
```

---

An other example with `Question`:

```json
{
	"id": 1,
	"question": "How many do 1+1 equal ?",
	"quizzId" : 1
}
```

Will have to be defined in API Model has the class below:

```c#
[Serializable]
public class QuestionInApi {
    public int id;
    public string question;
    public int quizzId;
}
```

---

But this is not all, to go further, each class has to be inherited from the abstract representation class. For example, The class `QuizzInApi` represents a `Quizz`. So it should be inherited from it like this:

```c#
[Serializable]
public class QuizzInApi : Quizz // Inherit from Quizz because this class represents a Quizz {
    public int id;
    public string title;
}
```

The same thing has to be done for others classes, for example `QuestionInApi` represents a `Question` and must inherit from it:

```c#
[Serializable]
public class QuestionInApi : Question // Inherit from Question because this class represents a Question {
    public int id;
    public string question;
    public int quizzId;
}
```



## Example

For this example, we created a folder `DardiEachResourceHasEndpoint` into `Assets/Quizz/API/01Apis/`  and created 2 files in it :

- `DardiEachResourceHasEndpointApi.cs` : Will contain the class that inherits from Api Manager
- `DardiEachResourceHasEndpointApiModel.cs` : Will contain the structure of the data in

Our api root link will be: `http://dardi.ch/each_resource_has_endpoint/api/` and is of the type `Each resource has an endpoint`. The api doesn't need a token to connect to the API and is restful.

---

Here is the code we will put into `DardiEachResourceHasEndpointApi.cs`:

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

/**
 * DardiNestedApi does: 
 *  - Define which model it is attached to (here it is DardiEachResourceHasEndpointApiModel)
 *  - Configure/Set how the code should get data
 * 
 * The apis like this one that have one endpoint (url) per model are the easiest to implement. Others: partially or fully nested api have to have more code written for the same result
 */
public class DardiEachResourceHasEndpointApi : ApiManager
{
    public DardiEachResourceHasEndpointApiModel dardiEachResourceHasEndpointApiModel;

    public DardiEachResourceHasEndpointApi()
    {
        base.SetChild(this); // Important, must be set so that ApiManager will call Serialize(Quizzes/Questions/Answers)..

        // Set the configuration needed to get data for Quizzes/Questions/Ansers. 
        base.rootApiUrl = "http://dardi.ch/each_resource_has_endpoint/api";
        base.apiQuizzesUrl = rootApiUrl + "/quizzes";
        base.apiQuestionsUrl = rootApiUrl + "/quizzes/{quizzId}/questions";
        base.apiAnswersUrl = rootApiUrl + "/quizzes/{quizzId}/questions/{questionId}/answers";

        base.apiDataModelEndpointType = ApiDataModelEndpointType.EachModelHasAnEndpoint;

        // Set the configuration needed for the API token.
        base.hasToHaveTokenForApi = false;
        base.hasToLoginToGetToken = false;
    }

    public override Quizzes SerializeQuizzes(string jsonData)
    {
        return (Quizzes)JsonUtility.FromJson<DardiEachResourceHasEndpointApiModel.QuizzesInAPI>(jsonData);
    }

    public override Questions SerializeQuestions(string jsonData)
    {
        return (Questions)JsonUtility.FromJson<DardiEachResourceHasEndpointApiModel.QuestionsInAPI>(jsonData);
    }

    public override Answers SerializeAnswers(string jsonData)
    {
        return (Answers)JsonUtility.FromJson<DardiEachResourceHasEndpointApiModel.AnswersInAPI>(jsonData);
    }
}

```



On line `34, 39, 44`: `public override Quizzes SerializeQuizzes(string jsonData)`, `public override Questions SerializeQuestions...`, `...`

We override the method of serialization used into the ApiManager. Indeed, ApiManager and all the application has no idea of how to serialize the data coming from API. Only `DardiEachResourceHasEndpointApi` and `DardiEachResourceHasEndpointApiModel` have and idea of it for this API. So here, we use `JsonUtiliy.FromJson` to serialize it in the good format and explicitly converting it to the right class.

---

Here is the code we will put into `DardiEachResourceHasEndpointApi.cs`:

```c#
using System;
using System.Collections.Generic;
using UnityEngine;
using static AbstractQuizzStructure;

/**
 *  DardiEachResourceHasEndpointApiModel is the class that defines how is structured the API data for Dardi Each Resource Has Endpoint
 *  - It maps the data from API to the classes that are used everywhere in the application logic (Quizzes/Quizz/Questions/Question/Answers/Answer)
 */
public class DardiEachResourceHasEndpointApiModel : MonoBehaviour
{
    [Serializable]
    public class QuizzesInAPI : Quizzes
    {
        public List<QuizzInAPI> quizzes;

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuizzInAPI quizzData in this.quizzes)
            {
                quizzData.MapAPIValuesToAbstractClass(); // Map values
                base.AddQuizz(quizzData);
            }
        }
    }

    [Serializable]
    public class QuizzInAPI : Quizz
    {
        public int id;
        public string title;
        public string description;

        public override void MapAPIValuesToAbstractClass()
        {
            base.SetQuizzId(this.id);
            base.SetQuizzTitle(this.title);
        }
    }

    [Serializable]
    public class QuestionsInAPI : Questions
    {
        public List<QuestionInAPI> questions;

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuestionInAPI questionData in this.questions)
            {
                questionData.MapAPIValuesToAbstractClass(); // Map values
                base.AddQuestion(questionData);
            }
        }
    }

    [Serializable]
    public class QuestionInAPI : Question
    {
        public int id;
        public string question;
        public int quizzId;

        public override void MapAPIValuesToAbstractClass()
        {
            base.SetQuestionid(this.id);
            base.SetQuestionText(this.question);
        }
    }


    [Serializable]
    public class AnswersInAPI : Answers
    {
        public List<AnswerInAPI> answers;

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (AnswerInAPI answerData in this.answers)
            {
                answerData.MapAPIValuesToAbstractClass(); // Map values
                base.AddAnswer(answerData);
            }
        }
    }

    [Serializable]
    public class AnswerInAPI : Answer
    {
        public int id;
        public string answer;
        public bool rightAnswer;
        public int questionId;
        public int quizzId;

        public override void MapAPIValuesToAbstractClass()
        {
            base.SetDataToShowAsPossibleAnswer(this.answer);
            base.SetIsCorrectAnswer(this.rightAnswer);
        }
    }
}



#region Example of json to class with JsonUtility.FromJson<Class>(JSON_DATA)
/**
 *
 * For example:
 * JSON_DATA: 
 * 
 * {
 *   "data": [
 *      {
 *          "id": 1,
 *          "title": "quizzTitle
 *          "description": "quizzDescription",
 *          "image": "https://i.ytimg.com/vi/U7VmBgp9D9o/maxresdefault.jpg"
 *      },
 *      {
 *          "id": 2,
 *          "title": "quizzTitle
 *          "description": "quizzDescription",
 *          "image": "https://i.ytimg.com/vi/U7VmBgp9D9o/maxresdefault.jpg"
 *      }
 *   ]
 * }
 *  
 *  --> Could be declared as the classes bellow:
 *  ---------------------------------------------------------------------------------
 *  
 *  [Serializable]
 *  public class QuizzesData 
 *  {
 *      public List<QuizzData> data = new List<QuizzData>();
 *  }
 *  
 *  [Serializable]
 *  public class QuizzData 
 *  {
 *      public int id;
 *      public string title;
 *      public string description;
 *      public string image;
 *  }
 *  
 *  --> And used like this:
 *  ---------------------------------------------------------------------------------
 *  
 *   QuizzesData quizzesData = JsonUtility.FromJson<QuizzesData>(JSON_DATA);
 *  
 */

#endregion


```



Let's make a parallel with json. For example:

```c#
[Serializable]
    public class QuizzesInAPI : Quizzes
    {
        public List<QuizzInAPI> quizzes;

        public override void MapAPIValuesToAbstractClass()
        {
            foreach (QuizzInAPI quizzData in this.quizzes)
            {
                quizzData.MapAPIValuesToAbstractClass(); // Map values
                base.AddQuizz(quizzData);
            }
        }
    }
```

Represent the json:

```json
{
    "quizzes": [
        #{...}
    ]
}

```

And

```c#
[Serializable]
public class QuizzInAPI : Quizz
{
    public int id;
    public string title;
    public string description;

    public override void MapAPIValuesToAbstractClass()
    {
        base.SetQuizzId(this.id);
        base.SetQuizzTitle(this.title);
    }
}
```
Represent the data structure represented in the json above with #{...}

```json
{
    "id": 1,
    "title": "Additions",
    "description": "Do you know to make additions ?"
}
```



For more example and to understand how it works by example, go to  `Assets/Quizz/API/01Apis/DardiEachResourceHasEndpoint` folder.