using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Transform cam;
    

    public float defaultSpeed = 6f;

    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    Animator _animator;

    private GameObject touchingObject;

    // public GameObject aimTarget;

    private CharacterController _characterController;

    // public override void OnStartClient()
    // {
    //     base.OnStartClient();
    //     if (base.IsOwner) {
    //         Debug.Log("is owner on start");
    //         _animator = GetComponent<Animator>();
    //         _inventroyManager = GetComponent<InventroyManager>();
    //     }
    // }

    private void Awake()
    {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
    }

    // TODO: decrease stamina/fatigue/thirst/hunger on some actions?
    void Update()
    {
        // if (!base.IsOwner) {
        //     Debug.Log("is not owner on update");
        //     return;
        // }
        
        Debug.Log("is owner on update");

        float horizontal = Input.GetAxisRaw("Horizontal");
       
       
        // 0 - 180 - looks to the right, 90 is max
        // 180 - 360 - looks to the left, 270 is max
        // else looks to the left
        // 0 = forward
        // aim - 10 to the right is max
        // -10 is the max to the left
        // float cameraLookY = cam.eulerAngles.y;
        // if (cameraLookY < 180) {
        //     float result = cameraLookY / 90; 
        //     if (result > 1) {
        //         result = 1;
        //     }
        //     float lookY = 3 * result;
        //     aimTarget.transform.position = new Vector3(lookY, aimTarget.transform.position.y, aimTarget.transform.position.z);
        // } else {
        //     float newLook = cameraLookY - 360;
        //     float result2 = newLook / 90; 
        //     float lookY2 = 3 * result2;
        //     aimTarget.transform.position = new Vector3(lookY2, aimTarget.transform.position.y, aimTarget.transform.position.z);
        // }

        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        bool isMoving = direction.magnitude >= 0.1f;
        bool isMovingFast = Input.GetKey(KeyCode.LeftShift) && isMoving;
        float speed = isMovingFast ? 8f : defaultSpeed; 

        bool isRolling = _animator.GetCurrentAnimatorStateInfo(0).IsTag("roll");
        bool isJumping = _animator.GetCurrentAnimatorStateInfo(0).IsTag("jump");
        bool isFighting = _animator.GetCurrentAnimatorStateInfo(0).IsTag("fight");

        // if (Input.GetMouseButtonDown(0)) {
        //     _animator.SetTrigger("LeftPunch");
        //     _animator.SetBool("Fight", true);   
        // }

        // if (Input.GetKeyDown(KeyCode.E) && touchingObject) {
        //     _inventroyManager.Interact(touchingObject);
        // }
        
        // if (Input.GetMouseButtonDown(1)) {
        //     _animator.SetTrigger("RightPunch");
        //     _animator.SetBool("Fight", true);   
        // }

        // if (Input.GetKeyDown(KeyCode.X)) {
        //     _animator.SetTrigger("Roll");
        //     _animator.SetBool("Fight", false);  
        // }
        
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     _animator.SetTrigger("Jump");
        //     _animator.SetBool("Fight", false);  
        // }

        if (!isRolling && !isJumping && !isFighting && isMoving) {
            _animator.SetBool("Fight", false);  
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            _characterController.Move(moveDir.normalized * speed * Time.deltaTime);
            
        } 
        
        _animator.SetBool("Walk", isMoving);    
        _animator.SetBool("Run", isMovingFast);
    }

    // void OnTriggerEnter(Collider other) {
    //     touchingObject = other.gameObject;
    // }
     
    // void OnTriggerExit(Collider other) {
    //     touchingObject = null;
    // }

    // IEnumerator HitFromLeft() {

    //     yield return new WaitForSeconds (3.0f);
    //     _animator.SetTrigger("HitFromLeft");
    // }
    
    // IEnumerator HitFromRight() {
    //     yield return new WaitForSeconds (8.0f);
    //     _animator.SetTrigger("HitFromRight");
    // }
}
