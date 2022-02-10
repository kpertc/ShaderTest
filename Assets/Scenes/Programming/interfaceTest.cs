using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interfaceTest : MonoBehaviour, IMyInterfaceII
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }


    //void testInterface()

    IMyInterface myInterface;

    //myInterface;
}

public interface IMyInterface : IMyInterfaceII
{
    void test();
}

public interface IMyInterfaceII
{

}

public class myClass : IMyInterface // the class have to include void test();
{
    public void test()
    {

    }
}