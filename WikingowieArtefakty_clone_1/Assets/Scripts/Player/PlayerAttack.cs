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
        //zwraca nic jeœli nie jesteœ w³aœcicielem tego prefabu gracza
        if (!IsOwner) return;

        if (enableBattleAxe)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                GetComponent<AnimController>().animator.SetBool("battleaxe", true);

                //raycast na wroga czy coœ i zadanie mu dmg
            }
            else
            {
                //Wy³¹czenie animacji
                GetComponent<AnimController>().animator.SetBool("battleaxe", false);
            }
        }
        else
        {
            //Wy³¹czenie animacji
            GetComponent<AnimController>().animator.SetBool("battleaxe", false);
        }
    }
}
