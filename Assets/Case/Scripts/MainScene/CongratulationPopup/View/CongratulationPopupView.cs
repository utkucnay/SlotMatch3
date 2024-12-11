using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace Case.MainScene.CongratulationPopup
{
    public class CongratulationPopupView : MonoBehaviour
    {
        [SerializeField] private Transform[] particles;
        [SerializeField] private Transform[] particleEndPositions;

        public async UniTask GameEndAnimation(CongratulationPopupAnimationData animationData)
        {
            for (int i = 0; i < particles.Length; ++i)
            {
                Transform particle = particles[i];
                Transform particleEndPosition = particleEndPositions[i];

                particle.DOJump(
                    particleEndPosition.transform.position, 
                    animationData.gameEndAnimationJumpPower, 
                    animationData.gameEndAnimationJumpCount, 
                    animationData.gameEndAnimationDuration).SetEase(Ease.InBounce);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(animationData.gameEndAnimationDuration + animationData.gameEndAnimationDurationOffset));
        }
    }
}
