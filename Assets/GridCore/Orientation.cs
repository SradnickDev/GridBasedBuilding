using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Orientation : MonoBehaviour {

    public enum HorizontalPoint
    {
        None,
        North,
        West,
        East,
        NorthWest,
        NorthEast,
        South,
        SouthEast,
        SouthWest,
        Up,
        ForwardUp,
        Down
    }
    public enum VerticalPoint
    {
        None, Forward, ForwardUp, ForwardDown, Down, Up
    }

    public HorizontalPoint Horizontal(float rotationYAngle)
    {
        HorizontalPoint m_horizontalPoint = HorizontalPoint.None;

        if (rotationYAngle >= 337.5f || rotationYAngle < 22.5f)
        {
            m_horizontalPoint = HorizontalPoint.North;
        }
        else if (rotationYAngle >= 22.5f && rotationYAngle < 67.5f)
        {
            m_horizontalPoint = HorizontalPoint.NorthEast;
        }
        else if (rotationYAngle >= 67.5f && rotationYAngle < 112.5f)
        {
            m_horizontalPoint = HorizontalPoint.East;
        }
        else if (rotationYAngle >= 112.5f && rotationYAngle < 157.5f)
        {
            m_horizontalPoint = HorizontalPoint.SouthEast;
        }
        else if (rotationYAngle >= 157.5f && rotationYAngle <= 202.5f)
        {
            m_horizontalPoint = HorizontalPoint.South;
        }
        else if (rotationYAngle >= 202.5f && rotationYAngle < 247.5f)
        {
            m_horizontalPoint = HorizontalPoint.SouthWest;
        }
        else if (rotationYAngle >= 247.5f && rotationYAngle < 292.5f)
        {
            m_horizontalPoint = HorizontalPoint.West;
        }
        else if (rotationYAngle >= 292.5f && rotationYAngle < 337.5f)
        {
            m_horizontalPoint = HorizontalPoint.NorthWest;
        }
        return m_horizontalPoint;
    }
    public VerticalPoint Vertical (float rotationXAngle)
    {
        VerticalPoint m_verticalPoint = VerticalPoint.None;

        if (rotationXAngle > 67.5f && rotationXAngle <= 90)
        {
            m_verticalPoint = VerticalPoint.Down;
        }
        if (rotationXAngle > 22.5f && rotationXAngle <= 67.5f)
        {
            m_verticalPoint = VerticalPoint.ForwardDown;
        }
        if (rotationXAngle < 22.5f || rotationXAngle > 337.5f)
        {
            m_verticalPoint = VerticalPoint.Forward;
        }
        if (rotationXAngle <= 337.5f && rotationXAngle > 292.5f)
        {
            m_verticalPoint = VerticalPoint.ForwardUp;
        }
        if (rotationXAngle <= 292.5f && rotationXAngle >= 270)
        {
            m_verticalPoint = VerticalPoint.Up;
        }
        return m_verticalPoint;
    }
}
