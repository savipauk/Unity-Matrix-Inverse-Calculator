using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class inverseMatrixContoller : MonoBehaviour
{
    int myNum;
    Transform myParent;

    string value = "";

    void Start()
    {
        myNum = int.Parse(transform.name.Substring(1));
        myParent = gameObject.transform.parent;
    }

    void Update()
    {
        if (myParent.Find("i" + myNum).gameObject.GetComponent<TMP_InputField>().isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
            {
                if (myNum == 0)
                    myParent.Find("i" + (myParent.childCount - 1)).gameObject.GetComponent<TMP_InputField>().Select();
                else
                    myParent.Find("i" + (myNum - 1)).gameObject.GetComponent<TMP_InputField>().Select();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (myNum == myParent.childCount - 1)
                    myParent.Find("i" + 0).gameObject.GetComponent<TMP_InputField>().Select();
                else
                    myParent.Find("i" + (myNum + 1)).gameObject.GetComponent<TMP_InputField>().Select();
            }
        }
    }

    public void keepValue()
    {
        if (!string.IsNullOrEmpty(value))
            GetComponent<TMP_InputField>().text = value;
    }

    public void keepValueHelper()
    {
        value = GetComponent<TMP_InputField>().text;
    }
}
