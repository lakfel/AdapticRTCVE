using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralBannerMannaher : MonoBehaviour
{

    public TextMesh banner;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(textConfiguration());
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
        banner.text = message;
        banner.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        banner.gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator textConfiguration()
    {
        while(true)
        {
            banner.gameObject.transform.Rotate(new Vector3(0f, 5f, 0f));
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
