using UnityEngine;

public class AIStrikerMovement : MonoBehaviour
{
    public Transform LeftBound, RightBound;
    private float StrikerRadius = 0.04f;
    private Vector3 initPos, initRot;
    private void Start()
    {
        initPos = transform.position;
        initRot = transform.eulerAngles;
    }
    public Vector3 FindStrikerNextPosition(Vector3 pos, Vector3 dir)
    {
        Vector3 newPosition = pos;
        bool isThisCorrectPosition;
        for (int i = 0; i <= 40; i++)
        {
            newPosition += dir * 0.02f;
            if (newPosition.x < LeftBound.position.x && newPosition.x > RightBound.position.x)
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
        return newPosition;
    }
    public void PlaceStriker()
    {
        Vector3 newPosition = (LeftBound.transform.position + RightBound.transform.position) / 2;
        bool isThisCorrectPosition;
        for (int i = 0; i <= 40; i++)
        {
            newPosition += -LeftBound.transform.right * 0.02f;
            if (newPosition.x < LeftBound.position.x && newPosition.x > RightBound.position.x)
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
