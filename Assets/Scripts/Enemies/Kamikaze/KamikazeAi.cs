using System;
using System.Collections;
using BaseClass;
using UnityEngine.PlayerLoop;

namespace Enemies.Kamikaze
{
    using UnityEngine;
    public class KamikazeAi : EnemyAi
    {
        private float distancePlayer;
        public ParticleSystem explosionParticles;
        enum State{
                Moving,
                Waiting,
                None
            }

            public override void StartMonster()
            {
                SetDestinationToPosition(player.transform.position);
                distancePlayer = 999999;
            }
    
            public override void AiIntervaleMethod()
            {
                switch (currentBehaviourStateIndex)
                { 
                    case((int)State.Moving):
                        agent.SetDestination(player.transform.position);
                        Debug.Log("Yo");
                        /*if (distancePlayer > Vector2.Distance(transform.position, player.transform.position))
                        {
                            StartCoroutine(StopRunning());
                            SetBehaviourState((int)State.Waiting);
                        }
                        else
                        {
                            distancePlayer = Vector2.Distance(transform.position, player.transform.position);
                        }*/
                        break;
                    case((int)State.Waiting):
                        agent.ResetPath();
                        agent.speed = 3;
                        agent.isStopped = true;
                        break;
                    
                    case((int)State.None):
                        
                        break;
                    default:
                        Logger($"A bad behaviour was set : [{currentBehaviourStateIndex}]");
                        break;
                }
            }

            private void OnCollisionEnter2D(Collision2D other)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<PlayerHandler>().TakeDamage(3);
                    explosionParticles.Play();
                    StartCoroutine(Explode());
                }
            }

            IEnumerator StopRunning()
            {
                yield return new WaitForSeconds(2);
                SetBehaviourState((int)State.Moving);
            }

            IEnumerator Explode()
            {
                yield return new WaitForSeconds(0.6f);
                DamageEnemy(2000);
            }
        }
    
}