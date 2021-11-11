using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Este script se encarga del comportamiento de los menus individualmente.
*/

public class Menu : MonoBehaviour
{

    public string menuName;
    public bool open;

    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }

}
