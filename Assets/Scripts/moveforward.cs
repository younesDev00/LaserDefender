using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveforward : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 pos = transform.position;
        // Vector3 velocity = new Vector3(0, 20 * Time.deltaTime,0);
        // pos += transform.rotation * velocity;
        // transform.position = pos;
        transform.Translate(new Vector3(0,1,0));
    }
}
