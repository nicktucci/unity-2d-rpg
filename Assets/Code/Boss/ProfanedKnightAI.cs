using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossAI
{
    public class ProfanedKnightAI : NeutralAI
    {
        private Stage stage = Stage.Stage1;
        private readonly float stage2Percent = 0.75f;
        private readonly float stage3Percent = 0.37f;

        private enum Stage
        {
            Stage1,
            Stage2,
            Stage3
        }
        protected override void StartAI()
        {
            StartCoroutine(ChangeStage(Stage.Stage1));
        }
        protected override IEnumerator AI()
        {
            while (unit.IsAlive)
            {
                if (target && target.IsAlive)
                {
                    yield return CheckStage();
                    yield return FollowAndAttack(target);
                    CycleTargets();

                }
                yield return new WaitForSeconds(.25f);
            }
        }

        private IEnumerator CheckStage()
        {
            switch (stage)
            {
                case Stage.Stage1:
                    if (unit.healthPoints.Percent <= stage2Percent) yield return ChangeStage(Stage.Stage2);

                    break;
                case Stage.Stage2:
                    if (unit.healthPoints.Percent <= stage3Percent) yield return ChangeStage(Stage.Stage3);

                break;
            }
        }

        private IEnumerator ChangeStage(Stage stage)
        {
            this.stage = stage;

            switch (stage)
            {
                case Stage.Stage1:
                    unit.AnimationScale = 2f;
                    Debug.Log("Entered stage 1");
                    break;
                case Stage.Stage2:
                    unit.AnimationScale = 1f;
                    Debug.Log("Entered stage 2");
                    break;
                case Stage.Stage3:
                    Debug.Log("Entered stage 3");
                    unit.AnimationScale = 1f;
                    events.Emit(Events.Anim.SetBool, new EventAnimBool(this, "IsEnraged", true));
                    yield return new WaitForSeconds(2.1f);
                    break;
            }
        }

        public override void ResetState()
        {
            base.ResetState();
            events.Emit(Events.Anim.SetBool, new EventAnimBool(this, "IsEnraged", false));

        }

        protected override MeleeAttackData ChooseMeleeAttackData()
        {
            switch (stage)
            {
                case Stage.Stage1:
                    return base.ChooseMeleeAttackData();
                case Stage.Stage2:
                    return base.ChooseMeleeAttackData();
                case Stage.Stage3:
                    if (UnityEngine.Random.Range(0f, 1f) > 0.6f)
                    {
                        return unit.data.specialAttacks[0];
                    }
                    else
                    {
                        return base.ChooseMeleeAttackData();
                    }
            }
            return null;
        }
    }
}