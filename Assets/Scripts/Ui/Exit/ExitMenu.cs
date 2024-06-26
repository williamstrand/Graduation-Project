﻿using System;
using UnityEngine;

namespace WSP.Ui.Exit
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Button yesButton;
        [SerializeField] UnityEngine.UI.Button noButton;

        public Action OnExit { get; set; }
        public Action OnCancel { get; set; }

        public void Open()
        {
            gameObject.SetActive(true);
            yesButton.onClick.AddListener(Yes);
            noButton.onClick.AddListener(No);
        }

        void Yes()
        {
            OnExit?.Invoke();
            Destroy(gameObject);
        }

        void No()
        {
            OnCancel?.Invoke();
            Destroy(gameObject);
        }
    }
}