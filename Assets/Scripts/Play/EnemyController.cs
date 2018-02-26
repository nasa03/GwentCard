﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class EnemyController : MonoBehaviour {
    public static EnemyController instance;
    public GameObject enemy;
    public UISprite avatar_group;
    public UILabel group_label;
    public UISprite deck_realms;
    public UILabel number_label;
    public UILabel deck_realms_label;
    public GameObject cardPerfab;
    public Transform[] grids;
    Constants.Group group;

    private void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        enemy.SetActive(true);
        int random = Random.Range(0, 4);
        UIAtlas totalAtlas = new UIAtlas();
        switch (random)
        {
            case 0:
                group = Constants.Group.northern;
                avatar_group.spriteName = "player_faction_northern_realms";
                group_label.text = "北方领域";
                deck_realms.spriteName = "board_deck_northern_realms";
                totalAtlas = GameController.instance.atlas[0];
                break;
            case 1:
                group = Constants.Group.nilfgaardian;
                avatar_group.spriteName = "player_faction_northern_nilfgaard";
                group_label.text = "尼弗迦德";
                deck_realms.spriteName = "board_deck_nilfgaard";
                totalAtlas = GameController.instance.atlas[1];
                break;
            case 2:
                group = Constants.Group.monster;
                avatar_group.spriteName = "player_faction_northern_no_mans_land";
                group_label.text = "怪兽";
                deck_realms.spriteName = "board_deck_no_mans_land";
                totalAtlas = GameController.instance.atlas[2];
                break;
            case 3:
                group = Constants.Group.scoiatael;
                avatar_group.spriteName = "player_faction_scoiatael";
                group_label.text = "松鼠党";
                deck_realms.spriteName = "board_deck_scoiatael";
                totalAtlas = GameController.instance.atlas[3];
                break;
        }

        XmlDocument xml = new XmlDocument();
        xml.Load(Constants.enemyPath);
        XmlElement root = xml.DocumentElement;
        XmlNode xmlNode = root.SelectSingleNode(string.Format("/root/{0}", group));

        int name = 0;
        XmlNodeList special = xmlNode.SelectSingleNode("special").ChildNodes;
        foreach (XmlNode cardNode in special)
        {
            for (int i = 0; i < int.Parse(cardNode.Attributes["max"].Value); i++)
            {
                GameObject cardObject = Instantiate(cardPerfab, grids[0]);
                cardObject.name = name.ToString();
                name++;
                UISprite cardSprite = cardObject.GetComponent<UISprite>();
                cardSprite.atlas = GameController.instance.atlas[4];
                cardSprite.spriteName = cardNode.Attributes["sprite"].Value;
                CardProperty cardProperty = cardObject.GetComponent<CardProperty>();
                cardProperty.line = (Constants.Line)System.Enum.Parse(typeof(Constants.Line), cardNode.Attributes["line"].Value);
                cardProperty.effect = (Constants.Effect)System.Enum.Parse(typeof(Constants.Effect), cardNode.Attributes["effect"].Value);
                cardProperty.gold = bool.Parse(cardNode.Attributes["gold"].Value);
                cardProperty.power = int.Parse(cardNode.Attributes["power"].Value);
            }
        }

        XmlNodeList monster = xmlNode.SelectSingleNode("monster").ChildNodes;
        foreach (XmlNode cardNode in monster)
        {
            for (int i = 0; i < int.Parse(cardNode.Attributes["max"].Value); i++)
            {
                GameObject cardObject = Instantiate(cardPerfab, grids[0]);
                cardObject.name = name.ToString();
                name++;
                UISprite cardSprite = cardObject.GetComponent<UISprite>();
                cardSprite.atlas = totalAtlas;
                cardSprite.spriteName = cardNode.Attributes["sprite"].Value;
                CardProperty cardProperty = cardObject.GetComponent<CardProperty>();
                cardProperty.line = (Constants.Line)System.Enum.Parse(typeof(Constants.Line), cardNode.Attributes["line"].Value);
                cardProperty.effect = (Constants.Effect)System.Enum.Parse(typeof(Constants.Effect), cardNode.Attributes["effect"].Value);
                cardProperty.gold = bool.Parse(cardNode.Attributes["gold"].Value);
                cardProperty.power = int.Parse(cardNode.Attributes["power"].Value);
            }
        }

        XmlNodeList neutral = xmlNode.SelectSingleNode("neutral").ChildNodes;
        foreach (XmlNode cardNode in neutral)
        {
            for (int i = 0; i < int.Parse(cardNode.Attributes["max"].Value); i++)
            {
                GameObject cardObject = Instantiate(cardPerfab, grids[0]);
                cardObject.name = name.ToString();
                name++;
                UISprite cardSprite = cardObject.GetComponent<UISprite>();
                cardSprite.atlas = GameController.instance.atlas[4];
                cardSprite.spriteName = cardNode.Attributes["sprite"].Value;
                CardProperty cardProperty = cardObject.GetComponent<CardProperty>();
                cardProperty.line = (Constants.Line)System.Enum.Parse(typeof(Constants.Line), cardNode.Attributes["line"].Value);
                cardProperty.effect = (Constants.Effect)System.Enum.Parse(typeof(Constants.Effect), cardNode.Attributes["effect"].Value);
                cardProperty.gold = bool.Parse(cardNode.Attributes["gold"].Value);
                cardProperty.power = int.Parse(cardNode.Attributes["power"].Value);
            }
        }

        DrawCards(10);
        Number();
    }

    void DrawCards(int index)
    {
        for (int i = 0; i < index; i++)
        {
            int random = Random.Range(0, grids[0].childCount);
            grids[0].SetParent(random, grids[1]);
        }
    }

    void Number()
    {
        number_label.text = grids[1].childCount.ToString();
        deck_realms_label.text = grids[0].childCount.ToString();
    }

    public void Play(Transform grid)
    {
        if (grid.childCount == 0) return;
        int random = Random.Range(0, grid.childCount);
        CardProperty cardProperty = grid.GetChild(random).GetComponent<CardProperty>();

        switch (cardProperty.effect)
        {
            case Constants.Effect.spy:
                switch (cardProperty.line)
                {
                    case Constants.Line.melee:
                        grid.SetParent(random, PlayerController.instance.grids[2]);
                        break;
                    case Constants.Line.ranged:
                        grid.SetParent(random, PlayerController.instance.grids[3]);
                        break;
                    case Constants.Line.siege:
                        grid.SetParent(random, PlayerController.instance.grids[4]);
                        break;
                }
                DrawCards(2);
                break;
            case Constants.Effect.clear_sky:
                grid.SetParent(random, grids[5]);
                WeatherController.instance.ClearSky();
                break;
            case Constants.Effect.frost:
                if (!WeatherController.instance.frost)
                {
                    grid.SetParent(random, WeatherController.instance.grid);
                    WeatherController.instance.Frost();
                }
                break;
            case Constants.Effect.fog:
                if (!WeatherController.instance.fog)
                {
                    grid.SetParent(random, WeatherController.instance.grid);
                    WeatherController.instance.Fog();
                }
                break;
            case Constants.Effect.rain:
                if (!WeatherController.instance.fog)
                {
                    grid.SetParent(random, WeatherController.instance.grid);
                    WeatherController.instance.Rain();
                }
                break;
            default:
                switch (cardProperty.line)
                {
                    case Constants.Line.melee:
                        grid.SetParent(random, grids[2]);
                        break;
                    case Constants.Line.ranged:
                        grid.SetParent(random, grids[3]);
                        break;
                    case Constants.Line.siege:
                        grid.SetParent(random, grids[4]);
                        break;
                }
                break;
        }

        if (cardProperty.effect == Constants.Effect.nurse) Play(grids[5]);
        Number();
        PowerNumberController.instance.Number();
    }
}
