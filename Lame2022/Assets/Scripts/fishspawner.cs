using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishspawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fish;
  [Range(0,10)]

    public float fishspawnerdelay;
    float timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0) 
        {
            timer += fishspawnerdelay;
            Instantiate(fish);
        }

    }
}
