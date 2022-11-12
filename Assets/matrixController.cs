using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class matrixController : MonoBehaviour
{
    int myNum;
    Transform myParent;
    void Start()
    {
        myNum = int.Parse(transform.name.Substring(1));
        myParent = gameObject.transform.parent;
    }
    void Update()
    {
        if(myParent.Find("m" + myNum).gameObject.GetComponent<TMP_InputField>().isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
            {
                if (myNum == 0)
                    myParent.Find("m" + (myParent.childCount - 1)).gameObject.GetComponent<TMP_InputField>().Select();
                else
                    myParent.Find("m" + (myNum - 1)).gameObject.GetComponent<TMP_InputField>().Select();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (myNum == myParent.childCount - 1)
                    myParent.Find("m" + 0).gameObject.GetComponent<TMP_InputField>().Select();
                else
                    myParent.Find("m" + (myNum + 1)).gameObject.GetComponent<TMP_InputField>().Select();
            }
        }
    }
}
