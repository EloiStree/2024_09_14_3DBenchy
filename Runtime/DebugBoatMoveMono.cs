using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoatMoveMono : MonoBehaviour
{

    public Transform m_whatToMove;
    public float m_rotateLeftRightAngle = 0;
    public float m_moveForwardSpeed = 0;
    public float m_moveBackwardSpeed = 0;

    [Range(-1f,1f)]
    public float m_inputRotateLeftRightPercent = 0;
    [Range(-1f, 1f)]
    public float m_inputMoveForwardPercent = 0;

    public void Update()
    {
        float rotate = m_rotateLeftRightAngle * m_inputRotateLeftRightPercent * Time.deltaTime;
        float speed= m_inputMoveForwardPercent > 0 ? m_moveForwardSpeed : m_moveBackwardSpeed;
        float move = speed * m_inputMoveForwardPercent * Time.deltaTime;
        m_whatToMove.Rotate(0, rotate, 0);
        m_whatToMove.Translate(0, 0, move);
    }
}
