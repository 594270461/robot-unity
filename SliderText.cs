using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderText : MonoBehaviour {

    public Slider s1;
    public Text t;


    void Update()
    {
        t.text = s1.value.ToString("F2");
    }
}
