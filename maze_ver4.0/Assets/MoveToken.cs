using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToken : MonoBehaviour {

    public float rotationalSpeed = 60f; //degrees per second

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationalSpeed * Time.deltaTime, 0, Space.World);
    }
}
