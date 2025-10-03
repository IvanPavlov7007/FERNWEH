using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Sailboat.UI;
using UnityEngine.InputSystem;

namespace Sailboat
{
    public class G : Singleton<G>
    {
        public Camera mainCam;
        public Player player;
        public WindManager windManager;
        public PlayerInput playerInput;
        public PlayerInputController playerInputController;
        public DebugInputController debugInputController;
        public UIInputController uIInputController;

        public Inventory inventory;
        
        public GameManager gameManager;
        [Space]
        public DialogUI dialogUI;
        public TradeUI tradeUI;
        public BlockingButton blockingButton;
        public InventoryUI inventoryUI;

        [Space]
        public SoundManager soundManager;

    }
}
