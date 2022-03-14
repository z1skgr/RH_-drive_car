using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    // Start is called before the first frame update
    Text _lbl;
    public float BlinkFadeInTime;
    public float BlinkStayTime;
    public float BlinkFadeOutTime;
    private float _timeChecker = 0;
    private Color _color;
    void Start()
    {
        _lbl = GetComponent<Text>();
        _color = _lbl.color;
    }

    // Update is called once per frame
    void Update()
    {
        //Blink text after playing/choosing button screen
        _timeChecker += Time.deltaTime;
        if (_timeChecker < BlinkFadeInTime)
        {
            _lbl.color = new Color(_color.r, _color.g, _color.b, _timeChecker / BlinkFadeInTime);
        }
        else if (_timeChecker < BlinkFadeInTime + BlinkStayTime)
        {
            _lbl.color = new Color(_color.r, _color.g, _color.b, 1);
        }
        else if (_timeChecker < BlinkFadeInTime + BlinkStayTime+BlinkFadeOutTime)
        {
            _lbl.color = new Color(_color.r, _color.g, _color.b, 1 - (_timeChecker-(BlinkFadeInTime +BlinkStayTime))/BlinkFadeOutTime);
        }
        else
        {
            _timeChecker = 0;
        }
    }
}
