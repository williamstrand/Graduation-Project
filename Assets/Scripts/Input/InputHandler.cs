using System;
using UnityEngine;

namespace WSP.Input
{
    public static class InputHandler
    {
        public static Action<Vector2> OnTarget { get; set; }
        public static Action OnCancel { get; set; }
        public static Action OnInventory { get; set; }
        public static Action OnStop { get; set; }

        public static Action<int> OnSpecialAttack { get; set; }

        public static Vector2 MousePosition
        {
            get
            {
                var mousePosition = controls.General.MousePosition.ReadValue<Vector2>();
                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                return worldPosition;
            }
        }

        static Controls controls;
        static UnityEngine.Camera mainCamera;

        public static Controls Initialize()
        {
            controls?.Dispose();

            mainCamera = UnityEngine.Camera.main;
            controls = new Controls();
            controls.Enable();
            controls.Game.Target.performed += _ => OnTarget?.Invoke(MousePosition);
            controls.Game.CancelTarget.performed += _ => OnCancel?.Invoke();
            controls.Menu.Inventory.performed += _ => OnInventory?.Invoke();
            controls.Game.Stop.performed += _ => OnStop?.Invoke();
            controls.Game.Special1.performed += _ => OnSpecialAttack?.Invoke(0);
            controls.Game.Special2.performed += _ => OnSpecialAttack?.Invoke(1);
            controls.Game.Special3.performed += _ => OnSpecialAttack?.Invoke(2);
            controls.Game.Special4.performed += _ => OnSpecialAttack?.Invoke(3);
            return controls;
        }
    }
}