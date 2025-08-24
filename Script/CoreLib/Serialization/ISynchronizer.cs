using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISynchronizer
{
    public byte Synchronize(byte byteData);
    public short Synchronize(short shortData);
    public string Synchronize(string stringData);
}
