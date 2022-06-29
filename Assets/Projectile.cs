using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public float Speed { get => speed; set => speed = value; }

    float speed = 100;
    float damage = 1;
    float lifetime = 3;
    float skinWidth =.1f;

    void Start() {
        Destroy(gameObject, lifetime);

        Collider[] initialCollision = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if(initialCollision.Length > 0) {
            OnHitObject(initialCollision[0]);
        }
    }

    void Update() {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hitInfo);
        }
    }

    void OnHitObject(RaycastHit hitInfo) {
        IDamageable damageableObject = hitInfo.collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeHit(damage, hitInfo);
        }
        Destroy(gameObject);
    }

    void OnHitObject(Collider collider) {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
