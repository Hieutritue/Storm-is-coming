using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [ReadOnly]
    public List<BaseUnit> AllUnits = new ();
    public List<BaseUnit> AllEnemies => AllUnits.Where(u => !u.IsAlly).ToList();
    public List<BaseUnit> AllAllies => AllUnits.Where(u => u.IsAlly).ToList();
}
