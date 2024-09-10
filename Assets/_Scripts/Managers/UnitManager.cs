using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<BaseUnit> AllUnits;
    public List<BaseUnit> AllEnemies => AllUnits.Where(u => !u.IsAlly).ToList();
    public List<BaseUnit> AllAllies => AllUnits.Where(u => u.IsAlly).ToList();
}
