﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour {
    [HideInInspector] public int totalPower;
    CardProperty cardProperty;

    private void Awake()
    {
        cardProperty = GetComponent<CardProperty>();
    }

    public void Play()
    {
        Transform grid = ShowCards.instance.totalGrid;
        int index = 0;
        for (int i = 0; i < grid.childCount; i++)
            if (ShowCards.instance.grid.GetChild(i).name == name) index = i;

        switch (cardProperty.effect)
        {
            case Constants.Effect.spy:
                switch (cardProperty.line)
                {
                    case Constants.Line.melee:
                        grid.SetParent(index, EnemyController.instance.grids[2]);
                        break;
                    case Constants.Line.ranged:
                        grid.SetParent(index, EnemyController.instance.grids[3]);
                        break;
                    case Constants.Line.siege:
                        grid.SetParent(index, EnemyController.instance.grids[4]);
                        break;
                }
                PlayerController.instance.DrawCards(2);
                break;
            case Constants.Effect.clear_sky:
                WeatherController.instance.ClearSky();
                goto default;
            case Constants.Effect.frost:
                if (!WeatherController.instance.frost) WeatherController.instance.Frost();
                goto default;
            case Constants.Effect.fog:
                if (!WeatherController.instance.fog) WeatherController.instance.Fog();
                goto default;
            case Constants.Effect.rain:
                if (!WeatherController.instance.rain) WeatherController.instance.Rain();
                goto default;
            case Constants.Effect.scorch:
                if (cardProperty.effect == Constants.Effect.scorch)
                {
                    int maxPower = 0;
                    for (int i = 2; i < 5; i++)
                    {
                        for (int ii = 0; ii < PlayerController.instance.grids[i].childCount; ii++)
                        {
                            int power = PlayerController.instance.grids[i].GetChild(ii).GetComponent<CardBehavior>().totalPower;
                            if (power > maxPower) maxPower = power;
                        }
                    }
                    for (int i = 2; i < 5; i++)
                    {
                        for (int ii = 0; ii < EnemyController.instance.grids[i].childCount; ii++)
                        {
                            int power = EnemyController.instance.grids[i].GetChild(ii).GetComponent<CardBehavior>().totalPower;
                            if (power > maxPower) maxPower = power;
                        }
                    }

                    for (int i = 2; i < 5; i++)
                    {
                        for (int ii = 0; ii < PlayerController.instance.grids[i].childCount; ii++)
                        {
                            if (PlayerController.instance.grids[i].GetChild(ii).GetComponent<CardBehavior>().totalPower == maxPower)
                                PlayerController.instance.grids[i].SetParent(ii, PlayerController.instance.grids[5]);
                        }
                    }
                    for (int i = 2; i < 5; i++)
                    {
                        for (int ii = 0; ii < EnemyController.instance.grids[i].childCount; ii++)
                        {
                            if (EnemyController.instance.grids[i].GetChild(ii).GetComponent<CardBehavior>().totalPower == maxPower)
                                EnemyController.instance.grids[i].SetParent(ii, EnemyController.instance.grids[5]);
                        }
                    }
                }
                goto default;
            default:
                switch (cardProperty.line)
                {
                    case Constants.Line.melee:
                        grid.SetParent(index, PlayerController.instance.grids[2]);
                        break;
                    case Constants.Line.ranged:
                        grid.SetParent(index, PlayerController.instance.grids[3]);
                        break;
                    case Constants.Line.siege:
                        grid.SetParent(index, PlayerController.instance.grids[4]);
                        break;
                    case Constants.Line.empty:
                        grid.SetParent(index, PlayerController.instance.grids[5]);
                        break;
                }
                break;
        }

        ShowCards.instance.Hide();
        if (cardProperty.effect == Constants.Effect.nurse) ShowCards.instance.Show(ShowCards.Behaviour.draw, PlayerController.instance.grids[5]);

        PlayerController.instance.Number();
        PowerController.instance.Number();

        EnemyController.instance.Play(EnemyController.instance.grids[0]);
    }
}
