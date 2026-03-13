using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;
using UnityEngine.XR;
using System.Collections.Generic;
using VContainer;

public class Gun : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference shootInputLeft;
    [SerializeField] private InputActionReference shootInputRight;
    [Header("Transform")]
    [SerializeField] private Transform firePoint;
    [Header("Gun info")]
    [SerializeField] private GunData data;
    [Header("Raycast Settings")]
 
    [SerializeField] private HandType currentHandType = HandType.None;
    [Header("UI")]
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private AmmoSystem ammoSystem;
    [Header("Feel")]
    [SerializeField] private MMF_Player fireFeedbacks;
    [SerializeField] private MMF_Player reloadFeedbacks;
    private float recoilTarget;
    private float currentRecoil;
    private float nextFireTime;
    private int currentAmmo;
    private float currentReloadTime;
    private int totalAmmo;
    private float currentRecoveryTime;
    private bool isRecovery;
    
    
    
    void Start()
    {
        data = Instantiate(data);
        currentAmmo = data.magazineSize;
        UpdateAmmoUI();
    }

    void OnEnable()
    {
        ammoSystem.OnAmmoChanged += UpdateAmmo;
        UpdateAmmoUI();
    }

    void OnDisable()
    {
        ammoSystem.OnAmmoChanged -= UpdateAmmo;
    }

    void Update()
    {
        if (currentHandType == HandType.None) return;

        InputAction currentShootInputAction = currentHandType == HandType.Left ? shootInputLeft.action : shootInputRight.action;

        bool isShooting = data.isAutoGun ? currentShootInputAction.IsPressed() : currentShootInputAction.WasPressedThisFrame();

        if (isShooting)
        {
            TryShoot();
            currentRecoveryTime = 0;
            isRecovery = false;
        }
        else if (currentRecoveryTime < data.recoilRecoveryTime)
        {
            currentRecoveryTime += Time.deltaTime;
        }
        if (currentRecoveryTime >= data.recoilRecoveryTime)
        {
            isRecovery = true;
        }
        if (currentAmmo == 0)
        {
            if (currentReloadTime > data.reloadTime)
            {
                if (ammoSystem.GetAmmo(data.type) <= 0)
                {
                    ammoText.text = "No bullet";
                }
                else
                {
                    currentReloadTime = 0;
                    ammoSystem.UseAmmo(data.type, data.magazineSize);
                }
            }
            else
            {
                if (currentReloadTime == 0 && reloadFeedbacks != null)
                {
                    reloadFeedbacks?.PlayFeedbacks();
                }
                currentReloadTime += Time.deltaTime;
                ammoText.text = "Reloading";
            }
        }
        UpdateRecoil();
    }

    private void TryShoot()
    {

        if (Time.time < nextFireTime) return;
        if (currentAmmo <= 0) return;

        nextFireTime = Time.time + 1f / data.fireRate;

        if (data.freeAmmoPercent <= 0f || Random.Range(0f, 100f) > data.freeAmmoPercent)
        {
            currentAmmo--;
            UpdateAmmoUI();
        }

        for (int i = 0; i < data.pelletCount; i++)
        {
            Vector3 spreadDirection = GetSpreadDirection();

            Ray ray = new Ray(firePoint.position, spreadDirection);

            Debug.DrawRay(ray.origin, ray.direction * data.range, Color.red, 1f);

            RaycastHit[] hits = Physics.SphereCastAll(ray, data.bulletSize, data.range, data.hitLayers, QueryTriggerInteraction.Collide);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            Vector3 trailEndPoint = hits.Length > 0 ? hits[0].point : firePoint.position + spreadDirection * data.range;

            if (data.bulletTrailPrefab != null)
            {
                BulletTrailHandler.Spawn(data.bulletTrailPrefab, firePoint.position, trailEndPoint, data.bulletTrailSpeed, data.bulletSize);
            }     

            int penetrationCount = 0;
            float currentDamage = data.flatDamage;

            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("RewardCard"))
                {
                    ShootableCard card = hit.collider.GetComponent<ShootableCard>();
                    if (card != null) card.TriggerCard();
                    break; // Stop bullet penetration if it hits a card
                }
                if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("EnemyHead")) continue;
                IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
                if (damageable == null) continue;

                float finalDamage = currentDamage;

                if (hit.collider.CompareTag("EnemyHead"))
                    finalDamage *= data.headshotMultiplier;

                damageable.TakeDamage(finalDamage);

                currentDamage *= 1 - data.damagePenetrationReduction;
                penetrationCount++;

                if (penetrationCount >= data.penetration)
                    break;
            }
        }

        ApplyRecoil();
        if(fireFeedbacks != null)
        {
            fireFeedbacks.PlayFeedbacks();
        }
        TriggerHaptic();
    }

    private Vector3 GetSpreadDirection()
    {
        float spread = data.spreadAngle + currentRecoil;
        float randomX = Random.Range(-spread, spread);
        float randomY = Random.Range(-spread, spread);

        Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0f);

        return spreadRotation * firePoint.forward;
    }

    private void UpdateRecoil()
    {
        currentRecoil = Mathf.Lerp(
            currentRecoil,
            recoilTarget,
            15f * Time.deltaTime
        );

        if (isRecovery)
        {
            recoilTarget = Mathf.Lerp(
                recoilTarget,
                0f,
                data.recoilRecoverySpeed * Time.deltaTime
            );
        }

        transform.localRotation = Quaternion.Euler(-currentRecoil, 0f, 0f);
    }

    private void ApplyRecoil()
    {
        float finalRecoil = data.recoil - data.recoilReduction;

        recoilTarget += finalRecoil;

        if (recoilTarget > data.maximumRecoil)
        {
            recoilTarget = data.maximumRecoil;
        }
    }

    private void UpdateAmmo(WeaponType type, int recieve, int total)
    {
        if (type == data.type)
        {
            currentAmmo += recieve;
            totalAmmo = total;
            UpdateAmmoUI();
        }
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = currentAmmo.ToString() + " / " + totalAmmo.ToString();
    }

    public void SetCurrentHandType(HandType hand)
    {
        currentHandType = hand;
    }

    public GunData GetGunData()
    {
        return data;
    }

    private void TriggerHaptic()
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

        XRNode node = currentHandType == HandType.Left ? XRNode.LeftHand : XRNode.RightHand;

        InputDevices.GetDevicesAtXRNode(node, devices);

        if (devices.Count > 0)
        {
            devices[0].SendHapticImpulse(0, data.hapticAmplitude, data.hapticDuration);
        }
    }
}