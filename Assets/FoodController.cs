using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class FoodController : MonoBehaviour {

    public float speed;

    private int partNum;

    void Start() {
        String num = Regex.Match(gameObject.name, @"\d+").Value;
        if (num.Length != 0) {
            partNum = Int32.Parse(num);
            transform.position += Vector3.forward * partNum * 10;
        }
        else {
            partNum = 0;
        }
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
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += 0.0005f * (partNum*partNum) * Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += 0.0005f * (partNum*partNum) * Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += 0.0005f * (partNum*partNum) * Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += 0.0005f * (partNum*partNum) * Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(Vector3.zero, Vector3.forward, 20 * speed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.forward, -20 * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E)) {
            transform.RotateAround(Vector3.zero, Vector3.forward, -20 * speed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.forward, 20 * speed * Time.deltaTime);
        }
    }
}