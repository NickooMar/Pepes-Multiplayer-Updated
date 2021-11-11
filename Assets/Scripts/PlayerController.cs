using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex = -1;
    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    Rigidbody rb;

    PhotonView PV;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    void Update()
    {
        if(!PV.IsMine) return;
        Look();
        Move();
        Jump();
        for(int i=0; i < items.Length; i++)
        {
            if(Input.GetKeyDown((i + 1).ToString())) //Recorre los items que tenemos y si presionamos la tecla del lugar del item, equipamos ese item.
            {
                EquipItem(i);
                break;
            }
        }

    }

    void Look() //Encargado del movimiento de la camara con respecto al mouse
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime); //Aumentamos la velocidad si apreta el LeftShift.
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void setGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if(!PV.IsMine) return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void EquipItem(int _index)
    {
        if(_index == previousItemIndex) return;

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true); //Activamos el item seleccionado.
        
        if(previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false); //Desactivamos el item anterior.
        }

        previousItemIndex = itemIndex; //Guardamos el item anterior.

    }


}