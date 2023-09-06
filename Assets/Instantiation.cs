using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//prefabs, instantiation, hierarchy, local and global spaces, object scripts
public class Instantiation : MonoBehaviour
{
    //prefab explanation: concept, creation, modification
    //these prefabs have been dragged and dropped from the asset panel to the inspector
    public GameObject planePrefab;
    public GameObject cubePrefab;

    public float amplitude = 1.5f;
    public float frequency = 1f;

    private GameObject c1;
    private GameObject c2;
    private GameObject empty;

    // Start is called before the first frame update
    void Start()
    {

        //create a clone of the plane with position and rotation, without saving a reference to it
        Instantiate(planePrefab, new Vector3(-10, 5, 0), Quaternion.Euler(0, 90, 0));

        //create a clone of the plane
        GameObject plane = Instantiate(planePrefab);
        //set position
        plane.transform.position = new Vector3(0, -5, 0);
        //set name
        plane.name = "Ground Plane";

        //create an empty game object
        empty = new GameObject("Empty");

        //instantiate a cube prefab inside the empty object
        c1 = Instantiate(cubePrefab, empty.transform);
        
        //another way to do the same
        c2 = Instantiate(cubePrefab);
        c2.transform.SetParent(empty.transform);

        //by default they are in position 0,0 and scale 1
        c1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        c2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        //apply a rotation to the parent: delta xyz
        empty.transform.Rotate(0,0, Time.deltaTime * 20);

        //sin oscillated from -1 to 1 so I shift it to 0-1
        float x = (Mathf.Sin(Time.time*frequency)+1) * amplitude;

        //change the local positions
        c1.transform.localPosition = new Vector3(2+x, 0, 0);
        c2.transform.localPosition = new Vector3(-2-x, 0, 0);

    }
}
