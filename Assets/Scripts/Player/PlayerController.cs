using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Vector3 movementVector;

    private float moveSide;
    private float moveFront;

    [SerializeField] private float speed;
    [SerializeField] private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector = new Vector3(moveSide, 0, moveFront);
        characterController.Move(movementVector * Time.deltaTime * speed);
    }

    private void FixedUpdate()
    {
        
    }

    void OnMove(InputValue value) 
    {
        Debug.Log("Move");
        moveSide = value.Get<Vector2>().x;
        moveFront = value.Get<Vector2>().y;
    }
}
