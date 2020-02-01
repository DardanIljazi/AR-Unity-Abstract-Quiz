# Abstract Quiz AWA

A quiz made for AWA school project in **Unity** and with **AR Foundation** (`Augmented Reality` for new generation phones, works on computer without AR).

The quiz API can be changed easily and has no incidence on the logic code. For that, you will have to implement your API data model structure or use the default one (already implemented) to begin fastly.

## Requirements

- [Unity Hub](https://unity3d.com/get-unity/download)
- Git



## Fast start

### Add Unity Version (2019.1 or later)

To work, the project needs a recent version of Unity (`2019.1 or later`).
To install a version, go [on Unity download page](https://unity3d.com/fr/get-unity/download/archive) and choose the most recent version using the `Unity Hub` button to install it. 

`Unity Hub` software should open. Don't forget to check  `Android Build Support` or `iOS Build Support`  depending on which platform you want the final app to be and click install.



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

From `Unity Hub`, click on `Add` from `Projects` and find the `Abstract_Quiz_AWA` folder you just cloned.

![Add Project](docs/images/00AddProject.jpg)

Click on the newly added project from the list and wait until all elements are imported. 



### Install ARCore/ARKit XR Plugin

In Unity’s top menu: **Window** > **Package Manager**

Find and install:

- `ARCore XR Plugin` for Android (the project was set using this platform)
- `ARKit XR Plugin` for iOS (not tested)

*Info: The code uses AR Foundation that is a wrapper around ARCore and ARKit. If you want to know more about it [go here](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@2.2/manual/index.html)*



### Begin to work with the project

From the `Project WIndow` [Learning The Interface (Unity3d)](https://docs.unity3d.com/Manual/LearningtheInterface.html), select the folder `Scenes` that is in `Assets` and double-click on `QuizzScene`. You should see the scene below.

![QuizzScene](docs/images/01QuizzScene.jpg)



### Launch the project

The project is intended to be deployed on phones supporting `Augmented Reality`, but it can work on computer. To begin we will test it on computer, you will see later how to make it work on phones:

From the `Toolbar` of Unity Editor [Learning The Interface (Unity3d)](https://docs.unity3d.com/Manual/LearningtheInterface.html), click on the `Play` button. 

The project is by default configured to work with the api called `herokuapi`.

You should see the picture below asking to scan a QR code but we are on computer, so just click `No, go to quiz list please`

![LaunchedProject](docs/images/02LaunchedProject.jpg)

After that you can see the list of quizes. Select one and respond to it until the end.

![SelectQuizz](docs/images/03SelectQuizz.jpg)



## Going further

You made the project work and you can now go further to deploy it on your phone, implement your own API or to understand how the project works globally:

- [Deploy on phone](DeployOnPhone.md)
- [Use your own quiz API](UseOwnAPI.md)
- [Understand how the project works]()



This documentation is Work In Progress..

