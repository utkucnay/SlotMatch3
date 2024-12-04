using System;
using UnityEngine;

namespace Case.Shared.ButtonView
{
    [RequireComponent(typeof(Collider2D))]
    public class ButtonView : MonoBehaviour
    {
        public event Action<object, EventArgs> OnClickEvent;

        public SpriteRenderer spriteRenderer;
        
        [SerializeField] private bool interactable = true;
        public bool Interactable
        {
            get { return interactable; }
            set 
            {
                interactable = value; 
                if (interactable)
                {
                    spriteRenderer.color = Color.white;
                }
                else 
                {
                    spriteRenderer.color = Color.gray;
                }
            }
        }


        private void OnMouseDown()
        {
            if (!interactable) return;

            OnClickEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}