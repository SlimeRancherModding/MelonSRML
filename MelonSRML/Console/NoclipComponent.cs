using Il2CppKinematicCharacterController;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppMonomiPark.SlimeRancher.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace MelonSRML.Console
{
    public class NoclipComponent : MonoBehaviour
    {
        public static T Get<T>(string name) where T : UnityEngine.Object
        {
            return Resources.FindObjectsOfTypeAll<T>().FirstOrDefault((T x) => x.name == name);
        }

        public static float baseSpeed = 15f;
        public static float speedAdjust = 235f;
        public float speed = 15f;
        public Transform player;
        public KCCSettings settings;
        private Vector2 lastMousePos;

        public void OnDestroy()
        {
            player.gameObject.GetComponent<KinematicCharacterMotor>().enabled = true;
            settings.AutoSimulation = true;
            player.GetComponent<SRCharacterController>().Position = player.position;
        }

        public void Awake()
        {
            player = Get<Transform>("PlayerControllerKCC");
            player.gameObject.GetComponent<KinematicCharacterMotor>().enabled = false;
            settings = Get<KCCSettings>("");
            settings.AutoSimulation = false;
        }

        public void Update()
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                player.position += -transform.right * (speed * Time.deltaTime);
            }
            
            if (Keyboard.current.shiftKey.isPressed)
            {
                speed = baseSpeed * 2;
            }
            else
            {
                speed = baseSpeed;
            }

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                player.position += transform.right * (speed * Time.deltaTime);
            }

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                player.position += transform.forward * (speed * Time.deltaTime);
            }

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                player.position += -transform.forward * (speed * Time.deltaTime);
            }

            if (Mouse.current.scroll.ReadValue().y > 0)
            {
                baseSpeed += (speedAdjust * Time.deltaTime);
            }
            if (Mouse.current.scroll.ReadValue().y < 0)
            {
                baseSpeed -= (speedAdjust * Time.deltaTime);
            }
            if (baseSpeed < 1)
            {
                baseSpeed = 1.01f;
            }
        }
    }
}
