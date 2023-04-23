using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter,IKitchenObjectParent
{
    public event EventHandler OnPLayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) return;

        if (!HasKitchenObject())
        {
           KitchenObject kitchenObject =  KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPLayerGrabbedObject?.Invoke(this,EventArgs.Empty);
        }


    }

}
