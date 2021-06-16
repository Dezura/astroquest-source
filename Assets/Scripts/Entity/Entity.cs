using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using MEC;

public class Entity : MonoBehaviour
{
    [HideInInspector] public bool isDead = false;

    public SerializableDictionaryBase<string, float> health = new SerializableDictionaryBase<string, float>
    {
        {"max", 2500f},
        {"current", 0f},
        {"regenAmount", 0f},
        {"regenCooldown", 0f}
    };

    public float ivFrameDuration = 0.5f;
    public List<Object> blockedDamageSources;
    
    [HideInInspector] public bool canRegen = true;
    [HideInInspector] public Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        health["current"] = health["max"];
    }

    void Update()
    {
        if (!isDead && canRegen && health["regenAmount"] != 0) health["current"] = Mathf.Min(health["current"] + health["regenAmount"] * Time.deltaTime, health["max"]);
    }

    public void OnHit(GameObject hitSource, float damage, float forceApplied = 0, Vector3? hitPosition = null)
    {
    if (blockedDamageSources.Contains(hitSource) || isDead) return;
        StopCoroutine("RegenTimeout");
        StartCoroutine("RegenTimeout");

        if (hitPosition == null) hitPosition = hitSource.transform.position;

        BaseAI aiScript;
        if (transform.TryGetComponent<BaseAI>(out aiScript)) {
            aiScript.AfterHit(hitSource, damage, forceApplied, hitPosition);
        }

        rigidBody.AddExplosionForce(forceApplied, (Vector3) hitPosition, 1f, 0, ForceMode.Impulse);
        TakeDamage(hitSource, damage);
        
        if (health["regenAmount"] != 0 && health["regenCooldown"] != 0) Timing.RunCoroutine(UnblockObject(hitSource).CancelWith(gameObject));
    }

    public void TakeDamage(GameObject hitSource, float damage)
    {
        health["current"] = Mathf.Max(health["current"] - damage, 0);

        if (health["current"] == 0 && !isDead) {
            isDead = true;

            BaseAI aiScript;
            if (transform.TryGetComponent<BaseAI>(out aiScript)) {
                aiScript.OnDeath(hitSource);
            }

            return;
        }
    }

    public IEnumerator RegenTimeout()
    {
        canRegen = false;
        yield return new WaitForSeconds(health["regenCooldown"]);
        canRegen = true;
    }

    public IEnumerator<float> UnblockObject(Object hitSource)
    {
        yield return Timing.WaitForSeconds(ivFrameDuration);

        blockedDamageSources.Remove(hitSource);
    }
}
