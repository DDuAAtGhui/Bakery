using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

//UnityEngine.Touch 와의 네임스페이스 충돌 방지
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    //조이스틱 사이즈 지정
    [SerializeField] Vector2 joystickSize = new Vector2(500, 500);
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] NavMeshAgent player;

    Finger movementFinger;
    Vector2 movementAmount;

    // 공식 문서에 이렇게 쓰라고 지정되어있음
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnFingerDown;
        ETouch.Touch.onFingerUp += OnFingerUp;
        ETouch.Touch.onFingerMove += OnFingerMove;
    }

    //플레이어 비활성화시 터치 해제
    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.Touch.onFingerUp -= OnFingerUp;
        ETouch.Touch.onFingerMove -= OnFingerMove;
        EnhancedTouchSupport.Disable();
    }
    private void OnFingerDown(Finger finger)
    {
        //터치 안하고 있을때 터치시에만 조이스틱 따라가게
        //터치 된 손가락이 화면 중심 기준으로 왼쪽에 있는지 체크
        if (movementFinger == null /* &&
            finger.screenPosition.x <= Screen.width / 2f */)
        {
            movementFinger = finger;
            movementAmount = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.rectTransform.sizeDelta = joystickSize;
            joystick.rectTransform.anchoredPosition = ClampStartPosition(finger.screenPosition);
        }
    }

    //화면 끝쪽에 터치하면 나머지부분이 화면에 의해서 잘리니까 원하는 이동 안됨
    //그래서 조이스틱을 화면 밖으로는 벗어나지 못하게 설정
    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joystickSize.x / 2)
        {
            startPosition.x = joystickSize.x / 2;
        }

        //조이스틱의 원이 우측 X축 화면과 접하는 위치보다 
        //터치 위치가 더 우측일경우
        else if (startPosition.x > Screen.width - joystickSize.x / 2)
        {
            startPosition.x = Screen.width - joystickSize.x / 2;
        }

        if (startPosition.y < joystickSize.y / 2)
        {
            startPosition.y = joystickSize.y / 2;
        }

        //조이스틱의 원이 위쪽 화면과 접하는 위치보다
        //터치 위치가 더 높을경우
        else if (startPosition.y > Screen.height - joystickSize.y / 2)
        {
            startPosition.y = Screen.height - joystickSize.y / 2;
        }

        return startPosition;
    }
    private void OnFingerMove(Finger finger)
    {
        if (finger == movementFinger)
        {
            Vector2 knobPosition;

            //조이스틱 밖으로 튀어나가지 않게
            float maxMovement = joystickSize.x / 2f;

            //currentTouch 변수로 줄이기
            ETouch.Touch currentTouch = finger.currentTouch;

            //현재 터치중인 지점이 조이스틱 이동한계값 밖이면
            if (Vector2.Distance(
                currentTouch.screenPosition, joystick.rectTransform.anchoredPosition)
                > maxMovement)
            {
                //조이스틱이 터치 지점 바라보는 방향으로 벡터 정규화 후
                //maxMovement 곱해줘서 조이스틱 한계선에서 놀게
                knobPosition = (currentTouch.screenPosition
                    - joystick.rectTransform.anchoredPosition).normalized
                    * maxMovement;
            }

            //그 외엔 그냥 이동
            else
            {
                knobPosition = currentTouch.screenPosition
                    - joystick.rectTransform.anchoredPosition;
            }

            joystick.knob.anchoredPosition = knobPosition;
            movementAmount = knobPosition / maxMovement;
        }
    }

    private void OnFingerUp(Finger finger)
    {
        movementFinger = null;
        joystick.knob.anchoredPosition = Vector2.zero;
        joystick.gameObject.SetActive(false);
        movementAmount = Vector2.zero;
    }

    private void Update()
    {
        Vector3 playerMovement = player.speed * Time.deltaTime *
            new Vector3(movementAmount.x, 0, movementAmount.y);

        transform.LookAt(transform.position + playerMovement, Vector3.up);
        player.Move(playerMovement);
    }

    public Vector2 MovementAmount { get { return movementAmount; } }
}
