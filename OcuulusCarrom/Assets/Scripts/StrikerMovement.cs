using UnityEngine;

public class StrikerMovement : MonoBehaviour
{

    public Transform LeftBound, RightBound;
    public float StrikerRadius = 0.04f;
    private Vector3 initPos, initRot; 
    void Start()
    {
 
        initPos = transform.position;
        initRot = transform.eulerAngles;
     
    }
    public void MoveStriker(string dir,float Increment)
    {
        
        if(dir == "LEFT")
        {
            transform.position = FindStrikerNextPosition(-LeftBound.transform.right,Increment);
        }
        else
        {
            transform.position = FindStrikerNextPosition(LeftBound.transform.right, Increment);
        }
        GetComponent<StrikerLine>().DrawLine();
    }

    public void RotateStriker(float Increment)
    {
       
        transform.Rotate(transform.up, Increment);
        GetComponent<StrikerLine>().DrawLine();
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
    public void PlaceStriker()
    {
        Vector3 newPosition = (LeftBound.transform.position+RightBound.transform.position)/2;
        bool isThisCorrectPosition;
        for (int i = 0; i <= 40; i++)
        {
            newPosition += LeftBound.transform.right * 0.02f;
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
        
        }
        transform.position = newPosition;
        transform.eulerAngles = initRot;
    }
}
