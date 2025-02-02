using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform orientation;
    public float arrowKeyRotationSpeed = 100f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * (PauseMenu.mouseValue * 50f);
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (PauseMenu.mouseValue * 50f);

        float arrowHorizontal = 0f;
        float arrowVertical = 0f;

        if (Input.GetKey(KeyCode.RightArrow)) arrowHorizontal += 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) arrowHorizontal -= 1f;
        if (Input.GetKey(KeyCode.DownArrow)) arrowVertical -= 1f;
        if (Input.GetKey(KeyCode.UpArrow)) arrowVertical += 1f;

        
        arrowHorizontal *= Time.deltaTime * arrowKeyRotationSpeed;
        arrowVertical *= Time.deltaTime * arrowKeyRotationSpeed;

       
        yRotation += mouseX + arrowHorizontal;
        xRotation -= mouseY + arrowVertical;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}