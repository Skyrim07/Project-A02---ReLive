using System;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public sealed class TestScript : MonoBehaviour
{
    private int i = 10;
    private float f = 1.0f;
    private string s = "Hello~";

    public GameObject cube;
    private float distance = 5;
    private void Start()
    {

    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    EventDispatcher.AddListener(EventDispatcher.Common, 0, new SJEvent(() => { print("listener"); }));
        //    EventDispatcher.AddListener(EventDispatcher.Combat, 0, new SJEvent(() => { print("listener2"); }));
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    EventDispatcher.Dispatch(EventDispatcher.Common, 0);
        //    EventDispatcher.Dispatch(EventDispatcher.Combat, 0);
        //}
    }
}

