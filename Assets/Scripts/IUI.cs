using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIOpened
{
    bool OnUIOpened();
}

public interface IUIClosed
{
    bool OnUIClosed();
}