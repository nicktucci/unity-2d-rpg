using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAI
{
    public class ProfanedKnightAI : NeutralAI
    {
        private bool isEnraged = false;
        private float enragePercent = 0.5f;
        protected override IEnumerator AI()
        {
            while (unit.IsAlive)
            {
                if (target && target.IsAlive)
                {
                    yield return CheckEnrage();
                    yield return FollowAndAttack(target);
                    CycleTargets();

                }
                yield return new WaitForSeconds(.25f);
            }
        }

        private IEnumerator CheckEnrage()
        {
            if (((float)unit.healthPoints.Value / unit.healthPoints.ValueMax) < enragePercent && !isEnraged)
            {
                events.Emit(Events.Anim.SetBool, new AnimBoolEvent(this, "IsEnraged", true));
                isEnraged = true;
                yield return new WaitForSeconds(2.1f);
            }
        }

        protected override string GetAttackLabel()
        {
            string animLabel;
            if (isEnraged)
            {
                animLabel = "Attack_Enraged";
            }
            else
            {
                float rnd = UnityEngine.Random.Range(0, 1f);
                animLabel = rnd > .5f ? "Attack_1" : "Attack_2";
            }
            return animLabel;
        }

        public override void ResetState()
        {
            base.ResetState();
            isEnraged = false;
            events.Emit(Events.Anim.SetBool, new AnimBoolEvent(this, "IsEnraged", false));

        }
    }
}