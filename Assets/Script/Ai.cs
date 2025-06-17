using UnityEngine;

public class Ai : MonoBehaviour
{
    public Transform target; // 미사일의 목표물
    public float speed = 15f; // 미사일 이동 속도
    public float rotationSpeed = 10f; // 미사일 회전 속도

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("미사일의 목표물이 설정되지 않았습니다.");
            return; // 목표물이 없으면 업데이트 종료
        }

        // 목표물을 향해 회전
        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        // 회전 처리: 미사일의 forward 방향을 목표물 방향으로 맞추기
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime); // Quaternion.Slerp(): 회전하는 대상 회전할 방향으로 부드럽게, 자연스럽게 전환할 때 사용

        // 목표물을 향해 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.name!="Floor"&&(collision.transform.CompareTag("A") || collision.transform.CompareTag("B") || collision.transform.CompareTag("C")))
            Destroy(gameObject); // "Floor"가 아니면서 태그가 A, B, C 중 하나라면 오브젝트를 파괴
    }
}
