using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Este script se utiliza para manejar los momentos de aparición de cada menu, evita que se ejecuten varios menus al mismo tiempo.
*/

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance;

    [SerializeField] Menu[] menus;


    public void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }


    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

}
