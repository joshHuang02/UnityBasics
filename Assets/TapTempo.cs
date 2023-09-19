using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTempo : MonoBehaviour
{
    private float first = 0;
    private float previous = 0;
    private int count = 0;

    //how many seconds without taps before it stops tracking (bpm doesn't change
    public float RESET_TIME = 2;
    public float PRESET_BPM = 139;

    //key to tap
    public KeyCode tapKey = KeyCode.Space;

    public float BPM = 0;
    public float average = 0; //time between beats in seconds

    //the corouting
    private Coroutine beatCoroutine;

    public GameObject beatObject;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(tapKey))
        {
            Tap();
        }


        //reduce the scale of my object to 1 with a smoothing function
        float currentScale = beatObject.transform.localScale.x;
        float targetScale = 1;
        float smoothedScale = currentScale + (targetScale - currentScale) * (Time.deltaTime * 10); //10 is the smoothing factor

        beatObject.transform.localScale = new Vector3(smoothedScale, smoothedScale, smoothedScale);

        //bonus function: if you already know the BPMs you can just use a number
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetBPM(PRESET_BPM);
        }

    }


    public void Tap()
    {
        //convert time to millisecond
        float time = Time.time;
        
        //it RESET_TIME seconds elapsed stop counting
        if ((time - previous) > RESET_TIME)
        {
            ResetCount();
        }

        if (count == 0)
        {
            print("First tap");
            first = time;
            count = 1;
        }
        else
        {
            BPM = Mathf.Round(60 * count / (time - first));
            average = 1 / (count / (time - first));

            print("BPM "+ BPM);

            count++;

            //restart the current coroutine to syncronize it
            if (beatCoroutine != null)
                StopCoroutine(beatCoroutine);
            
            beatCoroutine = StartCoroutine(Beat());
        }

        previous = time;
    }

    //starts the coroutine with a given bpm value
    void SetBPM(float b)
    {
        BPM = b;
        average = 60 / BPM;

        //restart the current coroutine to syncronize it
        if (beatCoroutine != null)
            StopCoroutine(beatCoroutine);

        beatCoroutine = StartCoroutine(Beat());
    }

    //this plays forever as long as the object is active
    IEnumerator Beat()
    {
        while (gameObject.activeSelf)
        {
            //print("BEAT");
            
            //do something each beat
            beatObject.transform.localScale = new Vector3(2, 2, 2);

            yield return new WaitForSeconds(average);
        }
    }

    public void ResetCount()
    {
        count = 0; //taps
    }

}
