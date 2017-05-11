using UnityEngine;
using System.Collections;

public class SpaceshipController : MonoBehaviour
{
    private Vector3 acc, vel;
    private Vector3 angVel, angAcc;
    private float mass = 10f;
    private float range = 1f;

    public void Init()
    {
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            TurnLeft();
        else if (Input.GetKey(KeyCode.D))
            TurnRight();
        else
            TurnStop();

        if (Input.GetKey(KeyCode.UpArrow))
            AccForward();
        else if (Input.GetKey(KeyCode.DownArrow))
            AccBack();
        else if (Input.GetKey(KeyCode.LeftArrow))
            AccLeft();
        else if (Input.GetKey(KeyCode.RightArrow))
            AccRight();

        //회전. //
        angVel = angVel + angAcc * Time.fixedDeltaTime;
        float sAng = angVel.magnitude;
        if (Vector3.Cross(angVel, Vector3.forward).y > 0f)
            sAng = -sAng;

        transform.Rotate(Vector3.up, sAng * Time.fixedDeltaTime * Mathf.Rad2Deg);
    }

    private void TurnLeft()
    {
        Vector3 torque = Vector3.left;
        angAcc = 2f / (mass * range * range) * torque;
    }

    private void TurnRight()
    {
        Vector3 torque = Vector3.right;
        angAcc = 2f / (mass * range * range) * torque;
    }

    private void TurnStop()
    {
        Vector3 torque = -angVel * mass * range * range / (2f * Time.fixedDeltaTime);
        float limitTorque = 0.1f;
        if (torque.magnitude > limitTorque)
            torque = torque.normalized * limitTorque;

        angAcc = 2f / (mass * range * range) * torque;
    }

    private void AccForward()
    {
    }

    private void AccBack()
    {
    }

    private void AccLeft()
    {
    }

    private void AccRight()
    {
    }
}