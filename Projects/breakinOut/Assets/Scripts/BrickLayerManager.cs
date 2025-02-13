using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BrickLayerManager : MonoBehaviour {
    public GameObject brick;
    

    public int rows, columns;

    [FormerlySerializedAs("brickSpacing_h")] public float brickSpacingH;
    [FormerlySerializedAs("brickSpacing_v")] public float brickSpacingV;
    
    
    // Start is called before the first frame update
    void Start() {
        Debug.Log(this.transform.localPosition.x);
        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                float xPos =  -columns + (i * brickSpacingH);
                float yPos = rows - (j * brickSpacingV);
                Instantiate(brick, new Vector3(xPos, yPos, 0), transform.rotation);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
