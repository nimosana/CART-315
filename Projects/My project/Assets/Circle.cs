using UnityEngine;

public class Circle : MonoBehaviour

{
    public float xLoc, yLoc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
    }
}
