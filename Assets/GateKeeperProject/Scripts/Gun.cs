using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private InputActionReference shootInput;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GunData data;
    [SerializeField] private float recoilReturnSpeed = 8f;
    [SerializeField] private TMP_Text ammoText;  
    private float recoilTarget;
    private float recoilCurrent; 
    private float nextFireTime;
    private int currentAmmo;

    void Start()
    {
        data = Instantiate(data);
        currentAmmo = data.magazineSize;
        UpdateAmmo();
    }

    void Update()
    {
        bool isShooting = data.isAutoGun ? shootInput.action.IsPressed() : shootInput.action.WasPressedThisFrame();
        UpdateRecoil(isShooting);

        if (isShooting)
        {
            TryShoot();
        }
        if(currentAmmo == 0)
        {
            currentAmmo = data.magazineSize;
            UpdateAmmo();
        }
    }

    private void TryShoot()
    {

        if (Time.time < nextFireTime) return;
        if (currentAmmo <= 0) return;

        nextFireTime = Time.time + 1f / data.fireRate;

        if (data.freeAmmoPercent <= 0f ||
            Random.Range(0f, 100f) > data.freeAmmoPercent)
        {
            currentAmmo--;
            UpdateAmmo();
        }

        for (int i = 0; i < data.pelletCount; i++)
        {
            Vector3 spreadDirection = GetSpreadDirection();

            Ray ray = new Ray(firePoint.position, spreadDirection);

            Debug.DrawRay(ray.origin, ray.direction * data.range, Color.red, 1f);

            RaycastHit[] hits = Physics.RaycastAll(ray, data.range);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            int penetrationCount = 0;
            float currentDamage = data.flatDamage;

            foreach (var hit in hits)
            {
                currentDamage *= data.damageReduction;
                penetrationCount++;

                if (penetrationCount >= data.penetration)
                    break;
            }
        }

        ApplyRecoil();
    }

    private Vector3 GetSpreadDirection()
    {
        float randomX = Random.Range(-data.spreadAngle, data.spreadAngle);
        float randomY = Random.Range(-data.spreadAngle, data.spreadAngle);

        Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0f);

        return spreadRotation * firePoint.forward;
    }

    private void UpdateRecoil(bool isShooting)
    {
        recoilCurrent = Mathf.Lerp(
            recoilCurrent,
            recoilTarget,
            15f * Time.deltaTime
        );

        if (!isShooting)
        {
            recoilTarget = Mathf.Lerp(
                recoilTarget,
                0f,
                recoilReturnSpeed * Time.deltaTime
            );
        }

        transform.localRotation = Quaternion.Euler(-recoilCurrent, 0f, 0f);
    }

    private void ApplyRecoil()
    {
        float finalRecoil = data.recoil - data.recoilReduction;

        recoilTarget += finalRecoil;

        if(recoilTarget > data.maximumRecoil)
        {
            recoilTarget = data.maximumRecoil;
        }
    }

    private void UpdateAmmo()
    {
        ammoText.text = currentAmmo.ToString() + " / " + data.magazineSize.ToString();
    }
}