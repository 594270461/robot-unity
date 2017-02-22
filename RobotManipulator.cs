using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RobotManipulator : MonoBehaviour {

    public float speed = 0f;
    private bool isClicked = false;
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public GameObject basePivot; // Baza
    public GameObject arm1,arm2,grip;
    public GameObject endPoint, destination;
    float [] tmp = { 0f, 0f, 0f };
    float l1, l2,l3;
    float timeSpeed, baseSpeed;
    double alfa, beta, alfa1;
    float k1, k2;
    float lx,ly,lz,l_fin;
    float new_l3, new_lx, new_ly, new_lz, new_l_fin, new_alfa,new_alfa1,new_beta;
    

    void Start(){
        tmp[0] = slider1.value;
        tmp[1] = slider2.value;
        tmp[2] = slider3.value;      
        alfa = 90F;
        beta = 90F;
        speed = 1F;
        ButtonToggle();

        // Obliczanie długości ramion
        l1 = Vector3.Distance(arm1.transform.position, arm2.transform.position); // pierwsze ramie
        l2 = Vector3.Distance(arm2.transform.position, endPoint.transform.position); // drugie ramie
    }

	void Update () {
        timeSpeed = speed / 2 * Time.deltaTime *25;
        baseSpeed = timeSpeed;




        if (!isClicked) { // Inverse Kinematics

            l3 = Vector3.Distance(arm1.transform.position, endPoint.transform.position); // odl. między początkiem i końcem

            lx = Math.Abs(arm1.transform.position.x - endPoint.transform.position.x);
            ly = Math.Abs(arm1.transform.position.y - endPoint.transform.position.y);
            lz = Math.Abs(arm1.transform.position.z - endPoint.transform.position.z);

            l_fin = pitagoras_przeciw(lz, lx);

            alfa = tw_Cosin(ly, l_fin, l3, -1);    // kąt między pierwszym ramieniem a płaszczyzną XZ
            alfa1 = tw_Cosin(l2, l3, l1, 1);       // kąt między pierwszym ramieniem i linią łączącą bazę z chwytakiem
            beta = tw_Cosin(l3, l1, l2, 1);        // kąt między pierwdzym i drugim ramieniem   

            // Nowe wartości na podstawie punktu docelowego
            new_l3 = Vector3.Distance(arm1.transform.position, destination.transform.position);
            if (new_l3 > l1 + l2)
                new_l3 = (l1 + l2) * 0.95f;

            new_lx = Math.Abs(destination.transform.position.x - arm1.transform.position.x);
            new_ly = Math.Abs(destination.transform.position.y - arm1.transform.position.y);
            new_lz = Math.Abs(destination.transform.position.z - arm1.transform.position.z);
            new_l_fin = pitagoras_przeciw(new_lz, new_lx);

            new_beta = tw_Cosin(new_l3, l1, l2, 1);
            new_alfa = tw_Cosin(new_ly, new_l_fin, l3, -1);
            new_alfa1 = tw_Cosin(l2, new_l3, l1, 1);

            k1 = cwiartka((float)Math.Atan(lx / lz), endPoint.transform.position.x, endPoint.transform.position.z);
            k2 = cwiartka((float)Math.Atan(new_lx / new_lz), destination.transform.position.x, destination.transform.position.z);
            ObrotBazy(k1, k2);
            obrot_arm1();
            obrot_arm2();
        }
        else // Forward Kinematics
        {
            Obrot(slider1, basePivot, 0);
            Obrot(slider2, arm1, 1);
            Obrot(slider3, arm2, 2);
        }
    }
    // ********************    INVERSE KINEMATICS    ********************
    void ObrotBazy(float alf, float alf2)
    {
        //Debug.Log(alf2);
        if (alf > alf2 - baseSpeed && alf < alf2 + baseSpeed){
            // stój
            basePivot.transform.Rotate(0, 0, 0, Space.Self);
        }
        else if (( alf>180 && (alf2>(alf-180) && alf>alf2)) || (alf<180 && (alf>alf2 || alf2 > alf + 180) )){
            //w prawo
            basePivot.transform.Rotate(0, baseSpeed, 0, Space.Self);  // obrot bazy w prawo
        }
        else{
            //w lewo
            basePivot.transform.Rotate(0, -baseSpeed, 0, Space.Self);//obrot bazy w lewo
        }
    }

    // obtor ramion (Inverse Kinematics)
    public void obrot_arm1()
    {
        if (new_beta < beta + speed / 2 && new_beta > beta - speed / 2)
            arm2.transform.Rotate(0, 0, 0, Space.Self);
        else if (beta < new_beta)
            arm2.transform.Rotate(-speed, 0, 0, Space.Self);
        else if (beta > new_beta)
            arm2.transform.Rotate(speed, 0, 0, Space.Self);
    }

    public void obrot_arm2()
    {
        double a, na;
        a = alfa + alfa1;
        na = new_alfa + new_alfa1;
        if (na< a + speed/2 && na > a - speed/2)
            arm1.transform.Rotate(0, 0, 0, Space.Self);
        else if (a < na)
            arm1.transform.Rotate(-speed, 0, 0, Space.Self);
        else if (a > na)
            arm1.transform.Rotate(speed, 0, 0, Space.Self);
    }

    // ********************    FORWARD KINEMATICS    ********************
    public void Obrot(Slider slider, GameObject arm,int i)
    {
        if (tmp[i]>slider.value-timeSpeed && tmp[i]<slider.value+(timeSpeed))
        {
            arm.transform.Rotate(0, 0, 0, Space.Self);
        }
        else if (slider.value> tmp[i])
        {
            tmp[i] += timeSpeed;
            if(i==0)
                arm.transform.Rotate(0, -baseSpeed, 0, Space.Self);//obrot bazy
            else
                arm.transform.Rotate(-timeSpeed, 0, 0, Space.Self); //obrot ramion
        }
        else if (slider.value < tmp[i])
        {
            tmp[i] = tmp[i]-timeSpeed;
            if(i == 0)
                arm.transform.Rotate(0, baseSpeed, 0, Space.Self);  // obrot bazy
            else
                arm.transform.Rotate( timeSpeed, 0, 0, Space.Self); // obrot ramion
        }
    }

    // ******************************************************************   

    //Obliczenie przeciwprostokątnej
    public float pitagoras_przeciw(double a, double b)
    {
        return (float)Math.Sqrt((a * a) + (b * b));
    }

    // Obliczenie tw cosinusów - wyliczenie kątów trójkąta
    public float tw_Cosin(double c, double a, double b,int stan)
    {
        int d = 1;
        if (ly < 0 || new_ly < 0)
            d = stan;
        return (float)(Mathf.Rad2Deg * d* Math.Acos(((a * a) + (b * b) - (c * c)) / (2 * a * b)));
    }


    float cwiartka(float angle, float x, float z)
    {
        float bx, bz;
        angle = 90-(Mathf.Rad2Deg * angle);
        
        bx = arm1.transform.position.x;
        bz = arm1.transform.position.z;
        if (x > bx && z > bz)
        {          // I ćwiartka
            return angle;
        }
        else if (x < bx && z > bz)
        {    // II ćwiartka
            return 180-angle;
        }
        else if (x < bx && z < bz)
        {    // III ćwiartka
            return 180 + angle;
        }
        else
        {                          // IV ćwiartka
            return 360 - angle;
        }
    }
    // zmiana kinematyki
    public void ButtonToggle()
    {
        isClicked = !isClicked;
        slider1.interactable = isClicked;
        slider2.interactable = isClicked;
        slider3.interactable = isClicked;
    }
}