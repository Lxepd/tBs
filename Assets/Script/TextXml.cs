using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TextXml : MonoBehaviour
{
    List<ThrowItemData> itemDataList = new List<ThrowItemData>();
    // Start is called before the first frame update

    Text text;
    private void Start()
    {
        StartCoroutine(Load());
        text = GetComponent<Text>();
    }



    IEnumerator Load()
    {
        
        yield return null;
    }
}
