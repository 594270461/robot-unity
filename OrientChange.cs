using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OrientChange : MonoBehaviour {

    public Text t;
    private bool b=false;

    public void ChangeText()
    {
        if (b)
        {
            t.text = "Local";
            b = !b;
        }
        else
        {
            t.text = "Global";
            b = !b;
        }
    }
}
