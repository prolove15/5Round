using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataStorage_Gameplay
{
    public UnitCardsData unitCardsData = new UnitCardsData();
    public TakaraCardsData takaraCardsData = new TakaraCardsData();
    public ItemCardsData itemCardsData = new ItemCardsData();
    public GameEventsData gameEventsData = new GameEventsData();
}

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Unit cards data
/// </summary>
//////////////////////////////////////////////////////////////////////
#region UnitCardsData

//--------------------------------------------------
public class UnitCardsData
{
    public List<UnitCardData> unitCards = new List<UnitCardData>();
    public int playerID = -1;
}

//--------------------------------------------------
public class UnitCardData
{
    public int id;
    public string name;
    public Sprite frontSide;
    public Sprite backSide;
    public int cost;
    public List<UnitAttribute> attrib = new List<UnitAttribute>();
    public int hp;
    public int atk;
    public int agi;
    public NormalAttack nlAtk = new NormalAttack();
    public SpecialAttack1 spcAtk1 = new SpecialAttack1();
    public SpecialAttack2 spcAtk2 = new SpecialAttack2();
    public UniqueAbility uniqAbil = new UniqueAbility();
    public ShienAbility shienAbil = new ShienAbility();

    public int nlAp_Leg; // it's test variable
    public string nlAtk_Leg; // it's test variable
    public int spc1Ap_Leg; // it's test variable
    public int spc1Sp_Leg; // it's test variable
    public string spc1Atk_Leg; // it's test variable 
    public int spc2Ap_Leg; // it's test variable
    public int spc2Sp_Leg; // it's test variable
    public string spc2Atk_Leg; // it's test variable
    public string uniqAbil_Leg; // it's test variable
    public string shienName_Leg; // it's test variable
    public string shienDes_Leg; // it's test variable
}

//--------------------------------------------------
public enum UnitAttribute
{
    Null = 0,
    Yumi = 1,
    Fushi = 2,
    Ryu = 3,
    Ken = 4,
    Ma = 5,
}

//--------------------------------------------------
public class NormalAttack
{
    public int ap;
    public string title;
    public List<NormalAttackContent> contents = new List<NormalAttackContent>();
}

//--------------------------------------------------
public class NormalAttackContent
{
    public NormalAttackType type = new NormalAttackType();
    public int amount;
    public List<GameEventsTiming> tgrEvents = new List<GameEventsTiming>();
    public List<GameEventsTiming> endEvents = new List<GameEventsTiming>();
}

//--------------------------------------------------
public enum NormalAttackType
{
    Null = 0,
    Type01 = 1, Type02 = 2, Type03 = 3, Type04 = 4,
    Type05 = 5, Type06 = 6, Type07 = 7, Type08 = 8, Type09 = 9,
}

//--------------------------------------------------
public class SpecialAttack1
{
    public int id;
    public int ap;
    public int sp;
    public string dsc;
    public List<SpecialAttack1Content> contents = new List<SpecialAttack1Content>();
}

//--------------------------------------------------
public class SpecialAttack1Content
{
    public SpecialAttack1Type type = new SpecialAttack1Type();
    public int amount;
    public List<GameEventsTiming> tgrEvents = new List<GameEventsTiming>();
    public List<GameEventsTiming> endEvents = new List<GameEventsTiming>();
}

//--------------------------------------------------
public enum SpecialAttack1Type
{
    Null = 0,
    Type01 = 1, Type02 = 2, Type03 = 3, Type04 = 4,
    Type05 = 5, Type06 = 6, Type07 = 7, Type08 = 8, Type09 = 9,
    Type10 = 10, Type11 = 11, Type12 = 12, Type13 = 13, Type14 = 14,
    Type15 = 15, Type16 = 16, Type17 = 17, Type18 = 18, Type19 = 19,
}

//--------------------------------------------------
public class SpecialAttack2
{
    public int id;
    public int ap;
    public int sp;
    public string dsc;
    public List<SpecialAttack2Content> contents = new List<SpecialAttack2Content>();
}

//--------------------------------------------------
public class SpecialAttack2Content
{
    public SpecialAttack2Type type = new SpecialAttack2Type();
    public int amount;
    public List<GameEventsTiming> tgrEvents = new List<GameEventsTiming>();
    public List<GameEventsTiming> endEvents = new List<GameEventsTiming>();
}

//--------------------------------------------------
public enum SpecialAttack2Type
{
    Null = 0,
    Type01 = 1, Type02 = 2, Type03 = 3, Type04 = 4,
    Type05 = 5, Type06 = 6, Type07 = 7, Type08 = 8, Type09 = 9,
    Type10 = 10, Type11 = 11, Type12 = 12, Type13 = 13, Type14 = 14,
    Type15 = 15, Type16 = 16, Type17 = 17, Type18 = 18, Type19 = 19,
    Type20 = 20, Type21 = 21, Type22 = 22, Type23 = 23, Type24 = 24,
    Type25 = 25, Type26 = 26, Type27 = 27, Type28 = 28, Type29 = 29,
    Type30 = 30, Type31 = 31, Type32 = 32, Type33 = 33, Type34 = 34,
    Type35 = 35, Type36 = 36, Type37 = 37,
}

//--------------------------------------------------
public class UniqueAbility
{
    public string dsc;
    public List<GameEventsTiming> tgrEvents = new List<GameEventsTiming>();
    public List<GameEventsTiming> endEvents = new List<GameEventsTiming>();
    public UniqueAbilityType type = new UniqueAbilityType();
}

//--------------------------------------------------
public enum UniqueAbilityType
{
    Null = 0,
    Type01 = 1, Type02 = 2, Type03 = 3, Type04 = 4, Type05 = 5, Type06 = 6, Type07 = 7, Type08 = 8, Type09 = 9,
    Type10 = 10, Type11 = 11, Type12 = 12, Type13 = 13, Type14 = 14, Type15 = 15, Type16 = 16, Type17 = 17,
    Type18 = 18, Type19 = 19, Type20 = 20, Type21 = 21, Type22 = 22, Type23 = 23, Type24 = 24, Type25 = 25,
    Type26 = 26, Type27 = 27, Type28 = 28, Type29 = 29, Type30 = 30, Type31 = 31, Type32 = 32, Type33 = 33,
    Type34 = 34, Type35 = 35, Type36 = 36, Type37 = 37, Type38 = 38, Type39 = 39, Type40 = 40, Type41 = 41,
    Type42 = 42, Type43 = 43, Type44 = 44, Type45 = 45, Type46 = 46, Type47 = 47, Type48 = 48,
}

//--------------------------------------------------
public class ShienAbility
{
    public string dsc;
    public List<GameEventsTiming> tgrEvent = new List<GameEventsTiming>();
    public List<GameEventsTiming> endEvent = new List<GameEventsTiming>();
    public ShienAbilityType type = new ShienAbilityType();
}

//--------------------------------------------------
public enum ShienAbilityType
{
    Null = 0,
    Type01 = 1, Type02 = 2, Type03 = 3, Type04 = 4, Type05 = 5, Type06 = 6,
}

//--------------------------------------------------
public enum AttackType
{
    Null = 0, Normal = 1, Spc1 = 2, Spc2 = 3,
}

//--------------------------------------------------
public enum AbilityType
{
    Null = 0, Normal = 1, Spc1 = 2, Spc2 = 3, Uniq = 4, Shien = 5, Item = 6,
}

#endregion

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Takara cards data
/// </summary>
//////////////////////////////////////////////////////////////////////
#region TakaraCardsData

//--------------------------------------------------
public class TakaraCardsData
{
    public List<TakaraCardData> takaraCards = new List<TakaraCardData>();
}

//--------------------------------------------------
public class TakaraCardData
{
    public int id;
    public string name;
    public Sprite frontSide;
    public Sprite backSide;
    public int count;
    public int gold;
    public string dsc;
    public List<TakaraCardContent> contents = new List<TakaraCardContent>(); 
}

//--------------------------------------------------
public class TakaraCardContent
{
    public DiceType diceType = new DiceType();
    public TakaraType takaraType = new TakaraType();
}

//--------------------------------------------------
public enum DiceType
{
    Null = 0, Dice1 = 1, Dice2 = 2, Dice3 = 3, Dice4 = 4, Dice5 = 5, Dice6 = 6,
}

//--------------------------------------------------
public enum TakaraType
{
    Null = 0, Type01 = 1, Type02 = 2, Type03 = 3, Type04 = 4, Type05 = 5, Type06 = 6, Type07 = 7,
}

#endregion

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Item cards data
/// </summary>
//////////////////////////////////////////////////////////////////////
#region ItemCardsData

//--------------------------------------------------
public class ItemCardsData
{
    public List<ItemCardData> itemCards = new List<ItemCardData>();
}

//--------------------------------------------------
public class ItemCardData
{
    public int id;
    public string name;
    public Sprite frontSide;
    public Sprite backSide;
    public string dsc;
    public int rare;
    public bool noroi;
    public List<ItemCardContent> contents = new List<ItemCardContent>();
}

//--------------------------------------------------
public class ItemCardContent
{
    public ItemContentType type = new ItemContentType();
    public int amount;
    public List<GameEventsTiming> tgrEvents = new List<GameEventsTiming>();
    public List<GameEventsTiming> endEvents = new List<GameEventsTiming>();
}

//--------------------------------------------------
public enum ItemContentType
{
    Null = 0,
    Type01 = 1, Type02 = 2, Type03 = 3, Type04 = 4, Type05 = 5,
    Type06 = 6, Type07 = 7, Type08 = 8, Type09 = 9,
    Type10 = 10, Type11 = 11, Type12 = 12, Type13 = 13, Type14 = 14,
    Type15 = 15, Type16 = 16, Type17 = 17, Type18 = 18, Type19 = 19,
    Type20 = 20, Type21 = 21, Type22 = 22, Type23 = 23, Type24 = 24,
    Type25 = 25, Type26 = 26, Type27 = 27, Type28 = 28, Type29 = 29,
    Type30 = 30, Type31 = 31, Type32 = 32, Type33 = 33,
}

#endregion

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Token
/// </summary>
//////////////////////////////////////////////////////////////////////
#region Token

//--------------------------------------------------
public enum TokenType
{
    Null, Shien, Move, Move1, Move2, Move3, Attack, Attack1, Attack2
}

//--------------------------------------------------
public class TokenValue
{
    public TokenType type = new TokenType();

    public int count;
}

//--------------------------------------------------
public class TokensData
{
    public TokenValue usedShienToken = new TokenValue();
    public TokenValue totalShienToken = new TokenValue();

    public TokenValue usedMoveToken
    {
        get
        {
            TokenValue value = new TokenValue();

            value.type = TokenType.Move;
            value.count = usedMove1Token.count + usedMove2Token.count + usedMove3Token.count;

            return value;
        }
    }
    public TokenValue totalMoveToken
    {
        get
        {
            TokenValue value = new TokenValue();

            value.type = TokenType.Move;
            value.count = totalMove1Token.count + totalMove2Token.count + totalMove3Token.count;

            return value;
        }
    }
    public TokenValue usedMove1Token = new TokenValue();
    public TokenValue totalMove1Token = new TokenValue();
    public TokenValue usedMove2Token = new TokenValue();
    public TokenValue totalMove2Token = new TokenValue();
    public TokenValue usedMove3Token = new TokenValue();
    public TokenValue totalMove3Token = new TokenValue();

    public TokenValue usedAtkToken
    {
        get
        {
            TokenValue value = new TokenValue();

            value.type = TokenType.Attack;
            value.count = usedAtk1Token.count + usedAtk2Token.count;

            return value;
        }
    }
    public TokenValue totalAtkToken
    {
        get
        {
            TokenValue value = new TokenValue();

            value.type = TokenType.Attack;
            value.count = totalAtk1Token.count + totalAtk2Token.count;

            return value;
        }
    }
    public TokenValue usedAtk1Token = new TokenValue();
    public TokenValue totalAtk1Token = new TokenValue();
    public TokenValue usedAtk2Token = new TokenValue();
    public TokenValue totalAtk2Token = new TokenValue();
}

#endregion

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Marker
/// </summary>
//////////////////////////////////////////////////////////////////////
#region Marker

    //--------------------------------------------------
    public enum MarkerType
    {
        Null, SP, AP, Gold, Turn
    }

    //--------------------------------------------------
    public class MarkerValue
    {
        public MarkerType type;

        public int count;
    }

    //--------------------------------------------------
    public class MarkersData
    {
        public MarkerValue usedSpMarkers = new MarkerValue();
        public MarkerValue totalSpMarkers = new MarkerValue();
        public MarkerValue usedGoldMarkers = new MarkerValue();
        public MarkerValue totalGoldMarkers = new MarkerValue();
        public MarkerValue apMarkers = new MarkerValue();
        public MarkerValue turnMarkers = new MarkerValue();
    }

#endregion

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Round
/// </summary>
//////////////////////////////////////////////////////////////////////
#region Round

//--------------------------------------------------
public class RoundValue
{
    public int index;

    public Transform roundPanel_Tf;

    public Transform allyVan1_Tf, allyVan2_Tf, enemyVan1_Tf, enemyVan2_Tf;

    public Transform markersGroup_Tf;

    public Transform token_Tf;

    public TokenValue token = new TokenValue();

    public int originUnitIndex, targetUnitIndex;

    public AttackType atkType = new AttackType();

    public UnitCard shienUnit_Cp;

    public List<Transform> marker_Tfs = new List<Transform>();

    public int spMarkerCount;

    public GameObject hlTarget_GO;

    public int minAgi;

    public ActionType action
    {
        get
        {
            ActionType value = new ActionType();

            switch (token.type)
            {
                case TokenType.Shien:
                    value = ActionType.Shien;
                    break;
                case TokenType.Move:
                    value = ActionType.Move;
                    break;
                case TokenType.Attack:
                    value = ActionType.Attack;
                    break;
                case TokenType.Null:
                    if (spMarkerCount > 0)
                    {
                        value = ActionType.Guard;
                    }
                    break;
            }

            return value;
        }
    }
}

//--------------------------------------------------
public enum ActionType
{
    Null, Guard, Shien, Move, Attack
}

#endregion

//////////////////////////////////////////////////////////////////////
/// <summary>
/// BattleInfo
/// </summary>
//////////////////////////////////////////////////////////////////////
#region BattleInfo

//--------------------------------------------------
public class BattleInfo
{
    public int mihariUnitCount;

    public int discardUnitCount;

    public int ken, deadKen;

    public int ma, deadMa;

    public int yumi, deadYumi;

    public int fushi, deadFushi;

    public int ryu, deadRyu;
}

#endregion

//////////////////////////////////////////////////////////////////////
/// <summary>
/// UnitInfo
/// </summary>
//////////////////////////////////////////////////////////////////////
#region UnitInfo

//--------------------------------------------------
public class UnitInfo
{
    public int cost, maxHP, curHp, baseAtk, atkCorr, baseAgi, agiCorr, defCorr, hitCorr, ctCorr,
        nlAtkCorr, spcAtkCorr, dmgCorr, indirDmgCorr, shienEffCorr, diceEffCorr;

    public bool visible, hpVisible;

    public bool isNlEnable, isSpc1Enable, isSpc2Enable, isUniqEnable, isShienEnable;

    public UnitCardData unitData = new UnitCardData();

    public List<UnitCard> shienUnit_Cps = new List<UnitCard>();

    public List<ItemCard> item_Cps = new List<ItemCard>();

    public AttackType occuredAtk = new AttackType();

    public List<ShienAbility> shienEffects
    {
        get
        {
            List<ShienAbility> value = new List<ShienAbility>();

            for (int i = 0; i < shienUnit_Cps.Count; i++)
            {
                value.Add(shienUnit_Cps[i].unitCardData.shienAbil);
            }

            return value;
        }
    }

    public int agi
    {
        get { return baseAgi + agiCorr; }
    }

    public int atk
    {
        get { return baseAtk + atkCorr; }
    }

    public int def
    {
        get { return defCorr; }
    }
}

#endregion

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Game Events Data
/// </summary>
//////////////////////////////////////////////////////////////////////
#region GameEventsData

//--------------------------------------------------
public class GameEventsData
{
    public List<GameEventData> gEvents = new List<GameEventData>();

    public GameEventData this[GameEventsTiming timing_pr]
    {
        get
        {
            GameEventData value = null;

            for(int i = 0; i < gEvents.Count; i++)
            {
                if (gEvents[i].timing == timing_pr)
                {
                    value = gEvents[i];
                }
            }

            return value;
        }
    }
}

//--------------------------------------------------
public class GameEventData
{
    public int id;
    public GameEventsTiming timing = new GameEventsTiming();
}

//--------------------------------------------------
public enum GameEventsTiming
{
    Null = 0,
    BefHitDet = 1, WhenHitDet = 2, AftAtk = 3, AftAtkSucc = 4, TurnEnd = 5,
    EndPhase = 6, ImmTgr = 7, UntilNextRnd = 8, BtlRevEffTgr = 9,
    ItemGetOrDest = 10, StrPhaseAtkSet = 11, BtlPhaseAftShienApply = 12, CalcDmgBesideAtk = 13, UnitDeath = 14,
    WhenMoved = 15, Btl1thRnd = 16, Btl2thRnd = 17, Btl3thRnd = 18,
    Btl4thRnd = 19, Btl5thRnd = 20, SupplyPhase = 21, NextAtk = 22, NextTakeDmg = 23,
    BtlPhase = 24, HpRec = 25, HitSuccOver8 = 26, NlAtk = 27, SpcAtk = 28, HitSuccOver6 = 29,
    AftSuccDetDest = 30, TurnStart = 31, HpNoReduce = 32, TakeDmg = 33, SpcAtkWithDefCorr = 34,
    TakeShien = 35, AftDice = 36,
    RndEnd = 37,
}

//--------------------------------------------------
public class GameEventsInfo
{
    public GameEventsTiming gTiming = new GameEventsTiming();
}

#endregion
