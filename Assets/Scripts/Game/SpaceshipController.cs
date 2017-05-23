using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceshipController : MonoBehaviour
{
    //힘. //
    private Vector3 thrust, torque;
    //가속도. //
    private Vector3 acc, angAcc;
    //속도. //
    private Vector3 vel, angVel;
    
    public class Spaceship
    {
        public List<Engine> engines;
        public List<Thrust> thrusts;

        //모든 엔진의 최대 출력 합. //
        public float maxPower = 100f;
        //모든 출력체로 얻어지는 출력량. //
        public float maxForwardThrust = 100f;
        public float maxBackThrust = 20f;
        public float maxLeftTorque = 10f;
        public float maxRightTorque = 10f;
        //모든 파츠의 무게 합. //
        public float mass = 10f;
        //최종 우주선 형태의 크기. //
        public float sizeX = 1f;
        public float sizeZ = 1f;
        //관성 모멘트. //
        public float inertiaMoment = 5f / 3f;   //I = m / 12 * (Mathf.Pow(sizeX, 2f) * Mathf.Pow(sizeZ, 2f));
    }

    //엔진. 에너지를 발생하는 장치. //
    //엔진에서 발생한 에너지는 컨트롤러에 따라 Thrust 와 Torque가 나눠 사용한다. //
    public class Engine : Parts
    {
        public float maxPower;  //최대 출력. //
    }

    //추진체. 엔진에서 에너지를 가져다가 사용. //
    public class Thrust : Parts
    {
        public Vector3 direction;   //추진체의 출력 방향. //
        public float thrustPower;   //추진체의 출력 크기. //
    }

    //부품. 모든 우주선의 부분들은 부품. //
    public class Parts
    {
        public float mass;  //질량. //
    }

    private Spaceship spaceshipData;

    private void Awake()
    {
        Joystick.Instance.SetJoystickMovement(Move);

        spaceshipData = new Spaceship();
    }

    private void Move(Vector2 move)
    {
        if (move.y > 0f)
            thrust = Vector3.forward * spaceshipData.maxForwardThrust * move.y;
        else if (move.y < 0f)
            thrust = Vector3.forward * spaceshipData.maxBackThrust * move.y;
        else
            thrust = Vector3.zero;

        if (move.x > 0f)
            torque = Vector3.right * spaceshipData.maxRightTorque * move.x;
        else if (move.x < 0f)
            torque = Vector3.right * spaceshipData.maxLeftTorque * move.x;
        else
            torque = Vector3.zero;

        acc = thrust / spaceshipData.mass;
        //angAcc = 2f * torque / (spaceshipData.mass * spaceshipData.range * spaceshipData.range);
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
        //angAcc = 2f / (mass * range * range) * torque;
    }

    private void TurnRight()
    {
        Vector3 torque = Vector3.right;
        //angAcc = 2f / (mass * range * range) * torque;
    }

    private void TurnStop()
    {
        //Vector3 torque = -angVel * mass * range * range / (2f * Time.fixedDeltaTime);
        //float limitTorque = 0.1f;
        //if (torque.magnitude > limitTorque)
        //    torque = torque.normalized * limitTorque;

        //angAcc = 2f / (mass * range * range) * torque;
    }

    private void AccForward()
    {
        Vector3 power = transform.forward;
        //acc = power / mass;
    }

    private void AccBack()
    {
        Vector3 power = -transform.forward;
        //acc = power / mass;
    }

    private void AccLeft()
    {
        Vector3 power = -transform.right;
        //acc = power / mass;
    }

    private void AccRight()
    {
        Vector3 power = transform.right;
        //acc = power / mass;
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