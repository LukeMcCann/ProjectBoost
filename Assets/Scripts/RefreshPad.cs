using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshPad : MonoBehaviour
{
    [SerializeField] Rocket rocket;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" )
        {
            rocket.SetFuel(100f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
