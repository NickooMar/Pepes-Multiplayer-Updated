using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
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
        if (PV.IsMine) //El PV.IsMine es para interactuar con el personaje local.
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

        if(Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }

    }

    void Look() //Encargado del movimiento de la camara con respecto al mouse.
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move() //Funcion que se encarga de mover al jugador.
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
        grounded = _grounded; //Detecta si esta en el suelo o no.
    }

    void FixedUpdate()
    {
        if(!PV.IsMine) return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime); //Hacemos que el movimiento sea sincronizado.
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

        if(PV.IsMine) //Si es el local player.
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);       //Enviamos el indice del item por la red.
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }

    //Esta función se usa para que el player no local pueda ver cuando el player local cambie el arma, es decir sincroniza el arma que esta usando con el servidor.
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) //Esta funcion se llama cuando la información llega
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]); //Si es un player distinto al local, le enviamos el indice del item y equipamos el item.
        }
    }


    public void TakeDamage(float damage) //Agregar la inteface al principio del script nos permite llamar a la función TakeDamage
    {
        Debug.Log("Player took damage");
    }

}
