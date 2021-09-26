using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class ThreadQueue : MonoBehaviour
{
    void Start ()
    {
        Debug.Log("Start");
        toMainThread = new List<Action>();

        StartThreadedFunction(SlowFunction);
        Debug.Log("Done");
    }

    void Update () //always runs in main thread
    {
        while(toMainThread.Count > 0)
        {
            //takes 1st function in list
            Action func = toMainThread[0];
            toMainThread.RemoveAt(0);

            //running
            func();
        }
    }

    List<Action> toMainThread;

    public void StartThreadedFunction(Action f)
    {
        Thread t = new Thread(new ThreadStart( f ) );
        t.Start();
    }

    public void QueueMainThredFunction(Action f)
    {
        // making sure f is running from main thread
        //f(); // not good if in child thread

        toMainThread.Add(f);
    }

    void SlowFunction()
    {
        //slow thing
        Debug.Log("object will move in 5s");
        Thread.Sleep(5000); //sleeps for 5s

        //modifying gameobjcet
        Action function = () =>
        {
            this.transform.position = new Vector3(3, 2, 1); //can't call from child thread
            Debug.Log("Now");
        };

        //can't call from child thread yet
        //function();

        QueueMainThredFunction(function);
    }

}
