using System;
using DG.Tweening;
using Extensions;
using Others;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Ui.Components
{
    public class TapZone : MonoBehaviour
    {
        public Action<int> Used;


        [SerializeField]
        private float duration;

        [SerializeField]
        private float backDuration;

        [SerializeField]
        private FloatRange anglesRange;

        [SerializeField]
        private Ease ease;

        [Space]
        [SerializeField]
        private TapZoneAngleMultiplierPair[] zoneAngles;

        [Space]
        [SerializeField]
        private RectTransform pointer;

        [SerializeField]
        private bool loop;

        [SerializeField]
        private Text rewardText;

        private Tween _animation;

        private Vector3 _from;
        private Vector3 _to;

        private TapZoneState _state;

        private float CurrentWrappedAngle => AnglesUtility.WrapAngle(pointer.localEulerAngles.z);

        private int reward;

        private void Awake()
        {
            _from = new Vector3(0, 0, anglesRange.min);
            _to = new Vector3(0, 0, anglesRange.max);
        }

        private void OnEnable()
        {
            _animation?.Kill();
            pointer.localRotation = Quaternion.Euler(_from);
            _state = TapZoneState.Idle;

            Activate();
        }

        public void SetReward(int value)
        {
            reward = value;
        }

        public int Use()
        {
            if (_state == TapZoneState.Active || _state == TapZoneState.Idle)
            {
                int multiplier = GetZoneMultiplier();
                print(multiplier);
                _animation?.Kill();
                Used?.Invoke(multiplier);

                return multiplier;
            }

            return 1;
        }

        public void Activate()
        {
            _state = TapZoneState.Active;
            pointer.localRotation = Quaternion.Euler(_from);

            if (_state != TapZoneState.Back)
            {
                _animation?.Kill();
                _animation = DOVirtual.Float(anglesRange.min, anglesRange.max, duration, UpdateRotation)
                    .SetEase(ease)
                    .OnComplete(() => _state = TapZoneState.Idle);
                    //.OnUpdate(UpdateRewardTExt);

                if (loop)
                    _animation.SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void UpdateRewardTExt()
        {
            print("Rew");
            rewardText.text = (reward * GetZoneMultiplier()).ToString();
        }


        private void UpdateRotation(float angle)
        {
            UpdateRewardTExt();

            pointer.localRotation = Quaternion.Euler(0, 0, angle);
        }

        public void Deactivate()
        {
        }


        public void Back()
        {
            float distanceBetweenRange = MathfExtension.Distance(anglesRange.min, anglesRange.max);
            float distanceBetweenCurrent = MathfExtension.Distance(anglesRange.min, CurrentWrappedAngle);
            float newDuration = backDuration / (distanceBetweenRange / distanceBetweenCurrent);

            _state = TapZoneState.Back;
            _animation?.Kill();
            _animation = DOVirtual.Float(CurrentWrappedAngle, anglesRange.min, newDuration, UpdateRotation)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    _state = TapZoneState.Active;
                    Activate();
                });
        }

        private int GetZoneMultiplier()
        {
            int index = 0;

            float angle = CurrentWrappedAngle;

            while (zoneAngles[index].maxAngle > angle && index < zoneAngles.Length - 1)
                index++;

            return zoneAngles[index].multiplier;
        }


        #region Inner

        private enum TapZoneState
        {
            Idle,
            Active,
            Back,
            Finished
        }


        [Serializable]
        public class TapZoneAngleMultiplierPair
        {
            public float maxAngle;
            public int multiplier;
        }

        #endregion
    }
}