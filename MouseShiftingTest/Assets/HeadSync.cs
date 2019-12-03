using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSync : MonoBehaviour
{
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<PhotonView>().isMine)
        {
            camera.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            transform.position = camera.transform.position;
            transform.rotation = camera.transform.rotation;
        }
    }
}
