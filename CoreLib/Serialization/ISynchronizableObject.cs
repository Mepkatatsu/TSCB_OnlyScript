using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISynchronizableObject
{
    public void Synchronize(ISynchronizer synchronizer);
}
