using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandHeldObject : PlayerInput.IPlayerActions
{
    void AnAttachedCarrier(CarrierSystem attachedCarrier);
    void OnEquip();
    void OnUnequip();
}
