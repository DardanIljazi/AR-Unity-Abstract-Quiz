# Abstract Quiz AWA

A quiz made for AWA school project in **Unity** and with **AR Foundation** (`Augmented Reality` for new generation phones, works on computer without AR).

The quiz API can be changed easily and has no incidence on the application logic code. For that, you will have to implement your API data model structure or use the default one (already implemented) to begin fastly.

## Requirements

- [Unity](https://unity3d.com/fr/get-unity/download)
- Git

## Fast start

### Clone repository

Clone this repository

```bash
git clone https://github.com/Dardanboy/Abstract_Quiz_AWA.git
cd Abstract_Quiz_AWA
```

Because the repo use `Git Large File Storage (LFS)`, you need to fetch the large files with:

```bash
git lfs fetch --all
```

### Add/Import project in Unity

From `Unity Hub`, click on `Add` and find the `Abstract_Quiz_AWA` folder you just cloned and wait until all elements are imported.

### Begin to work with the project

From the `Project WIndow` [Learning The Interface (Unity3d)](https://docs.unity3d.com/Manual/LearningtheInterface.html), select the folder `Scenes` and double-click on `QuizzScene`. You should see the scene below.

![QuizzScene](docs\images\01QuizzScene.jpg)



### Launch the project

The project is intended to be deployed on phones supporting `Augmented Reality`, but it can work on computer:

From the `Toolbar` of Unity Editor [Learning The Interface (Unity3d)](https://docs.unity3d.com/Manual/LearningtheInterface.html), click on the `Play` button. 

The project is by default configured to work with the api called `herokuapi`.

You should see the picture below asking to scan a QR code but if you are on computer, just click `No, go to quiz list please`

![LaunchedProject](docs\images\02LaunchedProject.jpg)

After that you can see the list of quizes. Select one and respond to it until the end.

![SelectQuizz](docs\images\03SelectQuizz.jpg)



### Going further

You made the project work and you can now go further to implement your own API or to understand how the code works globally:

- [Implement my own API]()
- [Understand how the project works]()



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

