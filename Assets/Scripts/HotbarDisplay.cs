using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class HotbarDisplay : StaticInventoryDisplay
{
    private int _maxIndexSize = 9;
    private int _currentIndex = 0;
    private int _previousIndex = -1;
    private PlayerInput _playerInput;
    //[SerializeField] private EquipItemSystem PlayerEquipItemSystem;
    [NonSerialized] private InventoryItemData highlightedItemData;
    public InventoryItemData HighlightedItemData => highlightedItemData;
    //Check if current selected slot has been changed due to pickup
    public static UnityAction OnHotbarSlotChanged;


    private void Awake()
    {
        _playerInput = new PlayerInput();

    }

    protected override void Start()
    {
        base.Start();

        _currentIndex = 0;
        _maxIndexSize = slots.Length - 1;

        slots[_currentIndex].ToggleHighlight();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        _playerInput.Enable();
        _playerInput.Player.Hotbar1.performed += Hotbar1;
        _playerInput.Player.Hotbar2.performed += Hotbar2;
        _playerInput.Player.Hotbar3.performed += Hotbar3;
        _playerInput.Player.Hotbar4.performed += Hotbar4;
        _playerInput.Player.Hotbar5.performed += Hotbar5;
        _playerInput.Player.Hotbar6.performed += Hotbar6;
        _playerInput.Player.Hotbar7.performed += Hotbar7;
        _playerInput.Player.Hotbar8.performed += Hotbar8;
        _playerInput.Player.Hotbar9.performed += Hotbar9;
        _playerInput.Player.Hotbar10.performed += Hotbar10;
        _playerInput.Player.UseItem.performed += UseItem;
        OnHotbarSlotChanged += InstantiateItem;


    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _playerInput.Disable();
        _playerInput.Player.Hotbar1.performed -= Hotbar1;
        _playerInput.Player.Hotbar2.performed -= Hotbar2;
        _playerInput.Player.Hotbar3.performed -= Hotbar3;
        _playerInput.Player.Hotbar4.performed -= Hotbar4;
        _playerInput.Player.Hotbar5.performed -= Hotbar5;
        _playerInput.Player.Hotbar6.performed -= Hotbar6;
        _playerInput.Player.Hotbar7.performed -= Hotbar7;
        _playerInput.Player.Hotbar8.performed -= Hotbar8;
        _playerInput.Player.Hotbar9.performed -= Hotbar9;
        _playerInput.Player.Hotbar10.performed -= Hotbar10;
        _playerInput.Player.UseItem.performed -= UseItem;
        OnHotbarSlotChanged -= InstantiateItem;
    }

    #region Hotbar Select Methods

    private void Hotbar1(InputAction.CallbackContext obj)
    {
        SetIndex(0);
    }
    private void Hotbar2(InputAction.CallbackContext obj)
    {
        SetIndex(1);
    }
    private void Hotbar3(InputAction.CallbackContext obj)
    {
        SetIndex(2);
    }
    private void Hotbar4(InputAction.CallbackContext obj)
    {
        SetIndex(3);
    }
    private void Hotbar5(InputAction.CallbackContext obj)
    {
        SetIndex(4);
    }
    private void Hotbar6(InputAction.CallbackContext obj)
    {
        SetIndex(5);
    }
    private void Hotbar7(InputAction.CallbackContext obj)
    {
        SetIndex(6);
    }
    private void Hotbar8(InputAction.CallbackContext obj)
    {
        SetIndex(7);
    }
    private void Hotbar9(InputAction.CallbackContext obj)
    {
        SetIndex(8);
    }
    private void Hotbar10(InputAction.CallbackContext obj)
    {
        SetIndex(9);
    }
    #endregion

    private void Update()
    {
        if (_playerInput.Player.MouseWheel.ReadValue<float>() > 0.1f)
        {
            ChangeIndex(1);

        }
        if (_playerInput.Player.MouseWheel.ReadValue<float>() < -0.1f)
        {
            ChangeIndex(-1);

        }


       
    }
    private void InstantiateItem()

    {
        highlightedItemData = slots[_currentIndex].AssignedInventorySlot.ItemData;

        /*
        if ((PlayerEquipItemSystem.ItemInHand && _previousIndex == _currentIndex) || highlightedItemData == null)
        {
            PlayerEquipItemSystem.UnEquipItem();
        }else if (highlightedItemData != null){


            _previousIndex = _currentIndex;
            PlayerEquipItemSystem.UnEquipItem();
            PlayerEquipItemSystem.EquipItem();
        } 
        */
    }
    private void UseItem(InputAction.CallbackContext obj)
    {

        if (slots[_currentIndex].AssignedInventorySlot.ItemData!= null)
        {
            //highlightedItemData = slots[_currentIndex].AssignedInventorySlot.ItemData;
            //if (PlayerEquipItemSystem.ItemInHand)
            //{
                if (slots[_currentIndex].AssignedInventorySlot.StackSize >=  1)
                {
                    slots[_currentIndex].AssignedInventorySlot.ItemData.UseItem();
                    slots[_currentIndex].AssignedInventorySlot.AddToStack(-1);
                    PlayerInventoryHolder.OnPlayerInventoryChanged?.Invoke();
                    if(slots[_currentIndex].AssignedInventorySlot.StackSize < 1)
                    {
                        slots[_currentIndex].AssignedInventorySlot.ClearSlot();
                        PlayerInventoryHolder.OnPlayerInventoryChanged?.Invoke();
                        //PlayerEquipItemSystem.UnEquipItem();
                        OnHotbarSlotChanged?.Invoke();
                    }
                }else
                {
                    //Item is from Type weapon or throw
                  
                    slots[_currentIndex].AssignedInventorySlot.ItemData.UseItem();
                  
                }

               
            //}
            
        }

    }

    private void ChangeIndex(int direction)
    {
        slots[_currentIndex].ToggleHighlight();
        _currentIndex += direction;
        if(_currentIndex > _maxIndexSize) {
            _currentIndex = 0;

        }
        if(_currentIndex < 0) {
            _currentIndex = _maxIndexSize;
        }


        slots[_currentIndex].ToggleHighlight();
        OnHotbarSlotChanged?.Invoke();
    }

    private void SetIndex(int newIndex)
    {
        slots[_currentIndex].ToggleHighlight();
   

        if (newIndex < 0)
        {
            newIndex = 0;

        }
        if (newIndex > _maxIndexSize)
        {
            newIndex = _maxIndexSize;
        }

  
        _currentIndex= newIndex;    
        slots[_currentIndex].ToggleHighlight();
        OnHotbarSlotChanged?.Invoke();

    }
}
