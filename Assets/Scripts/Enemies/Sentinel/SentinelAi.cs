using System.Collections;

namespace DefaultNamespace.Sentinel
{
    using System;
    using UnityEngine;
    enum State{
            
        Waiting,
        Moving,
        None
    }
    public class SentinelAi: EnemyAi
    {
         

        private bool isWaiting;
        public override void StartMonster()
        {

            /*if (agent.isOnNavMesh)
            {

                FindAndMoveToRandomPosition(transform, 20f, 10f);
                Logger("Yep, we are!");
            }

            isWaiting = false;*/
        }

        public override void AiIntervaleMethod()
        {
            base.AiIntervaleMethod();
            
            switch (currentBehaviourStateIndex)
            { 
                case((int)State.Waiting):
                    if (HasDirectLineOfSight(player.transform.position))
                    {
                        monsterGun.isShooting = true;
                    }
                    else
                    {
                        monsterGun.isShooting = false;
                    }
                    break;
                case((int)State.Moving):
                    monsterGun.isShooting = false;
                    if (agent.isOnNavMesh && !isWaiting)
                    {
                        if (agent.remainingDistance <= agent.stoppingDistance)
                        {
                            StartCoroutine(WaitAndSwitchPositioning());
                            SetBehaviourState((int)State.Waiting);
                        }
                    }
                    break;
                case((int)State.None):
                
                    break;
                default:
                    Logger($"A bad behaviour was set : [{currentBehaviourStateIndex}]");
                    break;
            }
            
        }
        public IEnumerator WaitAndSwitchPositioning()
        {
            yield return new WaitForSeconds(5);
            FindAndMoveToRandomPosition(transform, 30f, 10f);
            isWaiting = false;
            SetBehaviourState((int)State.Moving);
        }
    }
            
    
}