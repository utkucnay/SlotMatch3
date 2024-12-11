using UnityEngine;
using Case.Shared.ButtonView;
using System;
using TMPro;

namespace Case.Match3.Spin
{
    public class SpinView : MonoBehaviour
    {
        public event Action<object, System.EventArgs> OnSpinButtonClick;

        [SerializeField] private ButtonView spinButton;
        [SerializeField] private TextMeshPro spinButtonText;

        private void OnEnable()
        {
            spinButton.OnClickEvent += SpinButtonView_OnClickEvent;
        }

        private void OnDisable()
        {
            spinButton.OnClickEvent -= SpinButtonView_OnClickEvent;
        }

        private void SpinButtonView_OnClickEvent(object sender, System.EventArgs args)
        {
            OnSpinButtonClick?.Invoke(sender, args);
        }

        public void SetInteractableSpinButton(bool interactable)
        {
            spinButton.Interactable = interactable;
        }

        public void SetSpinText(string text)
        {
            spinButtonText.text = text;
        }
    }
}