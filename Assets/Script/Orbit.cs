using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target; // 공전할 대상
    public float orbitSpeed; // 공전 속도
    Vector3 offSet; // 초기 위치와 대상 간의 거리 차이

    void Start()
    {
        offSet = transform.position - target.position; // 초기 위치와 대상 간의 거리 계산
    }

    // Update는 매 프레임마다 한 번씩 호출됩니다
    void Update()
    {
        transform.position = target.position + offSet; // 대상 위치에 offset을 더해 현재 위치 설정
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime); // RotateAround : target을 중심으로 회전
        offSet = transform.position - target.position; // 회전 후 새로운 offset 계산
    }
}
