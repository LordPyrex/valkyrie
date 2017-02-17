using UnityEngine;
using System.Collections;

// Window with Investigator attack information
public class InvestigatorAttack {
    // The monster that raises this dialog
    public Quest.Monster monster;
    public List<AttackData> attacks;
    public Hashset<string> attackType;

    // Create an activation window, if master is false then it is for minions
    public InvestigatorAttack(Quest.Monster m)
    {
        monster = m;
        Game game = Game.Get();
        attacks = new List<AttackData>();
        attackType = new List<string>();
        foreach (AttackData ad in game.cd.investigatorAttacks)
        {
            if (m.monsterData.ContainsTrait(ad.target))
            {
                attacks.Add(ad);
                attackType.Add(ad.attackType);
            }
        }
        AttackOptions();
    }

    public void AttackOptions()
    {
        // If a dialog window is open we force it closed (this shouldn't happen)
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("dialog"))
            Object.Destroy(go);

        DialogBox db = new DialogBox(new Vector2(GetVCenter(-15f), 0.5f), new Vector2(UIScaler.GetWidthUnits() - 30, 2), "Select Attack Type");
        db.textObj.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetMediumFont();
        db.AddBorder();

        foreach (string type in attackType)
        {
            offset += 2.5;
            string tmpType = type;
            // Make first character upper case
            string nameType = type[0].ToUpper() + type.Substring(1);
            new TextButton(new Vector2(GetVCenter(-8f), offset), new Vector2(UIScaler.GetWidthUnits() - 16, 2), nameType, delegate { Attack(tmpType); });
        }

        new TextButton(new Vector2(GetVCenter(-6f), offset + 2.5), new Vector2(UIScaler.GetWidthUnits() - 12, 2), "Cancel", delegate { Destroyer.Dialog(); });
    }

    public void Attack(string type)
    {
        List<AttackData> validAttacks = new List<AttackData>();
        foreach (AttackData ad in attacks)
        {
            if (ad.attackType.equals(type))
            {
                validAttacks.Add(ad)
            }
        }
        AttackData attack = validAttacks[Random.Range(0, validAttacks.Count)];

        string text = attack.text.Replace("{0}", m.monsterData.name);
        DialogBox db = new DialogBox(new Vector2(10, 0.5f), new Vector2(UIScaler.GetWidthUnits() - 20, 8), text);
        db.AddBorder();
    }
}
