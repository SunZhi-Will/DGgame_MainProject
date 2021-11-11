using System.Collections;
using UnityEngine;

namespace Frank
{
    public class StateEffectController : MonoBehaviour
    {
        public enum State { None, Diuretics, Stimulant, Steroid, Analgesics }

        public float defaultMoveSpeed = 3;
        public float varyMoveSpeed = 6;
        public float positiveDuration = 8;
        public float negativeDuration = 6;
        public float flashDuration = 1;
        [SerializeField] SkinnedMeshRenderer playerMeshRender;

        [Header("VFX")]
        [SerializeField] GameObject dizzyEffect;
        [SerializeField] GameObject recoverEffect, loseEffect;
        [SerializeField] GameObject invincibleEffect;

        [Header("Audio")]
        [SerializeField] AudioSource pickupAudioSource;

        private ActorController actorController;
        private HPController hpController;
        private AttackController attackController;

        [HideInInspector] public float diureticsTimer, stimulantTimer, steroidTimer, analgesicsTimer;
        [HideInInspector] public bool isDizzy, isInvincible, isConfusion;

        private State state1, state2, state3, state4;
        private float currentFlashDuration;
        private bool hadDizzyEffect, hadRecoverEffect, hadLoseEffect, hadInvincibleEffect;
        private bool startBoolBack = true;
        private bool startBoolDeduction = true;
        private bool isFlash;

        // Start is called before the first frame update
        void Start()
        {
            actorController = GetComponent<ActorController>();
            hpController = GetComponent<HPController>();
            attackController = GetComponentInChildren<AttackController>();

            currentFlashDuration = flashDuration;
        }

        // Update is called once per frame
        void Update()
        {
            SetProps(state1);
            SetProps(state2);
            SetProps(state3);
            SetProps(state4);

            SetEffectPrefasbScale(dizzyEffect, hadDizzyEffect, true);
            SetEffectPrefasbScale(recoverEffect, hadRecoverEffect, true);
            SetEffectPrefasbScale(loseEffect, hadLoseEffect, true);
            SetEffectPrefasbScale(invincibleEffect, hadInvincibleEffect, false);
        }

        private void SetProps(State state)
        {
            switch (state)
            {
                case State.Diuretics:
                    StartCoroutine("IE_Diuretics");
                    break;
                case State.Stimulant:
                    StartCoroutine("IE_Stimulant");
                    break;
                case State.Steroid:
                    StartCoroutine("IE_Steroid");
                    break;
                case State.Analgesics:
                    StartCoroutine("IE_Analgesics");
                    break;
                default:
                    break;
            }
        }

        IEnumerator IE_Diuretics()
        {
            diureticsTimer += Time.deltaTime;

            if (diureticsTimer <= positiveDuration)
            {
                actorController.movingSpeed = varyMoveSpeed;

                isFlash = true;
                if (diureticsTimer >= positiveDuration - currentFlashDuration)
                    SetFlashEffect(isFlash);
            }
            yield return new WaitForSeconds(positiveDuration);

            if (diureticsTimer > positiveDuration && diureticsTimer <= positiveDuration + negativeDuration)
            {
                isDizzy = true;
                hadDizzyEffect = true;

                isFlash = false;
                if (!isFlash)
                {
                    playerMeshRender.enabled = true;
                    currentFlashDuration = flashDuration;
                }
            }
            yield return new WaitForSeconds(negativeDuration);

            if (diureticsTimer > positiveDuration + negativeDuration)
            {
                diureticsTimer = 0;
                isDizzy = false;
                hadDizzyEffect = false;
                actorController.movingSpeed = defaultMoveSpeed;
                state1 = State.None;
            }
        }

        IEnumerator IE_Stimulant()
        {
            stimulantTimer += Time.deltaTime;

            if (hpController.currentHP <= hpController.maxHP.Length && stimulantTimer <= positiveDuration)
            {
                if (startBoolBack)
                {
                    StartCoroutine("IE_BoolBack");
                    startBoolBack = false;
                    hadRecoverEffect = true;
                    hadLoseEffect = false;
                }

                isFlash = true;
                if (stimulantTimer >= positiveDuration - currentFlashDuration)
                    SetFlashEffect(isFlash);
            }
            yield return new WaitForSeconds(positiveDuration);

            if (stimulantTimer > positiveDuration && stimulantTimer <= positiveDuration + negativeDuration)
            {
                if (startBoolDeduction)
                {
                    if (hpController.currentHP > 1)
                        StartCoroutine("IE_BoolDeduction");
                    startBoolDeduction = false;
                    hadRecoverEffect = false;
                    hadLoseEffect = true;
                }

                isFlash = false;
                if (!isFlash)
                {
                    playerMeshRender.enabled = true;
                    currentFlashDuration = flashDuration;
                }
            }
            yield return new WaitForSeconds(negativeDuration);

            if (stimulantTimer > positiveDuration + negativeDuration)
            {
                stimulantTimer = 0;
                hadLoseEffect = false;
                state2 = State.None;
            }
        }

        IEnumerator IE_BoolBack()
        {
            yield return new WaitForSeconds(2);
            if (hpController.currentHP < hpController.maxHP.Length)
                hpController.currentHP++;
            startBoolBack = true;
        }

        IEnumerator IE_BoolDeduction()
        {
            yield return new WaitForSeconds(2);
            SendMessage("GetHurt", gameObject);
            startBoolDeduction = true;
        }

        IEnumerator IE_Steroid()
        {
            steroidTimer += Time.deltaTime;

            if (steroidTimer <= positiveDuration)
            {
                transform.localScale = new Vector3(2, 2, 2);
                attackController.attackRadius = 1.6f;

                isFlash = true;
                if (steroidTimer >= positiveDuration - currentFlashDuration)
                    SetFlashEffect(isFlash);
            }
            yield return new WaitForSeconds(positiveDuration);

            if (steroidTimer > positiveDuration && steroidTimer <= positiveDuration + negativeDuration)
            {
                transform.localScale = new Vector3(.5f, .5f, .5f);
                attackController.attackRadius = .4f;

                isFlash = false;
                if (!isFlash)
                {
                    playerMeshRender.enabled = true;
                    currentFlashDuration = flashDuration;
                }
            }
            yield return new WaitForSeconds(negativeDuration);

            if (steroidTimer > positiveDuration + negativeDuration)
            {
                steroidTimer = 0;
                transform.localScale = new Vector3(1, 1, 1);
                attackController.attackRadius = .8f;
                state3 = State.None;
            }
        }

        IEnumerator IE_Analgesics()
        {
            analgesicsTimer += Time.deltaTime;

            if (analgesicsTimer <= positiveDuration)
            {
                isInvincible = true;
                isConfusion = false;
                hadInvincibleEffect = true;

                isFlash = true;
                if (analgesicsTimer >= positiveDuration - currentFlashDuration)
                    SetFlashEffect(isFlash);
            }
            yield return new WaitForSeconds(positiveDuration);

            if (analgesicsTimer > positiveDuration && analgesicsTimer <= positiveDuration + negativeDuration)
            {
                isInvincible = false;
                isConfusion = true;
                hadInvincibleEffect = false;

                isFlash = false;
                if (!isFlash)
                {
                    playerMeshRender.enabled = true;
                    currentFlashDuration = flashDuration;
                }
            }
            yield return new WaitForSeconds(negativeDuration);

            if (analgesicsTimer > positiveDuration + negativeDuration)
            {
                analgesicsTimer = 0;
                isInvincible = false;
                isConfusion = false;
                state4 = State.None;
            }
        }

        private void SetEffectPrefasbScale(GameObject _obj, bool _enabled, bool needVarity)
        {
            _obj.SetActive(_enabled);

            if (needVarity)
                _obj.transform.localScale = transform.localScale;
        }

        private void SetFlashEffect(bool _enabled)
        {
            if (_enabled)
            {
                playerMeshRender.enabled = !playerMeshRender.enabled;
                currentFlashDuration -= Time.fixedDeltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Diuretics")
            {
                state1 = State.Diuretics;
                diureticsTimer = 0;

                PlayPickupAudio();
            }

            if (other.gameObject.tag == "Stimulant")
            {
                state2 = State.Stimulant;
                stimulantTimer = 0;

                PlayPickupAudio();
            }

            if (other.gameObject.tag == "Steroid")
            {
                state3 = State.Steroid;
                steroidTimer = 0;

                PlayPickupAudio();
            }

            if (other.gameObject.tag == "Analgesics")
            {
                state4 = State.Analgesics;
                analgesicsTimer = 0;

                PlayPickupAudio();
            }
        }

        private void PlayPickupAudio()
        {
            pickupAudioSource.Play();
        }
    }
}
