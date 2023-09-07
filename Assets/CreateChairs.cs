using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class CreateChairs : MonoBehaviour {
    public int rows;
    public int cols;
    public int height;

    // public GameObject prefab1;
    // public GameObject prefab2;
    // public GameObject prefab3;
    
    private int[,] chairs;
    
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(chairPrefab, new Vector3(rows, cols, 0), Quaternion.identity);
        for (var row = 0; row < rows; row++) {
            for (var col = 0; col < cols; col++) {
                for (var h = 0; h < height; h++) {
                    // int selectPrefab = UnityEngine.Random.Range(1, 4);
                    // switch (selectPrefab) {
                    //     case 1:
                    //         Instantiate(prefab1, new Vector3(row, h, col), Quaternion.identity);
                    //         break;
                    //     case 2:
                    //         Instantiate(prefab2, new Vector3(row, h, col), Quaternion.identity);
                    //         break;
                    //     case 3:
                    //         Instantiate(prefab3, new Vector3(row, h, col), Quaternion.identity);
                    //         break;
                    // }
                    UnityEngine.Object[] objects = Resources.LoadAll("EverythingObjects", typeof(GameObject));
                    GameObject obj = (GameObject)objects[UnityEngine.Random.Range(0, objects.Length)];
                    Instantiate(obj, new Vector3(row, h, col), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
