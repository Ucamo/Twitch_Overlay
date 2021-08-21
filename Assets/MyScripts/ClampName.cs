using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClampName : MonoBehaviour
{
    public TextMeshProUGUI nameLabel;
    void Update()
    {
        Vector3 offSetPosition = new Vector3(this.transform.position.x,this.transform.position.y+2,this.transform.position.z);
        Vector3 namePos = Camera.main.WorldToScreenPoint(offSetPosition);
        nameLabel.transform.position = Vector3.Lerp(nameLabel.transform.position,namePos,0.5f);        
    }
}
