using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class cylinderRobot : MonoBehaviour {

    public GameObject basePivot, elevator, pipe,endpoint,destination;
    public Slider slider_1, slider_2, slider_3;
    private bool isClicked = true;
    private float[] tmp = { 0f, 0f, 0f };
    private float timeSpeed, baseSpeed, elevSpeed,pipeSpeed;
    private float dx, dy, dz,new_dx,new_dy,new_dz;
    private float alfa, beta, gamma,new_alfa,new_beta_new_gamma;
	// Use this for initialization
	void Start () {
        tmp[0] = slider_1.value;
        tmp[1] = slider_2.value;
        tmp[2] = slider_3.value;
        slider_1.interactable = isClicked;
        slider_2.interactable = isClicked;
        slider_3.interactable = isClicked;
    }
	
	void Update () {
        //prędkości poszczególnych silników
        timeSpeed = Time.deltaTime*4;
        baseSpeed = timeSpeed * 6;
        elevSpeed = timeSpeed / 4;

        if (isClicked) { 
            ObrotBazy();
            TransElev();
            TransPipe();
        }
        else
        {
                        // odległość między bazą a końcem (3), i punktem docelowym (3)
            dx = Math.Abs(basePivot.transform.position.x - endpoint.transform.position.x); //na osi x
            dy = Math.Abs(basePivot.transform.position.y - endpoint.transform.position.y); // na osi y
            dz = Math.Abs(basePivot.transform.position.z - endpoint.transform.position.z);  // na osi z
            new_dx = Math.Abs(basePivot.transform.position.x - destination.transform.position.x);// x
            new_dy = Math.Abs(basePivot.transform.position.y - destination.transform.position.y);// y
            new_dz = Math.Abs(basePivot.transform.position.z - destination.transform.position.z);// z

            alfa = cwiartka((float)Math.Atan(dx / dz),endpoint.transform.position.x,endpoint.transform.position.z);
            new_alfa = cwiartka((float)Math.Atan(new_dx/new_dz),destination.transform.position.x,destination.transform.position.z);

            ObrotBazy(alfa, new_alfa);
            Winda();
            Rura();
        }
    }

    // ********************    INVERSE KINEMATICS    ********************
    void ObrotBazy(float alf, float alf2)
    {
        if (alf >alf2 - baseSpeed && alf<alf2+baseSpeed){
            // stój
            basePivot.transform.Rotate(0, 0, 0, Space.Self);
        }
        else if ((alf < 180 && (alf2 > alf + 180 || alf>alf2)) || (alf > 180 && (alf2 < alf && alf2>alf-180)) ){
            //w prawo
            basePivot.transform.Rotate(0, 0, baseSpeed, Space.Self);  // obrot bazy w prawo
        }
        else{
            //w lewo
            basePivot.transform.Rotate(0, 0, -baseSpeed, Space.Self);//obrot bazy w lewo
        }
    }

    void Winda()
    {                           // zakres osi Y
            if (dy > new_dy - elevSpeed && dy < new_dy + elevSpeed)
                elevator.transform.Translate(0, 0, 0, Space.Self);      //stój 11.36  13.15
            else if (dy > new_dy)
                elevator.transform.Translate(0, 0, -elevSpeed, Space.Self); // w dół
            else
                elevator.transform.Translate(0, 0, elevSpeed, Space.Self);  // w górę
    }

    void Rura()
    {
        float l = pitagoras(dx, dz);
        float nl = pitagoras(new_dx, new_dz);
        if(nl>1.4f && nl < 2.72f) { 
            if (l>nl-pipeSpeed && l < nl + pipeSpeed)
                pipe.transform.Translate(0, 0, 0, Space.Self);
            else if (l>nl)
                pipe.transform.Translate(0, 0, -pipeSpeed, Space.Self); // w tył
            else
                pipe.transform.Translate(0, 0, pipeSpeed, Space.Self);  // do przodu
        }
    }
    // ********************    FORWARD KINEMATICS    ********************
    private void ObrotBazy()
    {
        if (tmp[0] > slider_1.value - baseSpeed && tmp[0] < slider_1.value + (baseSpeed))
        {
            basePivot.transform.Rotate(0, 0, 0, Space.Self);
        }
        else if (slider_1.value > tmp[0])
        {
            tmp[0] += baseSpeed;
            basePivot.transform.Rotate(0, 0, -baseSpeed * 2, Space.Self);//obrot bazy w lewo
        }
        else if (slider_1.value < tmp[0])
        {
            tmp[0] = tmp[0] - baseSpeed;
            basePivot.transform.Rotate(0, 0, baseSpeed * 2, Space.Self);  // obrot bazy w prawo

        }
    }

    private void TransElev()
    {
        if (tmp[1] > slider_2.value - elevSpeed && tmp[1] < slider_2.value + (elevSpeed)){
            elevator.transform.Translate(0, 0, 0, Space.Self);
        }
        else if (slider_2.value > tmp[1]){
            tmp[1] += elevSpeed;
            elevator.transform.Translate(0, 0, -elevSpeed, Space.Self); // w dół
        }
        else if (slider_2.value < tmp[1]){
            tmp[1] = tmp[1] - (elevSpeed);
            elevator.transform.Translate(0, 0, elevSpeed, Space.Self);  // w górę
        }
    }

    private void TransPipe()
    {
        pipeSpeed = timeSpeed / 4;
        if (tmp[2] > slider_3.value - pipeSpeed && tmp[2] < slider_3.value + (pipeSpeed)){
            pipe.transform.Translate(0, 0, 0, Space.Self);
        }
        else if (slider_3.value > tmp[2]){
            tmp[2] += pipeSpeed;
            pipe.transform.Translate(0, 0, -pipeSpeed, Space.Self); // w tył
        }
        else if (slider_3.value < tmp[2]){
            tmp[2] = tmp[2] - (pipeSpeed);
            pipe.transform.Translate(0, 0, pipeSpeed, Space.Self);  // do przodu
        }
    }
    // ******************************************************************
    float cwiartka(float angle, float x, float z)
    {
        float bx, bz;
        angle = 90 - (Mathf.Rad2Deg * angle);
        bx = basePivot.transform.position.x;
        bz = basePivot.transform.position.z;
        if (x > bx && z > bz)
        {          // I ćwiartka
            return angle;
        }
        else if (x < bx && z > bz)
        {    // II ćwiartka
            return 180 - angle;
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

    public float pitagoras(float a, float b)
    {
        return (float)Math.Sqrt((a * a) + (b * b));
    }

    public void ButtonToggle()
    {
        isClicked = !isClicked;
        slider_1.interactable = isClicked;
        slider_2.interactable = isClicked;
        slider_3.interactable = isClicked;
    }
}