using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

using SKCell;

/// <summary>
/// Common references throughout the game. [Global Only]
/// </summary>
public sealed class CommonReference : MonoSingleton<CommonReference>
{
    public static Camera cam;
    public Transform center;

    public Transform effectContainer;
    public GameObject[] tilePrefabs;
    public GameObject puffEffect, goalPuffEffect;

    public Sprite[] convBoxes;

    public VideoClip cutsceneClip, endClip;

    public void Initialize()
    {
        cam = Camera.main;
        center = GameObject.FindGameObjectWithTag("Center").transform;
    }
}
