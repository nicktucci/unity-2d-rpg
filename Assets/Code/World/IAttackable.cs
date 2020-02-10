using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public interface IAttackable
    {
    bool IsValidTarget { get;  }
    void RecieveAttack(int damage, Unit attacker);
    }
