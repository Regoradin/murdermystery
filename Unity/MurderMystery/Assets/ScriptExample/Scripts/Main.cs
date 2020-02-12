using UnityEngine;
using FrameSynthesis.VR;

namespace ScriptExample
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        ScriptEngine scriptEngine;
        [SerializeField]
        AudioSource gestureSound;

        void Start()
        {
            VRGestureRecognizer.Current.NodHandler += OnNod;
            VRGestureRecognizer.Current.HeadshakeHandler += OnHeadshake;
        }

        void OnNod()
        {
            if (scriptEngine.IsWaitingYesNo)
            {
                scriptEngine.AnswerYes();
                gestureSound.Play();
                Debug.Log("I called the method");
            }
        }

        void OnHeadshake()
        {
            if (scriptEngine.IsWaitingYesNo)
            {
                scriptEngine.AnswerNo();
                gestureSound.Play();
            }
        }
    }
}
