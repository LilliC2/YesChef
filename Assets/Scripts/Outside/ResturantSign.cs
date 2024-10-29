using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResturantSign : GameBehaviour
{
    private void Start()
    {
        _UI.resturantSign_TMPText = GetComponentInChildren<TMP_Text>();
    }
    private void OnMouseDown()
    {




        _UI.ActivateResturantSignRenamePanel();
    }

}
