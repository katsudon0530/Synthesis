using System.Collections;
using UnityEngine;

public class EnemyShaker : MonoBehaviour
{
    public float duration = 0.5f;   // 揺れる時間
    public float magnitude = 0.1f;  // 揺れの強さ

    public void Shake()
    {
        if(this.gameObject.activeSelf)
            StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
