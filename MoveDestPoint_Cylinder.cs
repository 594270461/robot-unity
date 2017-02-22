using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveDestPoint_Cylinder : MonoBehaviour {

    public Slider x_slider, y_slider, z_slider;
    public GameObject destination,endPoint;
    public GameObject ball;
    private bool isClicked = false;
    Vector3 v;
    Vector3 ba;
    float [] dx = { 0F, 0F, 0F };
    float T;
    void Start() {
        v = endPoint.transform.position;
        destination.transform.position = v;
        ball.transform.position = v;
        x_slider.value = v.x;
        y_slider.value = v.y;
        z_slider.value = v.z;
        x_slider.interactable = isClicked;
        y_slider.interactable = isClicked;
        z_slider.interactable = isClicked;       
    }

    void Update()
    {
        if (isClicked)
        {
            T = Time.deltaTime / 2;
            ba.x = x_slider.value;
            ba.y = y_slider.value;
            ba.z = z_slider.value;
            dzielnik_przesuniecia(v, ba);
            przesun(v, ba);

            destination.transform.position = v;
            ball.transform.position = ba;
        }else
        {
            v = endPoint.transform.position;
            ball.transform.position = v;
            ba = v;
            destination.transform.position = v;
            x_slider.value = v.x;
            y_slider.value = v.y;
            z_slider.value = v.z;
        }
    }

    void dzielnik_przesuniecia(Vector3 v1, Vector3 v2)
    {
        float x = Mathf.Abs(v1.x - v2.x);
        float y = Mathf.Abs(v1.y - v2.y);
        float z = Mathf.Abs(v1.z - v2.z);

        dx[0] = Mathf.Max(x, y, z) / T;
        dx[1] = y / dx[0];
        dx[2] = z / dx[0];
        dx[0] = x / dx[0];
    }

    void przesun(Vector3 v1, Vector3 v2)
    {
        if (v1.x > v2.x - dx[0] && v1.x < v2.x + dx[0]) { }
        else if (v1.x - v2.x > 0)
            v1.x -= dx[0];
        else if (v1.x - v2.x < 0)
            v1.x += dx[0];

        if (v1.y > v2.y - dx[1] && v1.y < v2.y + dx[1]) { }
        else if (v1.y - v2.y > 0)
            v1.y -= dx[1];
        else if (v1.y - v2.y < 0)
            v1.y += dx[1];

        if (v1.z > v2.z - dx[2] && v1.z < v2.z + dx[2]) { }
        else if (v1.z - v2.z > 0)
            v1.z -= dx[2] * 2;
        else if (v1.z - v2.z < 0)
            v1.z += dx[2] * 2;
        v = v1;
    }

    public void ButtonToggle()
    {
        isClicked = !isClicked;
        x_slider.interactable = isClicked;
        y_slider.interactable = isClicked;
        z_slider.interactable = isClicked;
    }
}
