using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset ; // Good Default: (-1.829258f, 7.77f, -52.81f);

    void Start()
    {
        this.offset = new Vector3(-1.829258f, 7.77f, -52.81f); //transform.position - this.player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = this.player.transform.position + offset;
    }
}
