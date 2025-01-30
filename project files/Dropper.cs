using UnityEngine;
using System.Collections;
using System.Threading;
public class Dropper : MonoBehaviour
{
    public GameObject circle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Drop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Drop()
    {
        float rX = Random.Range(-1f, 1f);
        Vector3 loc = new Vector3(rX, 4, 0);
        Instantiate(circle, loc, transform.rotation);
        //wait
        float next = Random.Range(0.25f, 1.5f);
        yield return new WaitForSeconds(next);
        //go again
        StartCoroutine(Drop());
    }
}