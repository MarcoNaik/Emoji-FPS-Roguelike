using System;

namespace Items
{
    
    public class UpgradeItemController: ItemController
    {
        public Upgrades upgrade;

        protected override void Effect()
        {

            switch (upgrade)
            {
                case Upgrades.Critic:
                {
                    PlayerController.Instance.CriticUpgrade();
                    GrabbedText = "+ critic";
                    break;
                }
                case Upgrades.Damage:
                {
                    PlayerController.Instance.DamageUpgrade();
                    GrabbedText = "+ damage";
                    break;
                }
                case Upgrades.Defense:
                {
                    PlayerController.Instance.DefenseUpgrade();
                    GrabbedText = "+ defense";
                    break;
                }
                case Upgrades.AttackSpeed:
                {
                    PlayerController.Instance.ShootSpdUpgrade();
                    GrabbedText = "+ fire rate";
                    break;
                }
                case Upgrades.HealthReg:
                {
                    PlayerController.Instance.HealthRegUpgrade();
                    GrabbedText = "+ Health Regeneration";
                    break;
                }
                case Upgrades.HealthUp:
                {
                    PlayerController.Instance.AddMaxHpBy(10);
                    GrabbedText = "+ max health";
                    break;
                }
                case Upgrades.MoveSpeed:
                {
                    PlayerController.Instance.SpdUpgrade();
                    GrabbedText = "+ speed";
                    break;
                }
                    
            }
        }
    }

    public enum Upgrades
    { 
        HealthUp, HealthReg, Damage,Critic, AttackSpeed, MoveSpeed, Defense
    }

}


