using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerAttack : NetworkBehaviour
{
    //true gdy trzymasz battle axe 
    //aktualizowany jest w skrypcie HandItem na prefabie gracza
    public bool enableBattleAxe = false;

    
    void Update()
    {
        //zwraca nic je�li nie jeste� w�a�cicielem tego prefabu gracza
        if (!IsOwner) return;

        if (enableBattleAxe)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                GetComponent<AnimController>().animator.SetBool("battleaxe", true);

                //raycast na wroga czy co� i zadanie mu dmg
            }
            else
            {
                //Wy��czenie animacji
                GetComponent<AnimController>().animator.SetBool("battleaxe", false);
            }
        }
        else
        {
            //Wy��czenie animacji
            GetComponent<AnimController>().animator.SetBool("battleaxe", false);
        }
    }
}
