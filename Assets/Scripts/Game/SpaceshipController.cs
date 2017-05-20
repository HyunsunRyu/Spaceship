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
        else
            AccStop();

        //회전. //
        angVel = angVel + angAcc * Time.fixedDeltaTime;
        float sAng = angVel.magnitude;
        if (Vector3.Cross(angVel, Vector3.forward).y > 0f)
            sAng = -sAng;

        transform.Rotate(Vector3.up, sAng * Time.fixedDeltaTime * Mathf.Rad2Deg);

        //이동. //
        vel = vel + acc * Time.fixedDeltaTime;
        transform.position = transform.position + vel * Time.fixedDeltaTime + 0.5f * acc * Time.fixedDeltaTime * Time.fixedDeltaTime;
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
        Vector3 power = transform.forward;
        acc = power / mass;
    }

    private void AccBack()
    {
        Vector3 power = -transform.forward;
        acc = power / mass;
    }

    private void AccLeft()
    {
        Vector3 power = -transform.right;
        acc = power / mass;
    }

    private void AccRight()
    {
        Vector3 power = transform.right;
        acc = power / mass;
    }

    private void AccStop()
    {
        acc = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + acc * 100f);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + angAcc * 100f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + vel * 100f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + angVel * 100f);
    }
}