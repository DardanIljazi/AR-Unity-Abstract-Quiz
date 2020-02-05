# How the project works

From the beginning, the goal of the project was to be:

- Easy to add or modify elements
- Make easy to change the backend API
- Be oriented "programmer" so that if someone needs to implement a new API, less code has to be written

### API Model

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

**The example above are not fully complete and they have been simplified for general understanding. If you need to implement an API please go read [Use your own quiz API](UseOwnAPI.md) ** 



### Abstraction

The project uses the abstraction `Quizz`, `Question` and `Answer`. The list containing these elements have the same name but in plural (`Quizzes`, `Questions`, `Answers`).

Only the main properties below have been put into the classes

| Class    | Properties             |
| -------- | ---------------------- |
| Quizz    | Id<br />Title          |
| Question | Id<br />Question Title |
| Answer   | Answer title           |

### 

## Schemas

The schema below shows the 5 distincts separation of:

- API (data from internet)
- API Model (Structure of API data)
- AbstractQuizzStructure (The abstract structure of a quiz)
- The mapping between API and Questions/Questions/Answers
- Application logic (uses AbstractQuizzStructure classes everywhere in the logic)

![How it works](docs/HowItWorksSimple.png)



The Workflow (simplified) of the code.

![How it works](docs/SimplifiedWorkflow.png)



The classes used and their order in execution:

![How it works](docs/HowItWorks.png)





