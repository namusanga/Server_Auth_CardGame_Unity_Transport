using UnityEngine;
using System.Collections;

public class TestingCode : MonoBehaviour
{
    testValue value = new testValue();

    // Use this for initialization
    void Start()
    {
        ChangeVaue(value);
        print(value.value);
    }

    void ChangeVaue(testValue value)
    {
        value.value = 1;
    }
}

public class testValue
{
    public int value = 0;

    public void Increase()
    {
        value += 1;
    }
}
