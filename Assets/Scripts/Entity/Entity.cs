using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class Entity : MonoBehaviour
{
    [HideInInspector] public bool isDead = false;

    public SerializableDictionaryBase<string, float> hp = new SerializableDictionaryBase<string, float>
    {
        {"max", 100f},
        {"current", 0f}
    };

    public float ivFrameDuration = 0.5f;
    public List<Object> blockedDamageSources;
    
    [HideInInspector] public Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        hp["current"] = hp["max"];
    }

    public void OnHit(GameObject hitSource, float damage, float forceApplied = 0)
    {
        if (blockedDamageSources.Contains(hitSource) || isDead) {
            return;
        }

        BaseAI aiScript;
        if (transform.TryGetComponent<BaseAI>(out aiScript)) {
            aiScript.OnHit(hitSource, damage, forceApplied);
        }

        rigidBody.AddExplosionForce(forceApplied, hitSource.transform.position, 1f, 0, ForceMode.Impulse);
        TakeDamage(hitSource, damage);
        
        StartCoroutine("UnblockObject", hitSource);
    }

    public void TakeDamage(GameObject hitSource, float damage)
    {
        hp["current"] = Mathf.Max(hp["current"] - damage, 0);

        if (hp["current"] == 0 && !isDead) {
            isDead = true;

            BaseAI aiScript;
            if (transform.TryGetComponent<BaseAI>(out aiScript)) {
                aiScript.OnDeath(hitSource);
            }

            return;
        }
    }

    private IEnumerator UnblockObject(Object hitSource)
    {
        yield return new WaitForSeconds(ivFrameDuration);

        blockedDamageSources.Remove(hitSource);
    }
}