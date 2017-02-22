using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cameraChange : MonoBehaviour {

    public Camera cam1, cam2;
    
    public Slider slider_1, slider_2, slider_3, slider_4, slider_5, slider_6;
    public Slider cslider_1, cslider_2, cslider_3, cslider_4, cslider_5, cslider_6;

    void Start()
    {
        //slider_1 = GameObject.FindGameObjectWithTag("BaseRotate");
        cam1.enabled = true;
        cam2.enabled = false;
        cslider_1.gameObject.SetActive(cam2.enabled);
        cslider_2.gameObject.SetActive(cam2.enabled);
        cslider_3.gameObject.SetActive(cam2.enabled);
        cslider_4.gameObject.SetActive(cam2.enabled);
        cslider_5.gameObject.SetActive(cam2.enabled);
        cslider_6.gameObject.SetActive(cam2.enabled);

    }

    public void changeCameras()
    {
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
        slider_1.gameObject.SetActive(cam1.enabled);
        slider_2.gameObject.SetActive(cam1.enabled);
        slider_3.gameObject.SetActive(cam1.enabled);
        slider_4.gameObject.SetActive(cam1.enabled);
        slider_5.gameObject.SetActive(cam1.enabled);
        slider_6.gameObject.SetActive(cam1.enabled);
        cslider_1.gameObject.SetActive(cam2.enabled);
        cslider_2.gameObject.SetActive(cam2.enabled);
        cslider_3.gameObject.SetActive(cam2.enabled);
        cslider_4.gameObject.SetActive(cam2.enabled);
        cslider_5.gameObject.SetActive(cam2.enabled);
        cslider_6.gameObject.SetActive(cam2.enabled);
    }
}
