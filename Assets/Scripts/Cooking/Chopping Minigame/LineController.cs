using UnityEngine;

public class LineControl : MonoBehaviour
{
    public float speed = 5f;          // Speed of the line
    public float upperBound = 4.5f;  // Upper bound of movement
    public float lowerBound = -4.5f; // Lower bound of movement
    private bool movingLeft = true;    // Determines the direction of movement

    void Update()
    {
        // Move the line in the current direction
        float move = speed * Time.deltaTime;
        if (movingLeft)
        {
            transform.Translate(Vector3.down * move);

            // If the line reaches the upper bound, switch to moving down
            if (transform.position.x >= upperBound)
            {
                movingLeft = false;
            }
        }
        else
        {
            transform.Translate(Vector3.up * move);

            // If the line reaches the lower bound, switch to moving up
            if (transform.position.x <= lowerBound)
            {
                movingLeft = true;
            }
        }
    }
}