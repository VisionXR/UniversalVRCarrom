using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerMovement : MonoBehaviour
{

    public Transform LeftBound, RightBound;
    public float StrikerRadius = 0.04f;
    void Start()
    {
        InputManager.instance.UserSwiped += MoveStriker;
    }
    public void MoveStriker(string dir,float Increment)
    {
        if(dir == "LEFT")
        {
            transform.position = FindStrikerNextPosition(-transform.right,Increment);
        }
        else
        {
            transform.position = FindStrikerNextPosition(transform.right, Increment);
        }
    }

    private Vector3 FindStrikerNextPosition(Vector3 dir,float Increment)
    {
        Vector3 newPosition = transform.position;
        bool isThisCorrectPosition;
        for (int i = 0; i <= 40; i++)
        {
            newPosition += dir * Increment;
            if (newPosition.x > LeftBound.position.x && newPosition.x < RightBound.position.x)
            {
                isThisCorrectPosition = true;
                Collider[] cols = Physics.OverlapSphere(newPosition, StrikerRadius);
                foreach (Collider c in cols)
                {
                    if (c.gameObject.tag == "White" || c.gameObject.tag == "Red" || c.gameObject.tag == "Black")
                    {
                        isThisCorrectPosition = false;
                        break;
                    }

                }
                if (isThisCorrectPosition)
                {
                    break;
                }
            }
            else
            {
                newPosition -= dir * Increment;
                break;
            }
        }
        return newPosition;
    }
}
