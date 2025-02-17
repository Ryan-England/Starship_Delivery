using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCustomer : MonoBehaviour
{
    public bool customer;
    public CharacterController cc;
    public GameObject targetposition;
    public float distance = 10f; 
    public Transform cam; 

    public float turnSmoothTime = 0.1f; 
    float turnSmoothVelocity;
    public float speed = 5f;
    public Animator movement;
    public int limit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(customer){
            // Debug.Log("customer mode started... Moving...");
            bool x = targetposition.transform.position.x != transform.position.x; 
            bool y = targetposition.transform.position.z != transform.position.z;
            distance = Vector3.Distance(transform.position, targetposition.transform.position);
            if (distance >= 2f) {
                    // Debug.Log("moving...");
                    movement.SetTrigger("moving");
                    Vector3 direction = targetposition.transform.position - transform.position;
                    // Debug.Log(direction);
                    // direction.y = 0.1ff;
                    direction.Normalize(); 
                    // float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f); 
                    // Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;   
                    cc.Move(direction * speed * Time.deltaTime);
            }
            else{
                movement.ResetTrigger("moving");
                customer = false;
            }
        }
        int randomNumber = UnityEngine.Random.Range(0, limit); // 0 is inclusive, 11 is exclusive
        if(randomNumber == 4){
            customer = true;
        }
    }
}
