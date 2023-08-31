using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CakeController : MonoBehaviour {

    public float speed;

    private int partNum;
    void Start() {
        String num = Regex.Match(gameObject.name, @"\d+").Value;
        partNum = Int32.Parse(num);
        //todo fix 0 index
        print(partNum);
        transform.position = transform.position + Vector3.left * partNum * 10;
        
    }

    void Update() {
        if (Input.GetKey(KeyCode.J)) {
            transform.Rotate(100 * Time.deltaTime * speed * Vector3.left);
            transform.localScale += new Vector3(speed, speed, speed) * 0.0001f;
        }
        if (Input.GetKey(KeyCode.K)) {
            transform.Rotate(-100 * Time.deltaTime * speed * Vector3.left);
            transform.localScale -= new Vector3(speed, speed, speed) * 0.0001f;

        }
    }
}