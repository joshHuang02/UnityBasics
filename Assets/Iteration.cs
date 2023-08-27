using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iteration : MonoBehaviour
{
    public GameObject prefabA;
    public GameObject prefabB;
    public GameObject prefabC;
    public GameObject prefabD;

    public List<GameObject> objectList;
    public GameObject[,] objectArray;

    //this is the list equivalent to a 2D array
    public List<List<GameObject>> listOfLists;

    public List<GameObject> movingTunnel;

    public float distanceA = 2;

    public float frequency = 1;
    public float amplitude = 2;
    public float phase = 10;

    public int ROWS = 40;
    public int COLS = 20;
    //between pyramids
    public float distance = 2;

    //of the green objects
    public float speed = 1;

    public float noiseMultiplier = 0.1f;
    public float scaleMultiplier = 2;

    //rotational symmety example
    public int SIDES = 10;
    public int DEPTH = 20;
    
    public float radius = 5;
    public float ringDistance = 2;

    public float radius2 = 10;
    public float ringDistance2 = 3;
    public float tunnelSpeed = 10;
    public float tunnelRotationSpeed = 10;

    public float counter = 0;

    public Material alternativeMaterial;

    private GameObject containerA;
    private GameObject containerB;
    private GameObject containerC;
    private GameObject containerD;
    private GameObject containerE;


    // Start is called before the first frame update
    void Start()
    {
        //create 3 empty object at 0,0 to make switching on and off easy
        containerA = new GameObject("containerA");
        containerB = new GameObject("containerB");
        containerC = new GameObject("containerC");
        containerD = new GameObject("containerD");
        containerE = new GameObject("containerE");


        //iteration
        for (int i=0; i<100; i++)
        {
            GameObject go = Instantiate(prefabA, containerA.transform);
            //position them in a row with 2 units as distance
            go.transform.localPosition = new Vector3(0, 0, i*distanceA);

            //rotate them based on the position
            //remember rotation is in Quaternions so it has to be converted from Euler angles
            go.transform.localRotation = Quaternion.Euler(0, 0, i * 10);

            //add each object to a list so I can quickly reference it later
            objectList.Add(go);
        }


        //initialize the 2D array
        //array have a fixed lenght
        objectArray = new GameObject[ROWS, COLS];

        //nested iteration
        //2D arrangement of objects
        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                GameObject go = Instantiate(prefabB, containerB.transform);
                //position them in a row with 2 units as distance
                go.transform.localPosition = new Vector3(c * distance, 0, r * distance);

                //add each object to the array
                objectArray[r,c] = go;

            }
        }

        //center the container, moving all the pyramids inside
        containerB.transform.position = new Vector2(-COLS*distance/2, -10);


        //green bubbles
        for (int i = 0; i < 100; i++)
        {
            GameObject go = Instantiate(prefabC, containerC.transform);
            
            //randomize their position and rotation
            go.transform.localPosition = new Vector3(Random.Range(-20f, 20f) , Random.Range(0f, 10f) , Random.Range(-20, 20));
            
            go.transform.localRotation = Random.rotation;

        }

        //the static "tunnel"
        //create an empty list of lists
        listOfLists = new List<List<GameObject>>();

        for (int r = 0; r < DEPTH; r++)
        {
            //create each list (ring)
            List<GameObject> ring = new List<GameObject>();

            //populate the list
            for (int s = 0; s < SIDES; s++)
            {
                GameObject go = Instantiate(prefabD, containerD.transform);
                ring.Add(go);
                

                //the positioning happens in the update
            }

            //add it to the list of lists
            listOfLists.Add(ring);
        }


        //the moving tunnel, similar to the one above but different update logic
        //the objects are initialized at their positions and then moved individually
        //they are stored in a 1d list
        movingTunnel = new List<GameObject>();

        for (int r = 0; r < DEPTH; r++)
        {

            //populate the list
            for (int s = 0; s < SIDES; s++)
            {
                //put it in a different container
                GameObject go = Instantiate(prefabD, containerE.transform);


                //I could randomize the angle like this
                //float angle = Random.Range(0f, 360f);
                //instead I find the angle for each side like above and I add a randomness
                //to have more even distribution
                float angDelta = 360f / SIDES;
                float angle = s * angDelta + Random.Range(-angDelta/2, angDelta/2);


                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius2;
                float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius2;

                float z = r * ringDistance2;

                go.transform.localPosition = new Vector3(x, y, z);

                go.transform.rotation = Quaternion.Euler(0, 0, angle);
                //add a rotation in local space to make them face the center
                go.transform.Rotate(0, 90, 0, Space.Self);

                //i can use the same prefab but change the material
                go.GetComponent<Renderer>().material = alternativeMaterial;

                movingTunnel.Add(go);
            }

            
        }


        


    }

    // Update is called once per frame
    void Update()
    {
        
        //go through the list
        //Count is same as Length but for lists 
        for(int i=0; i< objectList.Count; i++)
        {
            GameObject go = objectList[i];

            //scale according to the time (increasing linearily) and the position
            float oscillation = (Mathf.Sin(Time.time * frequency + i*phase)+1) * amplitude;
            
            //phase is the difference in oscillation between each ring
            //1 so it's always bigger than 1
            float scale = 1 + oscillation;

            go.transform.localScale= new Vector3(scale, scale, 1);
        }


        //iterate through the array
        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                GameObject go = objectArray[r, c];

                //noise sampling moves linearly
                float noiseOffset = Time.time;
                
                //sample the noise relative to the position
                float noise = Mathf.PerlinNoise(r * noiseMultiplier+ noiseOffset, c * noiseMultiplier);
                
                //change the z scale (pointy axis)
                Vector3 scale = go.transform.localScale;
                scale.z = noise * scaleMultiplier;
                go.transform.localScale = scale;
            }
        }

        //you can also iterate through children of an object
        foreach (Transform obj in containerC.transform)
        {
            obj.Translate(obj.forward * speed * Time.deltaTime);

            //wrap around if it exits
            Vector3 pos = obj.position;
            if(pos.x > 20)
                pos.x = -20;

            if (pos.x < -20)
                pos.x = 20;

            if (pos.y > 20)
                pos.y = 0;

            if (pos.y < 0)
                pos.y = 20;

            if (pos.z > 20)
                pos.z = -20;

            if (pos.z < -20)
                pos.z = 20;

            obj.position = pos;
        }


        //the tunnel with quads
        //positions and rotations are recalculated every frame
        counter += Time.deltaTime;

        //every 0.5 seconds reset
        if (counter > 0.5f)
            counter = 0;

        for (int r = 0; r < listOfLists.Count; r++)
        {
            List<GameObject> ring = listOfLists[r];

            for(int s = 0; s<ring.Count; s++)
            {
                GameObject go = ring[s];
                //360/SIDES is the angular dist between object arranged in a circle
                float angle = s * 360f / SIDES; //+ Time.time*r*3; //add this for a spiral effect
                //Mathf.Deg2Rad converts degrees in radians - necessary for trigonometric ops
                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

                float z = r * ringDistance;

                go.transform.localPosition = new Vector3(x, y, z);

                go.transform.rotation = Quaternion.Euler(0, 0, angle);
                //add a rotation in local space to make them face the center
                go.transform.Rotate(0, 90, 0, Space.Self);

                //if it's 0 it has been reset so it's every 0.5 seconds
                if (counter == 0)
                {
                    float scale = Random.Range(0.5f, 1f);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }


        //moving tunnel 
        //the positions are updated incrementally
        foreach(GameObject quad in movingTunnel)
        {
            quad.transform.Translate(0, 0, tunnelSpeed * Time.deltaTime, Space.World);
            
            //if the position goes below 0 (behind the camera) push it at the end of the tunnel 
            if(quad.transform.position.z < -ringDistance2)
            {
                Vector3 pos = quad.transform.position;
                pos.z += ringDistance2 * DEPTH;
                quad.transform.position = pos;
            }
        }

        //I can manipulate the container
        containerE.transform.Rotate(0, 0, Time.deltaTime * tunnelRotationSpeed);


        //since I put everything in separate containers I can switch on and off
        //the various parts

        //Alpha1 is the number 1 on the keyboard
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(containerA.activeSelf)
                containerA.SetActive(false);
            else
                containerA.SetActive(true);

        }

        //short version of the if above
        if (Input.GetKeyDown(KeyCode.Alpha2))
            containerB.SetActive(!containerB.activeSelf);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            containerC.SetActive(!containerC.activeSelf);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            containerD.SetActive(!containerD.activeSelf);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            containerE.SetActive(!containerE.activeSelf);

        //the objects inside the containers are still active and updated even if invisible
    }
}
