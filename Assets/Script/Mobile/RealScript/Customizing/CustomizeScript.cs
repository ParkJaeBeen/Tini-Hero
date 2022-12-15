using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class CustomizeScript : MonoBehaviour
{
    public static CustomizeScript instance;
    public List<GameObject> headList,hairList, mouthList, eyeList, hatList, headdressList, mustacheList, bodyList, WeaponList;

    // Body
    [SerializeField] private Transform charOne,charTwo, charThree;
    // Head
    [SerializeField] private Transform charOneHead, charTwoHead, charThreeHead;
    // Weapon
    [SerializeField] private Transform charOneWeap, charTwoWeap, charThreeWeap;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        headList = new List<GameObject>();
        hairList = new List<GameObject>();
        mouthList = new List<GameObject>();
        eyeList = new List<GameObject>();
        hatList = new List<GameObject>();
        headdressList = new List<GameObject>();
        mustacheList = new List<GameObject>();
        bodyList = new List<GameObject>();
        WeaponList = new List<GameObject>();
    }

    public void SelectedChar(int _num)
    {
        if (_num.Equals(1))
        {
            AddList(charOneHead, "Head0", headList);
            AddList(charOneHead, "Hair", hairList);
            AddList(charOneHead, "Mouth", mouthList);
            AddList(charOneHead, "Eye", eyeList);

            AddList(charOneHead, "AC0", headdressList);
            AddList(charOneHead, "Hat", hatList);
            AddList(charOneHead, "Mustache", mustacheList);

            AddList(charOne, "Body", bodyList);
            AddList(charOneWeap, "THS", WeaponList);
        }
        else if (_num.Equals(2))
        {
            AddList(charTwoHead, "Head0", headList);
            AddList(charTwoHead, "Hair", hairList);
            AddList(charTwoHead, "Mouth", mouthList);
            AddList(charTwoHead, "Eye", eyeList);

            AddList(charTwoHead, "AC0", headdressList);
            AddList(charTwoHead, "Hat", hatList);
            AddList(charTwoHead, "Mustache", mustacheList);

            AddList(charTwo, "Body", bodyList);
            AddList(charTwoWeap, "Bow0", WeaponList);
        }
        else if (_num.Equals(3))
        {
            AddList(charThreeHead, "Head0", headList);
            AddList(charThreeHead, "Hair", hairList);
            AddList(charThreeHead, "Mouth", mouthList);
            AddList(charThreeHead, "Eye", eyeList);

            AddList(charThreeHead, "AC0", headdressList);
            AddList(charThreeHead, "Hat", hatList);
            AddList(charThreeHead, "Mustache", mustacheList);

            AddList(charThree, "Body", bodyList);
            AddList(charThreeWeap, "Wand", WeaponList);
        }
    }

    public void AddList(Transform _char, string _part, List<GameObject> _partList)
    {
        for (int i = 0; i < _char.childCount; i++)
        {
            if (_char.GetChild(i).name.Contains(_part))
            {
                _partList.Add(_char.GetChild(i).gameObject);
            }
        }
    }



    public void ClearList()
    {
        headList.Clear();
        bodyList.Clear();
        WeaponList.Clear();
        hairList.Clear();
        mouthList.Clear();
        eyeList.Clear();
        hatList.Clear();
        headdressList.Clear();
        mustacheList.Clear();
    }


    public GameObject FindActiveObj(List<GameObject> _list)
    {
        GameObject findObj = null;
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].gameObject.activeSelf)
            {
                findObj =  _list[i].gameObject;
                return findObj;
            }
        }
        if(findObj == null)
        {
            return _list[0];
        }
        return null;
    }
    public void ChangeActiveObjFoward(List<GameObject> _list)
    {
        if (!_list.IndexOf(FindActiveObj(_list)).Equals(_list.Count-1))
        {
            _list[_list.IndexOf(FindActiveObj(_list)) + 1].SetActive(true);
            FindActiveObj(_list).SetActive(false);
        }
    }
    public void ChangeActiveObjBackward(List<GameObject> _list)
    {
        if (!_list.IndexOf(FindActiveObj(_list)).Equals(0))
        {
            GameObject tmp = _list[_list.IndexOf(FindActiveObj(_list)) - 1];
            FindActiveObj(_list).SetActive(false);
            tmp.SetActive(true);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
