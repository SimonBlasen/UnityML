using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMinigun : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject prefabPartVisible;
    [SerializeField]
    private GameObject shootingParticlesys;
    [SerializeField]
    private GameObject capsulesParticlesys;
    [SerializeField]
    private GameObject empty;

    [SerializeField]
    private float health;

    [Header("Settings")]
    [SerializeField]
    private float attackSpeed = 0.1f;
    [SerializeField]
    private float rotationAcceleration = 100f;
    [SerializeField]
    private float maxRotationSpeed = 100f;
    [SerializeField]
    private float rotationSlowdownFactor = 0.5f;
    [SerializeField]
    private float damage = 1f;

    private GameObject instEmpty = null;

    private float maxHealth;
    private Vector3 rotateAroundAxe;
    public Vector3 relativeBulletSpawnPos = Vector3.zero;
    public Vector3 relativeCapsuleSpawnPos = Vector3.zero;

    private float rotationSpeed;
    private float attackCounter = 0f;

    private GameObject instPart;
    private GameObject instBarrels;

    private MultiplayerManager multiManager = null;

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Multiplayer Manager") != null)
        {
            multiManager = GameObject.Find("Multiplayer Manager").GetComponent<MultiplayerManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (instPart == null)
            Debug.Log("instPart");
        if (car == null)
            Debug.Log("car");
        if (instPart != null && instBarrels != null)
        {
            instBarrels.transform.Rotate(rotateAroundAxe, 1f * rotationSpeed);

            if (car.ShootingWeapon[0])
            {
                if (rotationSpeed >= maxRotationSpeed)
                {
                    attackCounter += Time.deltaTime;
                    if (attackCounter >= attackSpeed)
                    {
                        attackCounter = 0f;
                        shoot();
                    }
                }
                else
                {
                    rotationSpeed += Time.deltaTime * rotationAcceleration;
                    rotationSpeed = rotationSpeed >= maxRotationSpeed ? maxRotationSpeed : rotationSpeed;
                }
            }
            else
            {
                if (rotationSpeed > 0f)
                {
                    rotationSpeed -= Time.deltaTime * rotationAcceleration * rotationSlowdownFactor;
                    rotationSpeed = rotationSpeed <= 0f ? 0f : rotationSpeed;
                }
            }
        }

    }

    private void shoot()
    {
        GameObject particles = Instantiate(shootingParticlesys, transform);
        particles.transform.localPosition = relativeBulletSpawnPos;
        particles.transform.parent = null;
        //particles.GetComponent<ParticleShootingMinigun>().velocity = car.GetComponent<Rigidbody>().velocity;
        if (car.GetComponent<Rigidbody>() != null)
        {
            particles.GetComponent<ParticleShootingMinigun>().velocity = car.GetComponent<Rigidbody>().velocity;
        }
        else
        {
            particles.GetComponent<ParticleShootingMinigun>().velocity = car.GetComponent<OtherCar>().Velocity;
        }

        GameObject particlesCapsule = Instantiate(capsulesParticlesys, transform);
        particlesCapsule.transform.localPosition = relativeCapsuleSpawnPos;
        particlesCapsule.transform.parent = null;
        //particlesCapsule.GetComponent<ParticleShootingMinigun>().velocity = car.GetComponent<Rigidbody>().velocity;
        if (car.GetComponent<Rigidbody>() != null)
        {
            particlesCapsule.GetComponent<ParticleShootingMinigun>().velocity = car.GetComponent<Rigidbody>().velocity;
        }
        else
        {
            particlesCapsule.GetComponent<ParticleShootingMinigun>().velocity = car.GetComponent<OtherCar>().Velocity;
        }

        if (car.GetType() == typeof(TestCar))
        {
            Ray rayShoot = new Ray(instEmpty.transform.position + car.transform.forward * 0.1f, car.transform.forward);
            Debug.Log("Ray: " + instEmpty.transform.position.ToString() + " Dir: " + car.transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(rayShoot, 2000f);
            Debug.DrawRay(rayShoot.origin, rayShoot.direction * 100f, Color.blue, 1f);

            Damageable hitDamageable = null;
            float nearest = float.MaxValue;
            int nearestIndex = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance < nearest && hits[i].transform.gameObject.GetComponent<HasDamageable>() != null)
                {
                    hitDamageable = hits[i].transform.gameObject.GetComponent<HasDamageable>().damageable;
                    nearest = hits[i].distance;
                    nearestIndex = i;
                }
                else if (hits[i].distance < nearest && hits[i].transform.gameObject.GetComponentInParent<HasDamageable>() != null)
                {
                    hitDamageable = hits[i].transform.gameObject.GetComponentInParent<HasDamageable>().damageable;
                    nearest = hits[i].distance;
                    nearestIndex = i;
                }
            }

            //particles.GetComponent<ParticleShootingMinigun>().destroyTime = nearest / 600.0f;

            if (hitDamageable != null)
            {
                //hitDamageable.Damage(damage);

                DrivingCar drivingCar = (DrivingCar)hitDamageable;

                //if (hitDamageable.GetType() == typeof(DrivingCar))
                if (drivingCar != null)
                {

                    multiManager.Network.SendDamagePlayer(multiManager.OwnID, drivingCar.ID, 1f);
                    multiManager.Network.SendSpawnBulletImpact(multiManager.OwnID, hits[nearestIndex].point, 0);
                }
            }
        }
        
    }

    public void SetProperties(PartType type, PartDirection direction, PartRotation rotation, Vector3Int position)
    {
        instPart = (GameObject)Instantiate(prefabPartVisible);
        instPart.transform.parent = transform;
        instPart.GetComponent<PartVisible>().SetRotation(rotation);
        instPart.GetComponent<PartVisible>().SetDirection(direction);
        instPart.GetComponent<PartVisible>().SetPosition(position);
        instPart.GetComponent<PartVisible>().SetPart(PartType.PartWeaponMinigun_0);

        instBarrels = (GameObject)Instantiate(prefabPartVisible);
        instBarrels.transform.parent = transform;
        instBarrels.GetComponent<PartVisible>().SetRotation(rotation);
        instBarrels.GetComponent<PartVisible>().SetDirection(direction);
        instBarrels.GetComponent<PartVisible>().SetPosition(position);
        instBarrels.GetComponent<PartVisible>().SetPart(PartType.PartWeaponMinigun_1);

        relativeBulletSpawnPos = position.ToVector3() + new Vector3(0f, 0f, 6.5f);
        relativeCapsuleSpawnPos = position.ToVector3() + new Vector3(1.5f, 1.5f, 2f);

        rotateAroundAxe = direction.ToVector3();

        instEmpty = Instantiate(empty);
        instEmpty.transform.parent = transform;
        instEmpty.transform.localPosition = relativeBulletSpawnPos;
    }

    private DrivingCar car;

    public DrivingCar CarReference
    {
        get
        {
            return car;
        }
        set
        {
            car = value;
            gameObject.transform.parent = car.gameObject.transform;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
}
