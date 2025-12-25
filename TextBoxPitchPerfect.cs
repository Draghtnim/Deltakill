using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.Windows;


// Token: 0x0200027B RID: 635
public class TextBoxPitchPerfect : MonoBehaviour
{
    // Token: 0x06000DF7 RID: 3575 RVA: 0x00068F08 File Offset: 0x00067108
    private void Start()
    {
        this.txt = base.GetComponent<UnityEngine.UI.Text>();
        this.fullString = this.txt.text;
        this.txt.text = "";
        this.aud = base.GetComponent<AudioSource>();
        this.origPitch = this.aud.pitch;
        base.StartCoroutine(this.TextAppear());
    }

    // Token: 0x06000DF8 RID: 3576 RVA: 0x00068F6C File Offset: 0x0006716C
    private void OnDisable()
    {
        this.restart = true;
    }

    // Token: 0x06000DF9 RID: 3577 RVA: 0x00068F75 File Offset: 0x00067175
    private void OnEnable()
    {
        if (this.restart)
        {
            this.restart = false;
            base.StartCoroutine(this.TextAppear());
        }
    }

    // Token: 0x06000DFA RID: 3578 RVA: 0x00068F94 File Offset: 0x00067194
    private void Update()
    {
        if (MonoSingleton<OptionsManager>.Instance.paused)
        {
            return;
        }
        if (this.waitingForInput)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0) || MonoSingleton<InputManager>.Instance == null || (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame) || UnityEngine.Input.GetKey(KeyCode.Space) || MonoSingleton<InputManager>.Instance.InputSource.Dodge.IsPressed || (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
            {
                this.waitingForInput = false;
                return;
            }
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0) || MonoSingleton<InputManager>.Instance == null || (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame) || UnityEngine.Input.GetKey(KeyCode.Space) || MonoSingleton<InputManager>.Instance.InputSource.Dodge.IsPressed || (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
        {
            this.skipToInput = true;
        }
    }

    // Token: 0x06000DFB RID: 3579 RVA: 0x000690B2 File Offset: 0x000672B2
    private IEnumerator TextAppear()
    {
        int i = this.fullString.Length;
        if (this.preTextText)
        {
            this.preTextText.text = this.preText;
        }
        int num;
        for (int j = 0; j < i; j = num + 1)
        {
            char c = this.fullString[j];
            float waitTime = 0.05f;
            bool playSound = true;
            if (MonoSingleton<OptionsManager>.Instance.paused)
            {
                yield return new WaitUntil(() => MonoSingleton<OptionsManager>.Instance == null || !MonoSingleton<OptionsManager>.Instance.paused);
            }
            char c2 = c;
            if (c2 != ' ')
            {
                if (c2 != '}')
                {
                    if (c2 == '¼')
                    {
                        this.sb = new StringBuilder(this.fullString);
                        this.sb[j] = ' ';
                        this.fullString = this.sb.ToString();
                        this.txt.text = this.fullString.Substring(0, j);
                        this.tempString = this.txt.text;
                        this.skipToInput = false;
                        this.waitingForInput = true;
                        base.StartCoroutine(this.DotAppear());
                        yield return new WaitUntil(() => !this.waitingForInput || !base.gameObject.scene.isLoaded);
                    }
                    else
                    {
                        this.txt.text = this.fullString.Substring(0, j);
                    }
                }
                else
                {
                    this.sb = new StringBuilder(this.fullString);
                    this.sb[j] = ' ';
                    this.fullString = this.sb.ToString();
                    playSound = false;
                    waitTime = 0f;
                    this.txt.text = this.fullString.Substring(0, j);
                    UnityEvent unityEvent = this.onTextEvent;
                    if (unityEvent != null)
                    {
                        unityEvent.Invoke();
                    }
                }
            }
            else
            {
                waitTime = 0f;
                this.txt.text = this.fullString.Substring(0, j);
            }
            i = this.fullString.Length;
            if (waitTime != 0f && playSound)
            {
                //this.aud.pitch = UnityEngine.Random.Range(this.origPitch - 0.05f, this.origPitch + 0.05f);
                this.aud.Play();
            }
            if (this.skipToInput)
            {
                waitTime = 0f;
            }
            yield return new WaitForSecondsRealtime(waitTime);
            num = j;
        }
        UnityEvent unityEvent2 = this.onComplete;
        if (unityEvent2 != null)
        {
            unityEvent2.Invoke();
        }
        yield break;
    }

    // Token: 0x06000DFC RID: 3580 RVA: 0x000690C1 File Offset: 0x000672C1
    private IEnumerator DotAppear()
    {
        while (this.waitingForInput)
        {
            if (MonoSingleton<OptionsManager>.Instance.paused)
            {
                yield return new WaitUntil(() => !MonoSingleton<OptionsManager>.Instance.paused || !base.gameObject.scene.isLoaded);
            }
            this.txt.text = this.tempString + "<color=black>¼</color>";
            yield return new WaitForSecondsRealtime(0.25f);
            if (this.waitingForInput)
            {
                this.txt.text = this.tempString + "¼";
                yield return new WaitForSecondsRealtime(0.25f);
            }
        }
        yield break;
    }

    // Token: 0x04001285 RID: 4741
    private UnityEngine.UI.Text txt;

    // Token: 0x04001286 RID: 4742
    private string fullString;

    // Token: 0x04001287 RID: 4743
    private string tempString;

    // Token: 0x04001288 RID: 4744
    private StringBuilder sb;

    // Token: 0x04001289 RID: 4745
    private AudioSource aud;

    // Token: 0x0400128A RID: 4746
    private float origPitch;

    // Token: 0x0400128B RID: 4747
    private bool waitingForInput;

    // Token: 0x0400128C RID: 4748
    private bool skipToInput;

    // Token: 0x0400128D RID: 4749
    public UnityEvent onTextEvent;

    // Token: 0x0400128E RID: 4750
    public UnityEvent onComplete;

    // Token: 0x0400128F RID: 4751
    public string preText;

    // Token: 0x04001290 RID: 4752
    [SerializeField]
    private UnityEngine.UI.Text preTextText;

    // Token: 0x04001291 RID: 4753
    private bool restart;
}
