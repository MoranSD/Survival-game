using Weapon;
using UnityEngine;
using System.Collections.Generic;
using Interfaces;
using Cysharp.Threading.Tasks;

namespace GameItems
{
    [RequireComponent(typeof(Animator))]
    internal class Axe : GameItemObject
    {
        [SerializeField] private BladeCollider _bladeCollider;

        internal Animator Animator { get; private set; }
        protected internal override IGameItemData Data { get => _data; protected set => _data = (AxeGameItemData)value; }
        protected internal override bool IsActive { get; protected set; }

        private AxeGameItemData _data;

        private GameObject _model;
        private bool _isHitting;

        private void OnEnable()
        {
            Animator = GetComponent<Animator>();
            _model = transform.GetChild(0).gameObject;
        }
        internal override void InitData(IGameItemData data)
        {
            _data = (AxeGameItemData)data;
        }
        internal override void Enter()
        {
            Animator.SetTrigger("Show");
        }
        internal override void InteractUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _isHitting == false)
                Animator.SetTrigger("Hit");
        }
        async internal override void Exit()
        {
            await UniTask.WaitWhile(() => _isHitting);

            Animator.SetTrigger("Hide");

            await UniTask.WaitWhile(() => _model.activeSelf);

            IsActive = false;
        }
        private void OnBeginHit() => _isHitting = true;
        private void OnHit()
        {
            List<IDamageable> targets = _bladeCollider.GetAllFoundedTargets<IDamageable>();
            foreach (var target in targets)
            {
                float damage = _data.Damage;

                if (_data.TargetTypes.Contains(target.type))
                    damage *= _data.DamageMultiplyForTypeTarget;

                target.ApplyDamage(damage);
            }
        }
        private void OnEndHit() => _isHitting = false;
        private void HideModel() => _model.SetActive(false);
        private void ShowModel() => _model.SetActive(true);
        private void OnEndShowAnimation() => IsActive = true;
    }
}
