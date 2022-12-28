using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UtilityClass
{


    public static float GetRotationFromDirection(Vector2 direction)
    {
        // Get the angle of the direction vector, relative to the x-axis
        float angle = Mathf.Atan2(direction.y, direction.x);

        // Convert the angle from radians to degrees
        float rotation = angle * Mathf.Rad2Deg;

        // Return the rotation
        return rotation;
    }  

    public static Vector2 GetDirectionFromRotation(float rotation)
    {
        // Convert the rotation from degrees to radians
        float angle = rotation * Mathf.Deg2Rad;

        // Create a direction vector using the angle
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        // Return the direction vector
        return direction;
    }
}
