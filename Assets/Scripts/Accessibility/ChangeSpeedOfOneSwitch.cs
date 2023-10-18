using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeSpeedOfOneSwitch : MonoBehaviour
{
    private SpeedOfOneSwitch nextSpeed = SpeedOfOneSwitch.Fast;
    private NewOneSwitch newOneSwitch;
    [SerializeField] TMP_Text textObject;

    private void Start()
    {
        newOneSwitch = NewOneSwitch.Instance;
        
    }
    public void Click()
    {
        textObject.text = "Speed of OneSwitch:\n";
        switch (nextSpeed)
        {
            case SpeedOfOneSwitch.Slow:
                newOneSwitch.delay = 4;
                nextSpeed = SpeedOfOneSwitch.Medium;
                textObject.text += "Slow";
                break;
            case SpeedOfOneSwitch.Medium:
                newOneSwitch.delay = 2;
                nextSpeed = SpeedOfOneSwitch.Fast;
                textObject.text += "Medium";
                break;
            case SpeedOfOneSwitch.Fast:
                newOneSwitch.delay = 1;
                nextSpeed = SpeedOfOneSwitch.Slow;
                textObject.text += "Fast";
                break;
        }
    }
}
enum SpeedOfOneSwitch
{
    Slow,
    Medium,
    Fast
}
