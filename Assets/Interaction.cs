using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    //references to game objects

    //cube has been set by drag and drop in the inspector
    public GameObject cube;
    public GameObject isosphere;
    public GameObject isosphere2;
    public GameObject star5;
    public GameObject plane;
    public GameObject mess;

    public float rotationSpeed = 100;

    //these arrays are populated by drag and drop in the inspector
    public GameObject[] colorChanging;
    public Material[] changingMaterials;

    //used for oscillation example
    private float angle = 0;
    
    //used for color change
    private int materialIndex = 0;

    //sphere has been dragged and dropped in the inspector
    //if it has this component it is referenced here
    public SkinnedMeshRenderer sphereRenderer;

    //used in the color plane interpolation
    //must be set in the inspector
    public Color colorA;
    public Color colorB;
    public float colorTime = 0;

    public bool toggle = false;

    public float noiseDivider = 0.1f;

    private Coroutine animationCoroutine;



    // Start is called before the first frame update
    void Start()
    {
        //a common way to reference an object: perform a search by name
        //at the beginning an save it in a variable
        //issues: not very flexible, objects have to be uniquely named
        isosphere = GameObject.Find("iso");

    }

    // Update is called once per frame
    void Update()
    {

        //mouse input and scale
        //GetMouseButton(0) is true when left mouse is clicked
        if (Input.GetMouseButton(0))
        {
            //Find is slow so this is not recommended to do every frame
            //but you can find the object by name every time
            GameObject cylinder = GameObject.Find("cylinder");

            //transform is present in all gameobject and contains location rotation and scale
            cylinder.transform.localScale = new Vector3(0.5f, 0.5f, 3);
        }
        else
        {
            //another way to do the same thing without saving object and component
            GameObject.Find("cylinder").transform.localScale = new Vector3(1,1,1);
        }



        //mouse button, position and random (star teleporting)
        //GetMouseButtonDown is true only the frame of the right click
        if (Input.GetMouseButtonDown(1))
        {   
            //vectors properties x, y, z can't be set directly
            //you you typically change them by saving the Vector3 
            //to a temporary variable and then assign the result
            Vector3 pos = star5.transform.position;

            //add a random quantity to each component of the position
            pos.x += Random.Range(-0.2f, 0.2f);
            pos.y += Random.Range(-0.2f, 0.2f);
            pos.z += Random.Range(-0.2f, 0.2f);
            star5.transform.position = pos;
        }


        //keyboard input and interpolated transforms
        if (Input.GetKey(KeyCode.Z))
        {
            //access the script Interpolations, which I coded (it's not part of unity)
            Interpolations interpol = cube.GetComponent<Interpolations>();

            //call the public method Change
            interpol.Change(1);
        }
        else
        {
            //another way to do the same thing without saving the component
            cube.GetComponent<Interpolations>().Change(0);
        }


        //game controller input and activation (isosphere appearing and disappearing)
        
        //lower button on standard xbox controller
        //default alternate are command key and left mouse button
        //the input mapping is in edit > project settings > Input manager
        //I won't get into the new input system
        if (Input.GetButton("Fire1"))
        {
            //turn the game object on and off
            isosphere.SetActive(false);
        }
        else
        {
            isosphere.SetActive(true);
        }


        //analog input and rotation (gears)

        //get the analog stick horizontal axis: a value between -1 and 1
        float horizontal = Input.GetAxis("Horizontal");

        //get a bunch of game objects by tag and put them in an array
        //the tag is set from the inspector
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("rotating");

        //loop through them and apply a rotation based on the controller
        foreach (GameObject go in taggedObjects)
        {
            //the Rotate function expects a vector so I create one
            //the rotation along the local axis
            Vector3 deltaRotation = new Vector3(0, 0, horizontal);

            //access their transform and add a rotation
            //multiply by Time.deltaTime to make it frame independent
            go.transform.Rotate(deltaRotation * rotationSpeed * Time.deltaTime);
        }


        //coroutines (star freaking out)

        //GetKeyDown is true only the frame the key is pressed
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Coroutine
            //a problem with coroutines, multiple copies can happen at the same time if they aren't stopped
            //if there is a coroutine stop it
            //if (animationCoroutine != null)
            //    StopCoroutine(animationCoroutine);

            //and start a new one
            animationCoroutine = StartCoroutine(HardCodedAnimation());
        }


        //blendshapes (deformed sphere)

        //get the analog stick Vertical axis: a value between -1 and 1
        //alternate input left and right
        float vertical = Input.GetAxis("Vertical");

        if (vertical != 0)
        {

            //the blendshape weight is 0-100 so I convert the axis accordingly
            float blendWeight = Mathf.Abs(vertical) * 100;

            //first value is the blendshape number in order
            sphereRenderer.SetBlendShapeWeight(0, blendWeight);

            //blendshapes can be applied at the same time
            //Alpha1 is the number 1 on the keyboard
            //you can also check different inputs at the same time
            if (Input.GetKey(KeyCode.Alpha1))
                sphereRenderer.SetBlendShapeWeight(1, blendWeight);

            if (Input.GetKey(KeyCode.Alpha2))
                sphereRenderer.SetBlendShapeWeight(2, blendWeight);

        }
        else
        {
            //if no analog input reset
            sphereRenderer.SetBlendShapeWeight(0, 0);
            sphereRenderer.SetBlendShapeWeight(1, 0);
            sphereRenderer.SetBlendShapeWeight(2, 0);
        }

        //composite rotation (pyramids)
        if (Input.GetKey(KeyCode.C))
        {
            //game objects can be parented and transformations compounded
            GameObject composite = GameObject.Find("composite");
            GameObject pyramidA = GameObject.Find("pyramidA");
            GameObject pyramidB = GameObject.Find("pyramidB");

            composite.transform.Rotate(new Vector3(rotationSpeed * Time.deltaTime, 0, 0));

            //Space.Self means rotate in the local space (inheriting the composite rotation)
            pyramidA.transform.Rotate(new Vector3(0, 0, rotationSpeed * 2 * Time.deltaTime), Space.Self);
            pyramidB.transform.Rotate(new Vector3(0, 0, rotationSpeed * 2 * Time.deltaTime), Space.Self);

        }

        //oscillation (bouncing ball)
        if (Input.GetKey(KeyCode.V))
        {
            //increment angle in radians
            //10 is the oscillation speed (frequency)
            angle += 10 * Time.deltaTime;

            float sin = Mathf.Sin(angle);

            //sin is always between -1 to 1 so I use a Map function to remap it 
            //according to my needs
            float scaleXZ = Map(sin, -1, 1, 1f, 0.5f);
            float scaleY = Map(sin, -1, 1, 1f, 2f);

            isosphere2.transform.localScale = new Vector3(scaleXZ, scaleY, scaleXZ);
        }


        //array of object and materials (various objects changing color)
        if (Input.GetKeyDown(KeyCode.B))
        {
            //upon B press go through the objects in the array
            for(int i = 0; i<colorChanging.Length; i++)
            {
                //get the object, get the renderer component
                GameObject obj = colorChanging[i];
                Renderer ren = obj.GetComponent<Renderer>();

                //make sure the renderer is there because some objects may not have it
                //eg the blendshape sphere has a skinnedMeshRendere
                if(ren != null)
                {
                    //material or materials (if multiple) are a property of the renderer
                    ren.material = changingMaterials[materialIndex];

                }

            }

            //increment index so it will cycle though at the next click
            materialIndex++;
            //make sure the index doesn't point to a location outside of the array
            if (materialIndex >= changingMaterials.Length)
                materialIndex = 0;
        }

        //color interpolation (plane)
        //you can interpolate between to colors quite easily

        if(Input.GetKey(KeyCode.N))
        {
            colorTime += Time.deltaTime;

            // Time.time is the time elapsed since the beginning
            // modulo % prevents time from going beyond 1 (remainder of division by 1)
            float t = colorTime % 1;
            print("colorTime: " + colorTime + " t:" + t);

            //access renderer on plane
            Renderer ren = plane.GetComponent<Renderer>();

            //linearly interpolate between two given colors and assign it to the main albedo
            //of the material
            ren.material.color = Color.Lerp(colorA, colorB, t);
        }

        //use a variable to save an on/off state 
        if(Input.GetKeyDown(KeyCode.M))
        {
            toggle = !toggle; //negate the current value of the toggle
        }

        //when true rotate the messy ball
        if(toggle)
        {
            //sample 3 arbitrary perlin noise points to rotate the thing
            float n1 = Mathf.PerlinNoise(Time.time* noiseDivider, 1000+Time.time* noiseDivider);
            float n2 = Mathf.PerlinNoise(Time.time* noiseDivider, 100 + Time.time * noiseDivider);
            float n3 = Mathf.PerlinNoise(-1000 + Time.time * noiseDivider, 100 + Time.time * noiseDivider);

            mess.transform.Rotate(n1-0.5f, n2 - 0.5f, n3 - 0.5f);
        }

    }

    //a coroutine has to return a IEnumerator and needs a return
    //the execution can be paused at any point without affecting the rest of the program
    IEnumerator HardCodedAnimation()
    {
        GameObject star = GameObject.Find("star");
        Vector3 initialScale = star.transform.localScale;

        //do this 10 times
        for (int i = 0; i < 10; i++)
        {
            //randomize the scale across the 3 dimensions
            float scaleX = Random.Range(0.2f, 1f);
            float scaleY = Random.Range(0.2f, 1f);
            float scaleZ = Random.Range(0.2f, 1f);

            //set the scale
            star.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

            //wait for 0.2 seconds 
            //this can only happen in a coroutine
            yield return new WaitForSeconds(0.2f);
        }

        //set it back to whatever it was initially
        star.transform.localScale = initialScale;
    }

    //similar to p5 map function remap OldValue from a range between OldMin and OldMax
    //to a range NewMin NewMax
    public static float Map(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax, bool clamp = false)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        if (clamp)
            NewValue = Mathf.Clamp(NewValue, NewMin, NewMax);

        return (NewValue);
    }
}
