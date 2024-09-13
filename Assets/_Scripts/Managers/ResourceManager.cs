using NaughtyAttributes;
using UnityEngine;

public class ResourceManager : BaselineManager
{
    public int Meat;
    public int Wood;
    public int Iron;
    public int Gold;

    public void Start()
    {
        resourceManager = this;
    }
}