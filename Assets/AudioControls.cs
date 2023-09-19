using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioControls : MonoBehaviour {
    private GameObject[] slotWheels;
    public GameObject slotWheel1;
    public GameObject slotWheel2;
    public GameObject slotWheel3;
    public GameObject roulette;
    public GameObject cardSpawner;
    public GameObject cardSpitter;

    public float bassLimit;
    public float dootLimit;
    public float boopLimit;
    public float snareLimit;

    private float[] spectrum;
    private float[] rawSpectrum;
    private static float[] freqBand;
    private int bandCount;

    public GameObject[] bandObjects;
    public float heightMultiplier = 1;

    private bool bass;
    private bool doot;
    private bool boop;
    private bool snare;
    
    // Start is called before the first frame update
    void Start() {
        bandCount = bandObjects.Length;
        rawSpectrum =  new float[(int)Mathf.Pow(2, bandCount + 1)];
        freqBand = new float[bandCount];

        slotWheels = new[] { slotWheel1, slotWheel2, slotWheel3 };
    }

    // Update is called once per frame
    void Update() {
        
        //get the spectrum
        AudioListener.GetSpectrumData(rawSpectrum, 0, FFTWindow.Blackman);

        spectrum = rawSpectrum;

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
                        if (!bass) StartCoroutine(TriggerBass(i));
                    }
                    
                    if (i == 2 && height > dootLimit) {
                        if (!doot && !bass) StartCoroutine(TriggerDoot(i));
                    }
                    
                    if (i == 7 && height > snareLimit) {
                        // if (!snare && !boop) StartCoroutine(triggerSnare(i));
                        if (!snare) StartCoroutine(TriggerSnare(i));
                    }

                    if (i == 6 && height > boopLimit) {
                        if (!boop && !snare) StartCoroutine(TriggerBoop(i));
                    }
                    
                }
            }
        }
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

    private IEnumerator TriggerBass(int i) {
        bass = true;
        
        GameObject targetSlotWheel = slotWheels[Random.Range(0, slotWheels.Length)];
        targetSlotWheel.GetComponent<SlotWheelController>().rotate();
        
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.blue;
        yield return new WaitForSeconds(0.2f);
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.white;
        bass = false;
        yield return null;
    }
    
    private IEnumerator TriggerDoot(int i) {
        doot = true;
        
        cardSpitter.GetComponent<CardSpitter>().SpitCard();
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.black;
        yield return new WaitForSeconds(0.085f);
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.white;
        doot = false;
        yield return null;
    }
    
    private IEnumerator TriggerBoop(int i) {
        boop = true;
        
        roulette.GetComponent<RouletteController>().addTorque();
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.white;
        boop = false;
        yield return null;
    }
    
    private IEnumerator TriggerSnare(int i) {
        snare = true;
        for (int c = 1; c < Random.Range(2, 4); c++) {
            StartCoroutine(cardSpawner.GetComponent<CardController>().FlipCard());
        }

        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        bandObjects[i].gameObject.GetComponent<Renderer>().material.color = Color.white;
        snare = false;
        yield return null;
    }
}
