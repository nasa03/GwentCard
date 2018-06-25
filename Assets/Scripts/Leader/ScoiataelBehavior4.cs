﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GwentCard.Play;

namespace GwentCard.Leader
{
    public class ScoiataelBehavior4 : LeaderBehaviorBase
    {
        public sealed override void Play()
        {
            int max = 0;
            for (int i = 0; i < EnemyController.GetInstance().grids[2].childCount; i++)
            {
                Transform card = EnemyController.GetInstance().grids[2].GetChild(i);
                if (!card.GetComponent<CardProperty>().gold)
                {
                    int power = card.GetComponent<CardBehavior>().totalPower;
                    if (power > max)
                        max = power;
                }
            }
            for (int i = EnemyController.GetInstance().grids[2].childCount - 1; i > +0; i--)
            {
                Transform card = EnemyController.GetInstance().grids[2].GetChild(i);
                if (card.GetComponent<CardBehavior>().totalPower == max && !card.GetComponent<CardProperty>().gold)
                    card.SetTarget(EnemyController.GetInstance().grids[5]);
            }
            base.Play();
        }

        public sealed override bool IsEnabled
        {
            get
            {
                return (int.Parse(PowerController.GetInstance().enemy_power_label[0].text) > 10 && isEnabled);
            }
        }
    }
}