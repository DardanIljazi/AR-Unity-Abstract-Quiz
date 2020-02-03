using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    private List<Color32> listOfColor = new List<Color32>();
    public GameObject objectToRandomizeColorWhenEnable;
    System.Random rnd;

    // Start is called before the first frame update
    void Awake()
    {
        rnd = new System.Random();
        listOfColor.Add(new Color32(26, 188, 156, 255));
        listOfColor.Add(new Color32(22, 160, 133, 255));
        listOfColor.Add(new Color32(46, 204, 113, 255));
        listOfColor.Add(new Color32(39, 174, 96, 255));
        listOfColor.Add(new Color32(52, 152, 219, 255));
        listOfColor.Add(new Color32(41, 128, 185, 255));
        listOfColor.Add(new Color32(142, 68, 173, 255));
        listOfColor.Add(new Color32(52, 73, 94, 255));
        listOfColor.Add(new Color32(44, 62, 80, 255));
        listOfColor.Add(new Color32(243, 156, 18, 255));
        listOfColor.Add(new Color32(211, 84, 0, 255));
        listOfColor.Add(new Color32(231, 76, 60, 255));
        listOfColor.Add(new Color32(192, 57, 43, 255));
        listOfColor.Add(new Color32(243, 156, 18, 255));
    }

    void OnEnable()
    {
        this.objectToRandomizeColorWhenEnable.GetComponent<Renderer>().material.color = listOfColor[rnd.Next(0, listOfColor.Capacity-1)]; 
    }
}