using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    
    enum State{
    
        Waiting,
        Moving,
        None
    }
    public class GunnerAi : EnemyAi
    {
        public Transform position;
        public override void AiIntervaleMethod()
        {
            base.AiIntervaleMethod();
            
            switch (currentBehaviourStateIndex)
            { 
                case((int)State.Waiting):
                    if (HasDirectLineOfSight(player.transform.position))
                    {
                        SetBehaviourState((int)State.Moving);
                    }
                    
                    break;
                case((int)State.Moving):
                    if (agent.isOnNavMesh)
                    {
                        if (agent.remainingDistance <= agent.stoppingDistance)
                        {
                            FindAndMoveToRandomPosition(transform, 15f, 7f);
                        }
                    }
                    if (HasDirectLineOfSight(player.transform.position))
                    {
                        monsterGun.isShooting = true;
                    }
                    else
                    {
                        monsterGun.isShooting = false;
                    }
                    break;
                case((int)State.None):
                
                    break;
                default:
                    Logger($"A bad behaviour was set : [{currentBehaviourStateIndex}]");
                    break;
            }
            
        }
    }
}