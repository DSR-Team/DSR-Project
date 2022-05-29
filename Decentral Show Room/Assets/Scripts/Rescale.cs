using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescale : MonoBehaviour
{
    public float maxSize = 2;
    public float minSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        SizeNormalize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SizeNormalize(){
        Vector3 modelSize = Vector3.zero;

        for (int i = 0; i < transform.childCount; i++){
            try{
                modelSize = transform.GetChild(i).GetComponent<Renderer>().bounds.size;
                break;
            }
            catch{
                continue;
            }
        }

        for (int i = 0; i <transform.childCount; i++){
            try{
                if(modelSize.y < transform.GetChild(i).GetComponent<Renderer>().bounds.size.y){
                    modelSize = transform.GetChild(i).GetComponent<Renderer>().bounds.size;
                }
            }
            catch{
                continue;
            }
        }

        //Debug.Log("Original Size = " + modelSize);
        
        if(modelSize.y > maxSize){
            transform.localScale = new Vector3(
                transform.localScale.x / (modelSize.y / maxSize),
                transform.localScale.y / (modelSize.y / maxSize),
                transform.localScale.z / (modelSize.y / maxSize)
            );
        }
        else if(modelSize.y < minSize){
            transform.localScale = new Vector3(
                transform.localScale.x * (modelSize.y / minSize),
                transform.localScale.y * (modelSize.y / minSize),
                transform.localScale.z * (modelSize.y / minSize)
            );
        }
    }
}
