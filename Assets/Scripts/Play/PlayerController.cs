﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class PlayerController : Singleton<PlayerController> {
    public Transform[] grids;
    public GameObject player;
    [SerializeField] UISprite avatar_group;
    [SerializeField] UILabel group_label;
    [SerializeField] UISprite deck_realms;
    [SerializeField] UILabel number_label;
    [SerializeField] UILabel deck_realms_label;
    Global.Group group;

    public void Initialize(string playerGroup)
    {
        player.SetActive(true);
        group = (Global.Group)System.Enum.Parse(typeof(Global.Group), playerGroup);
        UIAtlas totalAtlas = new UIAtlas();
        switch (group)
        {
            case Global.Group.northern:
                avatar_group.spriteName = "player_faction_northern_realms";
                group_label.text = "北方领域";
                deck_realms.spriteName = "board_deck_northern_realms";
                totalAtlas = GameController.GetInstance().atlas[0];
                break;
            case Global.Group.nilfgaardian:
                avatar_group.spriteName = "player_faction_northern_nilfgaard";
                group_label.text = "尼弗迦德";
                deck_realms.spriteName = "board_deck_nilfgaard";
                totalAtlas = GameController.GetInstance().atlas[1];
                break;
            case Global.Group.monster:
                avatar_group.spriteName = "player_faction_northern_no_mans_land";
                group_label.text = "怪兽";
                deck_realms.spriteName = "board_deck_no_mans_land";
                totalAtlas = GameController.GetInstance().atlas[2];
                break;
            case Global.Group.scoiatael:
                avatar_group.spriteName = "player_faction_scoiatael";
                group_label.text = "松鼠党";
                deck_realms.spriteName = "board_deck_scoiatael";
                totalAtlas = GameController.GetInstance().atlas[3];
                break;
        }

        XmlDocument xml = new XmlDocument();
        xml.Load(Global.path);
        XmlElement root = xml.DocumentElement;
        XmlNode xmlNode = root.SelectSingleNode(string.Format("/root/{0}", group));

        int name = 0;
        XmlNodeList special = xmlNode.SelectSingleNode("special").ChildNodes;
        foreach(XmlNode cardNode in special)
        {
            for (int i = 0; i < int.Parse(cardNode.Attributes["total"].Value); i++)
            {
                GameObject cardObject = Instantiate(GameController.GetInstance().cardPerfab, grids[0]);
                cardObject.name = name.ToString();
                name++;
                UISprite cardSprite = cardObject.GetComponent<UISprite>();
                cardSprite.atlas = GameController.GetInstance().atlas[4];
                cardSprite.spriteName = cardNode.Attributes["sprite"].Value;
                CardProperty cardProperty = cardObject.GetComponent<CardProperty>();
                cardProperty.line = (Global.Line)System.Enum.Parse(typeof(Global.Line), cardNode.Attributes["line"].Value);
                cardProperty.effect = (Global.Effect)System.Enum.Parse(typeof(Global.Effect), cardNode.Attributes["effect"].Value);
                cardProperty.gold = bool.Parse(cardNode.Attributes["gold"].Value);
                cardProperty.power = int.Parse(cardNode.Attributes["power"].Value);
            }
        }

        XmlNodeList monster = xmlNode.SelectSingleNode("monster").ChildNodes;
        foreach (XmlNode cardNode in monster)
        {
            for (int i = 0; i < int.Parse(cardNode.Attributes["total"].Value); i++)
            {
                GameObject cardObject = Instantiate(GameController.GetInstance().cardPerfab, grids[0]);
                cardObject.name = name.ToString();
                name++;
                UISprite cardSprite = cardObject.GetComponent<UISprite>();
                cardSprite.atlas = totalAtlas;
                cardSprite.spriteName = cardNode.Attributes["sprite"].Value;
                CardProperty cardProperty = cardObject.GetComponent<CardProperty>();
                cardProperty.line = (Global.Line)System.Enum.Parse(typeof(Global.Line), cardNode.Attributes["line"].Value);
                cardProperty.effect = (Global.Effect)System.Enum.Parse(typeof(Global.Effect), cardNode.Attributes["effect"].Value);
                cardProperty.gold = bool.Parse(cardNode.Attributes["gold"].Value);
                cardProperty.power = int.Parse(cardNode.Attributes["power"].Value);
            }
        }

        XmlNodeList neutral = xmlNode.SelectSingleNode("neutral").ChildNodes;
        foreach (XmlNode cardNode in neutral)
        {
            for (int i = 0; i < int.Parse(cardNode.Attributes["total"].Value); i++)
            {
                GameObject cardObject = Instantiate(GameController.GetInstance().cardPerfab, grids[0]);
                cardObject.name = name.ToString();
                name++;
                UISprite cardSprite = cardObject.GetComponent<UISprite>();
                cardSprite.atlas = GameController.GetInstance().atlas[4];
                cardSprite.spriteName = cardNode.Attributes["sprite"].Value;
                CardProperty cardProperty = cardObject.GetComponent<CardProperty>();
                cardProperty.line = (Global.Line)System.Enum.Parse(typeof(Global.Line), cardNode.Attributes["line"].Value);
                cardProperty.effect = (Global.Effect)System.Enum.Parse(typeof(Global.Effect), cardNode.Attributes["effect"].Value);
                cardProperty.gold = bool.Parse(cardNode.Attributes["gold"].Value);
                cardProperty.power = int.Parse(cardNode.Attributes["power"].Value);
            }
        }

        DrawCards(10);
        Number();
    }

    public void DrawCards(int index)
    {
        for (int i = 0; i < index; i++)
        {
            int random = Random.Range(0, grids[0].childCount);
            grids[0].SetParent(random, grids[1]);
        }
    }

    public void Number()
    {
        number_label.text = grids[1].childCount.ToString();
        deck_realms_label.text = grids[0].childCount.ToString();
    }

    public void EndTurn()
    {
        Number();
        PowerController.GetInstance().Number();
        EnemyController.GetInstance().Play(EnemyController.GetInstance().grids[1]);
    }
}
