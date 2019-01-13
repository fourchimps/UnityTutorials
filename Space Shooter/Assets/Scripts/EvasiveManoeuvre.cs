using System.Collections;
using UnityEngine;

[System.Serializable]
public class TimeRange {
    public float tMin, tMax;
}

public class EvasiveManoeuvre : MonoBehaviour {
    public float dodge;
    public float smoothing;
    public float roll;
    public TimeRange startWait;
    public TimeRange manoeuvreTime;
    public TimeRange manoeuvreWait;
    public Boundary boundary;

    private float currentSpeed;
    private float targetManoeuvre;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
    }

    IEnumerator Evade() {
        yield return new WaitForSeconds(Random.Range(startWait.tMin, startWait.tMax));

        while (true) {
            targetManoeuvre = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(manoeuvreTime.tMin, manoeuvreTime.tMax));
            targetManoeuvre = 0;
            yield return new WaitForSeconds(Random.Range(manoeuvreWait.tMin, manoeuvreWait.tMax));
        }
    }

    void FixedUpdate() {
        /// WHY NOT JUST CHANGE rb.velocity.x?
        float newManoeuvre = Mathf.MoveTowards(rb.velocity.x, targetManoeuvre, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManoeuvre, 0.0f, currentSpeed);

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * roll);
    }
}
