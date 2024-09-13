using NaughtyAttributes;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int Meat;
    public int Wood;
    public int Iron;
    public int Gold;

    public bool CheckEnoughResources(TileRequirement requirements)
    {
        if (Meat >= requirements.meat && Wood >= requirements.wood && Iron >= requirements.iron && Gold >= requirements.gold)
        {
            return true;
        }

        return false;
    }

    public void RemoveResources(int meat, int wood, int iron, int gold)
    {
        Meat -= meat;
        Wood -= wood;
        Iron -= iron;
        Gold -= gold;
    }
}