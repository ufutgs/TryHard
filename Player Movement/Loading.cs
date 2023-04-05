using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public GameObject destory;
    public Transform door;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy_and_open());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator Destroy_and_open() 
    {
       
        destory.SetActive(false);
        yield return new WaitForSeconds(2);
        destory.SetActive(true);
        destory.transform.transform.position += Vector3.back * 5;
        yield return new WaitForSeconds(2); // confusing C# method
        door.transform.transform.position += Vector3.up * 10;
    } 
}
