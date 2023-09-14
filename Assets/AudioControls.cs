using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class AudioControls : MonoBehaviour
{
    public GameObject sphere1;
    public GameObject sphere2;
    public GameObject sphere3;

    public float bassLimit;
    public float boopLimit;
    public float snareLimit;

    private float[] spectrum;
    private float[] rawSpectrum;
    private static float[] freqBand;
    private int bandCount;

    public GameObject[] bandObjects;
    public float heightMultiplier = 1;

    public float smoothing = 100;
    public float threshold = 10;

    private bool bass;
    
    // Start is called before the first frame update
    void Start() {
        bandCount = bandObjects.Length;
        rawSpectrum =  new float[(int)Mathf.Pow(2, bandCount + 1)];
        freqBand = new float[bandCount];
    }

    // Update is called once per frame
    void Update()
    {
        //get the spectrum
        AudioListener.GetSpectrumData(rawSpectrum, 0, FFTWindow.Blackman);

        spectrum = rawSpectrum;
        // Debug.Log(spectrum[0]);

        //splits it in bands
        MakeFrequencyBands();

        //use each band level to control a corresponding cube for testing
        for(int i=0; i< freqBand.Length; i++)
        {
            //make sure the array has enough objects (ideally the same as the bands)
            if(i< bandObjects.Length)
            {
                //make sure there is an object in the array
                if(bandObjects[i] != null)
                {
                    float height = freqBand[i] * heightMultiplier;
                    bandObjects[i].transform.localScale = new Vector3(1, 1, height);
                    
                    if (i == 4 && height > bassLimit) {
                        if (!bass) { StartCoroutine(changeBandColor(i, Color.blue));}
                    }

                    if (i == 6 && height > boopLimit) {
                        StartCoroutine(changeBandColor(i, Color.red));
                    }
                    if (i == 7 && height > snareLimit) {
                        StartCoroutine(changeBandColor(i, Color.green));
                    }
                }
            }
        }

        // //or use the band independently to control something else (red sphere)
        // float s = freqBand[0]/2f;
        //
        // //same scale on 3 dimensions
        // sphere1.transform.localScale = new Vector3(s, s, s);
        //
        //
        //
        // //to make it less noisy I can use an easing function (blue sphere)
        // float currentScale = sphere2.transform.localScale.x; //it's the same across the 3 dimension so axis doesn't matter
        // float targetScale = freqBand[0] / 2f;
        //
        // //add to the current scale a fraction of the distance to the target scale
        // //zeno paradox: Achilles and the tortoise. Move halfway to your destination every time
        // float smoothedScale = currentScale + (targetScale - currentScale) / smoothing;
        //
        // sphere2.transform.localScale = new Vector3(smoothedScale, smoothedScale, smoothedScale);
        //
        //
        // //do something only when you reach a certain threshold (green sphere)
        // if(freqBand[0] > threshold)
        // {
        //     sphere3.transform.localScale = new Vector3(10, 10, 10);
        // }
        // else
        // {
        //     sphere3.transform.localScale = new Vector3(1, 1, 1);
        // }


    }


    //splits the spectrum in bands
    //from https://xr.berkeley.edu/decal/homework/hw2/
    void MakeFrequencyBands()
    {
        int count = 0;

        // Iterate through the bins.
        for (int i = 0; i < bandCount; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);

            // Adding the remaining two samples into the last bin.
            if (i == 7)
            {
                sampleCount += 2;
            }

            // Go through the number of samples for each bin, add the data to the average
            for (int j = 0; j < sampleCount; j++)
            {
                average += spectrum[count];
                count++;
            }

            // Divide to create the average, and scale it appropriately.
            average /= count;
            freqBand[i] = (i + 1) * 100 * average;
        }
    }


    public static float Map(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax, bool clamp = false)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        if (clamp)
            NewValue = Mathf.Clamp(NewValue, NewMin, NewMax);

        return (NewValue);
    }

    private IEnumerator changeBandColor(int i, Color color) {
        bass = true;
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = color;
        yield return new WaitForSeconds(0.2f);
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.white;
        bass = false;
        yield return null;
    }

}
