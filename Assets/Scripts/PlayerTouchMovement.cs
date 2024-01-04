using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

//UnityEngine.Touch ���� ���ӽ����̽� �浹 ����
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    //���̽�ƽ ������ ����
    [SerializeField] Vector2 joystickSize = new Vector2(500, 500);
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] NavMeshAgent player;

    Finger movementFinger;
    Vector2 movementAmount;

    // ���� ������ �̷��� ����� �����Ǿ�����
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnFingerDown;
        ETouch.Touch.onFingerUp += OnFingerUp;
        ETouch.Touch.onFingerMove += OnFingerMove;
    }

    //�÷��̾� ��Ȱ��ȭ�� ��ġ ����
    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.Touch.onFingerUp -= OnFingerUp;
        ETouch.Touch.onFingerMove -= OnFingerMove;
        EnhancedTouchSupport.Disable();
    }
    private void OnFingerDown(Finger finger)
    {
        //��ġ ���ϰ� ������ ��ġ�ÿ��� ���̽�ƽ ���󰡰�
        //��ġ �� �հ����� ȭ�� �߽� �������� ���ʿ� �ִ��� üũ
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

    //ȭ�� ���ʿ� ��ġ�ϸ� �������κ��� ȭ�鿡 ���ؼ� �߸��ϱ� ���ϴ� �̵� �ȵ�
    //�׷��� ���̽�ƽ�� ȭ�� �����δ� ����� ���ϰ� ����
    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joystickSize.x / 2)
        {
            startPosition.x = joystickSize.x / 2;
        }

        //���̽�ƽ�� ���� ���� X�� ȭ��� ���ϴ� ��ġ���� 
        //��ġ ��ġ�� �� �����ϰ��
        else if (startPosition.x > Screen.width - joystickSize.x / 2)
        {
            startPosition.x = Screen.width - joystickSize.x / 2;
        }

        if (startPosition.y < joystickSize.y / 2)
        {
            startPosition.y = joystickSize.y / 2;
        }

        //���̽�ƽ�� ���� ���� ȭ��� ���ϴ� ��ġ����
        //��ġ ��ġ�� �� �������
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

            //���̽�ƽ ������ Ƣ����� �ʰ�
            float maxMovement = joystickSize.x / 2f;

            //currentTouch ������ ���̱�
            ETouch.Touch currentTouch = finger.currentTouch;

            //���� ��ġ���� ������ ���̽�ƽ �̵��Ѱ谪 ���̸�
            if (Vector2.Distance(
                currentTouch.screenPosition, joystick.rectTransform.anchoredPosition)
                > maxMovement)
            {
                //���̽�ƽ�� ��ġ ���� �ٶ󺸴� �������� ���� ����ȭ ��
                //maxMovement �����༭ ���̽�ƽ �Ѱ輱���� ���
                knobPosition = (currentTouch.screenPosition
                    - joystick.rectTransform.anchoredPosition).normalized
                    * maxMovement;
            }

            //�� �ܿ� �׳� �̵�
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
