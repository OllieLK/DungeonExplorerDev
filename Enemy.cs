using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class BattleMoves { }



    public abstract class BattleEntity
    {
        public int health;
        public int maxHealth;
        public List<BattleMoves> moves;


        public virtual void attack()
        {
            throw new NotImplementedException();
        }
    }

    interface iBattleEntity
    {
        void Attack();
        void Damage();
    }

    public class Enemy : BattleEntity
    {

    }

}
