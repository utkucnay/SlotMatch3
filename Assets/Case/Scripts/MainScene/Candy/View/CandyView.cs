using UnityEngine;
using Zenject;
using System;
using Case.MainScene.Swipe;

namespace Case.MainScene.Candy
{
    public class CandyView : MonoBehaviour
    {
        public event Action<CandyView, SwipeDirection> OnSwipeEvent;
        public event Action<CandyView> OnDestroyEvent;

        [Inject] CandyViewService viewService;

        [SerializeField] SpriteRenderer candySpriteRenderer;
        [SerializeField] SwipeView swipeView;

        public bool interactable = true; 
        
        private CandyViewModel viewModel;
        public CandyViewModel ViewModel 
        { 
            get 
            {
                return viewModel;
            } 
            set 
            {
                candySpriteRenderer.sprite = viewService.GetSprite(value.CandyType);
                viewModel = value;
            } 
        }

        private void OnEnable()
        {
            swipeView.OnSwipeEvent += OnSwipe;
        }

        private void OnDisable()
        {
            swipeView.OnSwipeEvent -= OnSwipe;
        }

        private void OnDestroy()
        {

            OnDestroyEvent?.Invoke(this);
        }

        public void OnSwipe(SwipeDirection swipeDirection)
        {
            if (!interactable)
            {
                return;
            }

            OnSwipeEvent?.Invoke(this, swipeDirection);
        }

        public class Factory : PlaceholderFactory<CandyView>
        {

        }
    }
}
