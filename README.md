# Abstract Quiz AWA

An abstract quiz made for AWA school project in **Unity** and with **AR Foundation**.

## Requirements

Can work with any API (thanks to abstraction). To work, the API data structure should have:

- One or Multiple `Quiz`

- One or Multiple `Question` referenced to a `Quiz`

- One or Multiple `Answer` referenced to a `Question` and to a `Quiz`

- Data returned in json format from HTTP request (be it in *endpoint for each resource*  or in *endpoint with nested resources*

  For example:

  - *Endpoint for each resource*:

    | Data        | Endpoint                                             |
    | ----------- | ---------------------------------------------------- |
    | Quiz(es)    | http://example.com/api/quizzes                       |
    | Question(s) | http://example.com/api/quizzes/1/questions           |
    | Answer(s)   | http://example.com/api/quizzes/1/questions/2/answers |

  

  - *Endpoint with nested resources*

    | Data                               | Endpoint                       |
    | ---------------------------------- | ------------------------------ |
    | Quiz(es) / Question(s) / Answer(s) | http://example.com/api/quizzes |

## Implementation

All you have to do is:

- Define your Models structure (based on what backend returns in json) and map data. Inherited by `AbstractQuizzStructure`
- Define your Api manager (defines configuration for the API and manages serialization based on quiz models). Inherited by `ApiManager`

```
You can find examples of implementation in Assets/Quizz/Api/01Apis/. 
- Heroku api can work directly without any other component because it is already online 
- Quizawa api needs the backend (laravel) to be installed and launched according to the repo
```





Documentation is Work In Progress..

