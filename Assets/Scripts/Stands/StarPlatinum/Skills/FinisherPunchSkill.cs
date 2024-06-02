using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JJBA.Stands.Movement;
using JJBA.Stands.StarPlatinum.Controller;
using JJBA.UI;
using System.Threading.Tasks;
using JJBA.Core;
using JJBA.Combat;
using JJBA.Audio;

namespace JJBA.Stands.StarPlatinum.Skill
{
    public class FinisherPunchSkill : MonoBehaviour
    {
        [SerializeField] private float _cooldown = 0.5f;
        private float _cooldownTimer = 0f;
        private StandMover _mover;
        private CooldownUIManager _cooldownUIManager;
        private SPController _standController;
        private Animator _animator;
        private PunchEvent _punchEvent;
        private AudioManager _audioManager;
        private DynamicHitBox _dynamicHitBox;
        private GameObject _user;

        public void Initialize(SPController standController, GameObject user)
        {
            _standController = standController;
            _user = user;

            _mover = GetComponent<StandMover>();
            _animator = GetComponentInChildren<Animator>();
            _punchEvent = GetComponentInChildren<PunchEvent>();
            _dynamicHitBox = GetComponentInChildren<DynamicHitBox>();
            _cooldownUIManager = _user.GetComponent<CooldownUIManager>();
            _audioManager = GetComponentInChildren<AudioManager>();

            _punchEvent.punchEvent.AddListener(Punch);
        }

        private void Update()
        {
            if (_cooldownTimer > 0) _cooldownTimer -= Time.deltaTime;
        }

        public void Use()
        {
            if (!_standController.IsActive() || _cooldownTimer > 0) return;

            _cooldownTimer = _cooldown;

            string soundName = "FinisherPunch" + Random.Range(1, 3);
            Debug.Log(soundName);
            _audioManager.Play(soundName);

            _animator.SetTrigger("FinisherPunch");

            _mover.UsingSkill();

            if (_cooldownUIManager != null)
                _cooldownUIManager.AddCooldownTimer(_cooldown, "Star Finisher Punch");
        }

        private async void Punch()
        {
            _dynamicHitBox.CreateHitBox(Vector3.forward * 0.5f, new Vector3(1f, 1f, 2f), (collider) =>
            {
                if (collider.transform == _user.transform || !(collider is CapsuleCollider capsuleCollider))
                    return;

                Health enemyHealth = collider.transform.GetComponent<Health>();
                Rigidbody enemyRigidbody = collider.transform.GetComponent<Rigidbody>();
                Damage damage;

                damage = new()
                {
                    damageValue = 25f,
                    from = transform.position,
                    forse = transform.forward * 10f,
                    type = DamageType.PUNCH_FINISHER
                };

                if (enemyHealth != null)
                    collider.transform.GetComponent<Health>().GetDamage(damage);
            }, true);

            await Task.Delay((int)(0.5f * 1000));

            _mover.Idle();
        }
    }
}
