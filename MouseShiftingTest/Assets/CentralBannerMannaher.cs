using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralBannerMannaher : MonoBehaviour
{
    public bool allowed;
    public TextMesh banner;
    // Start is called before the first frame update
    void Start()
    {
        allowed = true;
        permanentMessage("Bienvenido!");
        if (!PhotonNetwork.isMasterClient)
            banner.gameObject.transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void disableBanner()
    {
        banner.gameObject.SetActive(false);
    }

    public void permanentMessage(string message)
    {
        banner.text= message;
        banner.gameObject.SetActive(true);

    }
   public void temporalMessage(string message)
    {
        StartCoroutine(tMessage(message));
    }
    public IEnumerator tMessage(string message)
    {
        allowed = true;
        banner.text = message;
        banner.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        allowed = false;
        banner.gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator textConfiguration()
    {
        while(allowed)
        {
            banner.gameObject.transform.Rotate(new Vector3(0f, 5f, 0f));
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
}
