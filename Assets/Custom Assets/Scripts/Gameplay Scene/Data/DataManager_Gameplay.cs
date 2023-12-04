using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ExcelDataReader;
using System.Data;
using System.Linq;

public class DataManager_Gameplay : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish,
        LoadDBFinished, LoadUnitCardsDBFinished, LoadTakaraCardsDBFinished, LoadItemCardsDBFinished,
        LoadDatasetFinished, SetTablesFinished, LoadCardsDataFinished,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- static fields
    public static int maxPartyUnitsCount = 12;

    public static int maxBattleUnitsCount = 5;

    public static int maxVanUnitsCount = 2;

    public static int maxRearUnitsCount = 3;

    public static int maxStandUnitsCount = 7;

    public static List<int> cardLayers = new List<int>() { 6, 7, 8 };

    //-------------------------------------------------- serialize fields
    // test game data
    [SerializeField]
    string unitCardsDBRelativePath = "Data/GameplayData/UnitCards.txt",
        takaraCardsDBRelativePath = "Data/GameplayData/TakaraCards.txt",
        itemCardsDBRelativePath = "Data/GameplayData/ItemCards.txt";

    [SerializeField]
    string unitsRsPath = "Sprites/UnitCards",
        unitsBackSideRsPath = "Sprites/UnitCards/BackSide",
        takarasRsPath = "Sprites/TakaraCards",
        takarasBacksideRsPath = "Sprites/TakaraCards/BackSide",
        itemsRsPath = "Sprites/ItemCards",
        itemsBackSideRsPath = "Sprites/ItemCards/BackSide";

    // real game data
    [SerializeField]
    string realGameplayDataPath = "Assets/Data/GameplayData/GameplayData.xlsx";

    [SerializeField]
    string gameEventsDataPath = "Assets/Data/GameplayData/GameEvents.xlsx";

    [SerializeField]
    string unitTableName = "UnitList",
        takarasTableName = "TakaraList",
        itemTableName = "ItemList",
        qaTableName = "QA",
        termsTableName = "5R Terms",
        gameEventsTableName = "Game Events",
        itemEventsTableName = "Item Events";

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    // excel data
    public DataSet playData_dataSet = new DataSet();

    public DataSet events_dataSet = new DataSet();

    public DataTable unitsTable = new DataTable();

    public DataTable takarasTable = new DataTable();

    public DataTable itemsTable = new DataTable();

    public DataTable qaTable = new DataTable();

    public DataTable termsTable = new DataTable();

    public DataTable gameEventsTable = new DataTable();

    public DataTable itemEventsTable = new DataTable();

    // game play data
    public DataStorage_Gameplay dataStorage = new DataStorage_Gameplay();

    public List<UnitCardsData> psUnitCardsData = new List<UnitCardsData>();

    public List<UnitCardsData> psBattleUnitCardsData = new List<UnitCardsData>();

    public List<UnitCardsData> psVanUnitCardsData = new List<UnitCardsData>();

    public List<UnitCardsData> psRearUnitCardsData = new List<UnitCardsData>();

    public List<UnitCardsData> psMihariUnitCardsData = new List<UnitCardsData>();

    public List<TakaraCardData> takaraDatas = new List<TakaraCardData>();

    // game events data


    //-------------------------------------------------- private fields
    // test data
    string unitCardsDBPath, takaraCardsDBPath, itemCardsDBPath;

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Properties
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    // Methods
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //-------------------------------------------------- Start is called before the first frame update
    void Start()
    {

    }

    //-------------------------------------------------- Update is called once per frame
    void Update()
    {

    }

    //////////////////////////////////////////////////////////////////////
    // ManageGameState
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public void AddMainGameState(GameState_En value)
    {
        if (gameStates.Count == 0)
        {
            gameStates.Add(value);
        }
    }

    //--------------------------------------------------
    public void AddGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value_tp in values)
        {
            gameStates.Add(value_tp);
        }
    }

    //--------------------------------------------------
    public bool ExistGameStates(params GameState_En[] values)
    {
        bool result = true;
        foreach (GameState_En value in values)
        {
            if (!gameStates.Contains(value))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public bool ExistAnyGameStates(params GameState_En[] values)
    {
        bool result = false;
        foreach (GameState_En value in values)
        {
            if (gameStates.Contains(value))
            {
                result = true;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public int GetExistGameStatesCount(GameState_En value)
    {
        int result = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == value)
            {
                result++;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public void RemoveGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value in values)
        {
            gameStates.RemoveAll(gameState_tp => gameState_tp == value);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        StartCoroutine(CorouInit());
    }

    IEnumerator CorouInit()
    {
        gameStates.Add(GameState_En.Nothing);

        InitDBPaths();

        InitVariables();

        mainGameState = GameState_En.Inited;

        yield return null;
    }

    //--------------------------------------------------
    void InitDBPaths()
    {
        // test data
        string appDataPath = Application.dataPath;

        unitCardsDBPath = Path.Combine(appDataPath, unitCardsDBRelativePath);
        takaraCardsDBPath = Path.Combine(appDataPath, takaraCardsDBRelativePath);
        itemCardsDBPath = Path.Combine(appDataPath, itemCardsDBRelativePath);
    }

    //--------------------------------------------------
    void InitVariables()
    {
        StartCoroutine(CorouInitVariables());
    }

    IEnumerator CorouInitVariables()
    {
        //
        psUnitCardsData.Clear();
        psBattleUnitCardsData.Clear();
        psVanUnitCardsData.Clear();
        psRearUnitCardsData.Clear();
        psMihariUnitCardsData.Clear();

        //
        for (int i = 0; i < 2; i++)
        {
            psUnitCardsData.Add(new UnitCardsData());

            psBattleUnitCardsData.Add(new UnitCardsData());

            psVanUnitCardsData.Add(new UnitCardsData());

            psRearUnitCardsData.Add(new UnitCardsData());

            psMihariUnitCardsData.Add(new UnitCardsData());
        }

        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Load gameplay data
    //////////////////////////////////////////////////////////////////////
    #region LoadGameplayData

    //--------------------------------------------------
    public void LoadGameplayData()
    {
        //StartCoroutine(CorouLoadGameplayData());

        StartCoroutine(Corou_LoadGamePlayRealData());
    }

    IEnumerator CorouLoadGameplayData()
    {
        LoadUnitCardsData();
        LoadTakaraCardsData();
        LoadItemCardsData();

        yield return new WaitUntil(() => ExistGameStates(GameState_En.LoadUnitCardsDBFinished,
            GameState_En.LoadTakaraCardsDBFinished, GameState_En.LoadItemCardsDBFinished));

        RemoveGameStates(GameState_En.LoadUnitCardsDBFinished, GameState_En.LoadTakaraCardsDBFinished,
            GameState_En.LoadItemCardsDBFinished);

        mainGameState = GameState_En.LoadDBFinished;
    }

    //--------------------------------------------------
    void LoadUnitCardsData()
    {
        StartCoroutine(CorouLoadUnitCardsData());
    }

    IEnumerator CorouLoadUnitCardsData()
    {
        UnitCardsData unitCardsData = new UnitCardsData();

        //
        string[] dbLines = File.ReadAllLines(unitCardsDBPath);

        //
        for(int i = 0; i < dbLines.Length; i++)
        {
            UnitCardData unitCardData = new UnitCardData();

            //
            dbLines[i] = dbLines[i].Trim();

            string[] dbSections = dbLines[i].Split(';');

            if(dbSections.Length == 0)
            {
                Debug.LogWarning("Invalid UnitCard data has been detected at " + i + " line");
                continue;
            }

            bool dbPartsError = false;

            for (int j = 0; j < dbSections.Length; j++)
            {
                dbSections[j] = dbSections[j].Trim();

                string[] dbParts = dbSections[j].Split(':');
                
                if(dbParts.Length != 2)
                {
                    Debug.LogWarning("Invalid UnitCard data has been detected at " + i + " line "
                        + j + " section");
                    continue;
                }

                for(int k = 0; k < dbParts.Length; k++)
                {
                    dbParts[k] = dbParts[k].Trim();
                    if (string.IsNullOrEmpty(dbParts[k]))
                    {
                        Debug.LogWarning("Invalid UnitCard data has been detected at " + i + " line "
                                + j + " section " + k + " part");
                    }
                }

                //
                switch (dbParts[0])
                {
                    case "index":
                        if (!int.TryParse(dbParts[1], out unitCardData.id))
                        {
                            dbPartsError = true;
                        }
                        break;
                    case "name":
                        unitCardData.name = dbParts[1];
                        break;
                    case "frontSide":
                        if (!LoadSprite(Path.Combine(unitsRsPath, dbParts[1]),
                            out unitCardData.frontSide))
                        {
                            dbPartsError = true;
                        }
                        break;
                    case "backSide":
                        if (!LoadSprite(Path.Combine(unitsRsPath, dbParts[1]),
                            out unitCardData.backSide))
                        {
                            dbPartsError = true;
                        }
                        break;
                    case "cost":
                        if (!int.TryParse(dbParts[1], out unitCardData.cost))
                        {
                            dbPartsError = true;
                        }
                        break;
                    // *
                    default:
                        //*
                        break;
                }

                if (dbPartsError)
                {
                    Debug.LogWarning("Invalid UnitCard data has been detected at dbParts");
                    break;
                }
            }

            if (!dbPartsError)
            {
                unitCardsData.unitCards.Add(unitCardData);
            }
            else
            {
                dbPartsError = false;
            }
        }

        dataStorage.unitCardsData = unitCardsData;

        gameStates.Add(GameState_En.LoadUnitCardsDBFinished);

        yield return null;
    }

    //--------------------------------------------------
    void LoadTakaraCardsData()
    {
        StartCoroutine(CorouLoadTakaraCardsData());
    }

    IEnumerator CorouLoadTakaraCardsData()
    {
        // *

        gameStates.Add(GameState_En.LoadTakaraCardsDBFinished);

        yield return null;
    }

    //--------------------------------------------------
    void LoadItemCardsData()
    {
        StartCoroutine(CorouLoadItemCardsData());
    }

    IEnumerator CorouLoadItemCardsData()
    {
        // *

        gameStates.Add(GameState_En.LoadItemCardsDBFinished);

        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Load real data of gameplay
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region LoadGamePlayRealData

    //--------------------------------------------------
    IEnumerator Corou_LoadGamePlayRealData()
    {
        SetDataset();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.LoadDatasetFinished));
        RemoveGameStates(GameState_En.LoadDatasetFinished);
        yield return new WaitForSeconds(Time.deltaTime);

        SetTables();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.SetTablesFinished));
        RemoveGameStates(GameState_En.SetTablesFinished);
        yield return new WaitForSeconds(Time.deltaTime);

        SetTablesDataToStorage();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.LoadCardsDataFinished));
        RemoveGameStates(GameState_En.LoadCardsDataFinished);
        yield return new WaitForSeconds(Time.deltaTime);

        //
        mainGameState = GameState_En.LoadDBFinished;
    }

    //--------------------------------------------------
    void SetDataset()
    {
        StartCoroutine(Corou_SetDataSet());
    }

    IEnumerator Corou_SetDataSet()
    {
        //
        yield return new WaitForSeconds(Time.deltaTime);

        FileStream stream = File.Open(realGameplayDataPath, FileMode.Open, FileAccess.Read);

        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        playData_dataSet = excelReader.AsDataSet();

        excelReader.Close();

        //
        yield return new WaitForSeconds(Time.deltaTime);

        FileStream stream_Events = File.Open(gameEventsDataPath, FileMode.Open, FileAccess.Read);

        IExcelDataReader excelReader_Events = ExcelReaderFactory.CreateOpenXmlReader(stream_Events);

        events_dataSet = excelReader_Events.AsDataSet();

        excelReader_Events.Close();

        yield return new WaitForSeconds(Time.deltaTime);

        //
        AddGameStates(GameState_En.LoadDatasetFinished);
    }

    //--------------------------------------------------
    void SetTables()
    {
        StartCoroutine(Corou_SetTables());
    }

    IEnumerator Corou_SetTables()
    {
        //
        yield return new WaitForSeconds(Time.deltaTime);

        unitsTable = playData_dataSet.Tables[unitTableName];
        takarasTable = playData_dataSet.Tables[takarasTableName];
        itemsTable = playData_dataSet.Tables[itemTableName];
        qaTable = playData_dataSet.Tables[qaTableName];
        termsTable = playData_dataSet.Tables[termsTableName];

        //
        yield return new WaitForSeconds(Time.deltaTime);

        gameEventsTable = events_dataSet.Tables[gameEventsTableName];
        itemEventsTable = events_dataSet.Tables[itemEventsTableName];

        yield return new WaitForSeconds(Time.deltaTime);

        //
        AddGameStates(GameState_En.SetTablesFinished);
    }

    //--------------------------------------------------
    void SetTablesDataToStorage()
    {
        StartCoroutine(Corou_SetTablesDataToStorage());
    }

    IEnumerator Corou_SetTablesDataToStorage()
    {
        SetUnitCardsData();
        yield return new WaitForSeconds(Time.deltaTime);

        SetTakaraCardsData();
        yield return new WaitForSeconds(Time.deltaTime);

        SetItemCardsData();
        yield return new WaitForSeconds(Time.deltaTime);

        SetGameEventsData();
        yield return new WaitForSeconds(Time.deltaTime);

        AddGameStates(GameState_En.LoadCardsDataFinished);
    }

    //--------------------------------------------------
    void SetUnitCardsData()
    {
        UnitCardsData unitCardsData = new UnitCardsData();

        for (int i = 3; i <= 50; i++)
        {
            UnitCardData unitCardData = new UnitCardData();

            //
            DataRow row = unitsTable.Rows[i];

            //
            unitCardData.id = int.Parse(row[0].ToString());
            unitCardData.name = row[1].ToString();
            unitCardData.frontSide = Resources.Load<Sprite>(Path.Combine(unitsRsPath, row[2].ToString()));
            unitCardData.backSide = Resources.Load<Sprite>(Path.Combine(unitsBackSideRsPath,
                row[3].ToString()));
            unitCardData.cost = int.Parse(row[4].ToString());

            if (row[5].ToString().Contains("ふし"))
            {
                unitCardData.attrib.Add(UnitAttribute.Fushi);
            }
            if (row[5].ToString().Contains("けん"))
            {
                unitCardData.attrib.Add(UnitAttribute.Ken);
            }
            if (row[5].ToString().Contains("ま"))
            {
                unitCardData.attrib.Add(UnitAttribute.Ma);
            }
            if (row[5].ToString().Contains("ゆみ"))
            {
                unitCardData.attrib.Add(UnitAttribute.Yumi);
            }
            if (row[5].ToString().Contains("りゅう"))
            {
                unitCardData.attrib.Add(UnitAttribute.Ryu);
            }

            unitCardData.hp = int.Parse(row[6].ToString());
            unitCardData.atk = int.Parse(row[7].ToString());
            unitCardData.agi = int.Parse(row[8].ToString());

            // set normal attack data
            unitCardData.nlAtk.ap = int.Parse(row[9].ToString());
            unitCardData.nlAtk.title = row[10].ToString();
            for (int j = 11; j <= 19; j++)
            {
                if (!string.IsNullOrEmpty(row[j].ToString()))
                {
                    NormalAttackContent nlAtkCont = new NormalAttackContent();

                    //
                    nlAtkCont.type = (NormalAttackType)(j - 10);
                    nlAtkCont.amount = int.Parse(row[j].ToString());

                    NormalAttackEvent nlAtkTgrEvent = new NormalAttackEvent();
                    if (j >= 11 && j <= 17)
                    {
                        nlAtkTgrEvent = NormalAttackEvent.BefHitDet;
                    }
                    else if (j >= 18 && j <= 19)
                    {
                        nlAtkTgrEvent = NormalAttackEvent.WhenHitDet;
                    }
                    nlAtkCont.tgrEvents.Add(nlAtkTgrEvent);

                    nlAtkCont.endEvents.Add(NormalAttackEvent.TurnEnd);
                    nlAtkCont.endEvents.Add(NormalAttackEvent.EndPhase);

                    //
                    unitCardData.nlAtk.contents.Add(nlAtkCont);
                }
            }

            // set special attack 1 data
            unitCardData.spcAtk1.id = int.Parse(row[0].ToString());
            unitCardData.spcAtk1.ap = int.Parse(row[20].ToString());
            unitCardData.spcAtk1.sp = int.Parse(row[21].ToString());
            unitCardData.spcAtk1.dsc = row[22].ToString();
            for (int j = 23; j <= 41; j++)
            {
                if (!string.IsNullOrEmpty(row[j].ToString()))
                {
                    SpecialAttack1Content spcAtk1Cont = new SpecialAttack1Content();

                    spcAtk1Cont.type = (SpecialAttack1Type)(j - 22);
                    spcAtk1Cont.amount = int.Parse(row[j].ToString());

                    if (j >= 23 && j <= 29)
                    {
                        spcAtk1Cont.tgrEvents.Add(SpecialAttack1Event.BefHitDet);
                    }
                    if (j >= 32 && j <= 41)
                    {
                        spcAtk1Cont.tgrEvents.Add(SpecialAttack1Event.BefHitDet);
                        spcAtk1Cont.tgrEvents.Add(SpecialAttack1Event.ImmTgr);
                    }

                    if (j >= 23 && j <= 37)
                    {
                        spcAtk1Cont.endEvents.Add(SpecialAttack1Event.TurnEnd);
                        spcAtk1Cont.endEvents.Add(SpecialAttack1Event.EndPhase);
                    }
                    else if (j >= 38 && j <= 41)
                    {
                        spcAtk1Cont.endEvents.Add(SpecialAttack1Event.NextRound);
                    }

                    unitCardData.spcAtk1.contents.Add(spcAtk1Cont);
                }
            }

            // set special attack 2 data
            unitCardData.spcAtk2.id = int.Parse(row[0].ToString());
            unitCardData.spcAtk2.ap = int.Parse(row[42].ToString());
            unitCardData.spcAtk2.sp = int.Parse(row[43].ToString());
            unitCardData.spcAtk2.dsc = row[44].ToString();
            for (int j = 45; j <= 81; j++)
            {
                if (!string.IsNullOrEmpty(row[j].ToString()))
                {
                    SpecialAttack2Content spcAtk2Cont = new SpecialAttack2Content();

                    //
                    spcAtk2Cont.type = (SpecialAttack2Type)(j - 44);
                    spcAtk2Cont.amount = int.Parse(row[j].ToString());

                    if (j >= 45 && j <= 61)
                    {
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.BefHitDet);
                    }
                    else if (j >= 62 && j <= 63)
                    {
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.ImmTgr);
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.BefHitDet);
                    }
                    else if (j >= 64 && j <= 68)
                    {
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.ImmTgr);
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.WhenHitDet);
                    }
                    else if (j >= 69 && j <= 72)
                    {
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.ImmTgr);
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.AtkSucc);
                    }
                    else if (j >= 73 && j <= 81)
                    {
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.ImmTgr);
                        spcAtk2Cont.tgrEvents.Add(SpecialAttack2Event.AftAtk);
                    }

                    spcAtk2Cont.endEvents.Add(SpecialAttack2Event.TurnEnd);
                    spcAtk2Cont.endEvents.Add(SpecialAttack2Event.EndPhase);

                    //
                    unitCardData.spcAtk2.contents.Add(spcAtk2Cont);
                }
            }

            // set unique ability data
            unitCardData.uniqAbil.dsc = row[83].ToString();
            for (int j = 84; j <= 99; j++)
            {
                if (!string.IsNullOrEmpty(row[j].ToString()))
                {
                    // add trigger events
                    unitCardData.uniqAbil.tgrEvents.Add((UniqueAbilityEvent)(j - 83));

                    unitCardData.uniqAbil.type = (UniqueAbilityType)(int.Parse(row[0].ToString()));

                    // add end events
                    unitCardData.uniqAbil.endEvents.Add(UniqueAbilityEvent.TurnEnd);

                    if (unitCardData.uniqAbil.type == UniqueAbilityType.Type02 ||
                        unitCardData.uniqAbil.type == UniqueAbilityType.Type17)
                    {
                        unitCardData.uniqAbil.endEvents.Add(UniqueAbilityEvent.RndEnd);
                    }
                }
            }

            // set shien ability data
            unitCardData.shienAbil.dsc = row[101].ToString();
            for (int j = 102; j <= 107; j++)
            {
                if (j == 102 || j == 103 || j == 104)
                {
                    unitCardData.shienAbil.tgrEvent = ShienAbilityEvent.NextAtk;
                }
                else if (j == 105)
                {
                    unitCardData.shienAbil.tgrEvent = ShienAbilityEvent.NextDmg;
                }
                else if (j == 106)
                {
                    unitCardData.shienAbil.tgrEvent = ShienAbilityEvent.HpRec;
                }

                unitCardData.shienAbil.endEvent = ShienAbilityEvent.EndPhase;

                unitCardData.shienAbil.type = (ShienAbilityType)(j - 101);
            }

            //
            unitCardsData.unitCards.Add(unitCardData);
        }

        dataStorage.unitCardsData = unitCardsData;
    }

    //--------------------------------------------------
    void SetTakaraCardsData()
    {
        TakaraCardsData takaraCardsData = new TakaraCardsData();

        for (int i = 1; i <= 17; i++)
        {
            TakaraCardData takaraCardData = new TakaraCardData();

            DataRow row = takarasTable.Rows[i];

            takaraCardData.id = int.Parse(row[0].ToString());
            takaraCardData.name = row[1].ToString();
            takaraCardData.count = int.Parse(row[2].ToString());
            takaraCardData.frontSide = Resources.Load<Sprite>(Path.Combine(takarasRsPath, row[3].ToString()));
            takaraCardData.backSide = Resources.Load<Sprite>(Path.Combine(takarasBacksideRsPath, "BackSide"));
            takaraCardData.gold = int.Parse(row[4].ToString());
            takaraCardData.dsc = row[5].ToString();

            for (int j = 6; j <= 12; j++)
            {
                TakaraCardContent takaraCont = new TakaraCardContent();

                takaraCont.diceType = (DiceType)(j - 5);
                takaraCont.takaraType = TakaraType.Type01; // it will be rewrite

                takaraCardData.contents.Add(takaraCont);
            }

            takaraCardsData.takaraCards.Add(takaraCardData);
        }

        dataStorage.takaraCardsData = takaraCardsData;
    }

    //--------------------------------------------------
    void SetItemCardsData()
    {
        ItemCardsData itemCardsData = new ItemCardsData();

        for (int i = 1; i <= 37; i++)
        {
            ItemCardData itemCardData = new ItemCardData();

            //
            DataRow row = itemsTable.Rows[i];

            itemCardData.id = int.Parse(row[0].ToString());
            itemCardData.name = row[1].ToString();
            itemCardData.frontSide = Resources.Load<Sprite>(Path.Combine(itemsRsPath, row[2].ToString()));
            itemCardData.backSide = Resources.Load<Sprite>(Path.Combine(itemsBackSideRsPath, row[3].ToString()));
            itemCardData.dsc = row[4].ToString();
            itemCardData.rare = int.Parse(row[5].ToString());
            itemCardData.noroi = string.IsNullOrEmpty(row[6].ToString()) ? false : true;

            for (int k = 1; k <= 14; k++)
            {
                DataRow eventsRow = itemEventsTable.Rows[k];
                if (!string.IsNullOrEmpty(eventsRow[i + 1].ToString()))
                {
                    itemCardData.tgrEvents.Add((ItemEvent)k);
                }
            }
            itemCardData.endEvents.Add(ItemEvent.UnitDead);

            for (int j = 7; j <= 39; j++)
            {
                if (!string.IsNullOrEmpty(row[j].ToString()))
                {
                    ItemCardContent cont = new ItemCardContent();

                    cont.type = (ItemContentType)(j - 6);
                    cont.amount = int.Parse(row[j].ToString());

                    itemCardData.contents.Add(cont);
                }
            }

            //
            itemCardsData.itemCards.Add(itemCardData);
        }

        dataStorage.itemCardsData = itemCardsData;
    }

    //--------------------------------------------------
    void SetGameEventsData()
    {
        GameEventsData gameEventsData = new GameEventsData();

        //
        for (int i = 1; i <= 37; i++)
        {
            GameEventData gameEventData = new GameEventData();

            //
            DataRow row = gameEventsTable.Rows[i];

            gameEventData.id = int.Parse(row[0].ToString());
            gameEventData.dsc = row[1].ToString();
            gameEventData.timing = (GameEventsTiming)(gameEventData.id);

            if (!string.IsNullOrEmpty(row[2].ToString()))
            {
                gameEventData.nlEvent = (NormalAttackEvent)(int.Parse(row[2].ToString()));
            }
            if (!string.IsNullOrEmpty(row[3].ToString()))
            {
                gameEventData.spc1Event = (SpecialAttack1Event)(int.Parse(row[3].ToString()));
            }
            if (!string.IsNullOrEmpty(row[4].ToString()))
            {
                gameEventData.spc2Event = (SpecialAttack2Event)(int.Parse(row[4].ToString()));
            }
            if (!string.IsNullOrEmpty(row[5].ToString()))
            {
                gameEventData.uniqEvent = (UniqueAbilityEvent)(int.Parse(row[5].ToString()));
            }
            if (!string.IsNullOrEmpty(row[6].ToString()))
            {
                gameEventData.shienEvent = (ShienAbilityEvent)(int.Parse(row[6].ToString()));
            }
            if (!string.IsNullOrEmpty(row[7].ToString()))
            {
                gameEventData.itemEvent = (ItemEvent)(int.Parse(row[7].ToString()));
            }

            //
            gameEventsData.gEvents.Add(gameEventData);
        }

        //
        dataStorage.gameEventsData = gameEventsData;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Generate random UnitCardsData
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region GenerateRandomUnitCardsData

    //--------------------------------------------------
    public void GenerateRandomUnitCardsData_PartyDecision()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateRandomPlayerUnitCardsData(i);
        }
    }

    //--------------------------------------------------
    public void GenerateRandomUnitCardsData_SetupStand()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateRandomPlayerUnitCardsData(i);
            GenerateRandomPlayerBattleUnitCardsData(i);
            GenerateRandomPlayerMihariUnitCardsData(i);
        }
    }

    //--------------------------------------------------
    public void GenRandUnitCardsData_Phases()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateRandomPlayerUnitCardsData(i);
            GenerateRandomPlayerBattleUnitCardsData(i);
            GenerateRandomPlayerMihariUnitCardsData(i);
        }
    }

    //--------------------------------------------------
    void GenerateRandomPlayerUnitCardsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxPartyUnitsCount;
        int otherPlayerID_pr = playerID_pr == 0 ? 1 : 0;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();
        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, dataStorage.unitCardsData.unitCards.Count);
            UnitCardData unitCardData_tp = dataStorage.unitCardsData.unitCards[randIndex];

            if (!unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !psUnitCardsData[otherPlayerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }

        unitCardsData_tp.playerID = playerID_pr;

        //
        psUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    public void GenPlayerUnitCardsDataByIndex(int playerID_pr, params int[] indexes)
    {
        int unitsCount_pr = DataManager_Gameplay.maxPartyUnitsCount;
        int otherPlayerID_pr = playerID_pr == 0 ? 1 : 0;

        //
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] -= 1;
        }

        UnitCardsData unitCardsData_tp = new UnitCardsData();
        for (int i = 0; i < indexes.Length; i++)
        {
            UnitCardData unitCardData_tp = dataStorage.unitCardsData.unitCards[indexes[i]];

            if (!unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !psUnitCardsData[otherPlayerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }

        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, dataStorage.unitCardsData.unitCards.Count);
            UnitCardData unitCardData_tp = dataStorage.unitCardsData.unitCards[randIndex];

            if (!unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !psUnitCardsData[otherPlayerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }

        unitCardsData_tp.playerID = playerID_pr;

        //
        psUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    public void GenerateRandomPlayerBattleUnitCardsData(int playerID_pr)
    {
        //GenerateRandomPlayerVanUnitCardsData(playerID_pr);
        //GenerateRandomPlayerRearUnitCardsData(playerID_pr);
        GenRandPlVanUnitsData(playerID_pr);
        GenRandPlRearUnitsData(playerID_pr);

        psBattleUnitCardsData[playerID_pr].unitCards.AddRange(
            psVanUnitCardsData[playerID_pr].unitCards);
        psBattleUnitCardsData[playerID_pr].unitCards.AddRange(
            psRearUnitCardsData[playerID_pr].unitCards);
    }

    //--------------------------------------------------
    void GenerateRandomPlayerVanUnitCardsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxVanUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();
        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, psUnitCardsData[playerID_pr].unitCards.Count);
            UnitCardData unitCardData_tp = psUnitCardsData[playerID_pr].unitCards[randIndex];

            if (!unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !psRearUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psMihariUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        psVanUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    void GenerateRandomPlayerRearUnitCardsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxRearUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();
        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, psUnitCardsData[playerID_pr].unitCards.Count);
            UnitCardData unitCardData_tp = psUnitCardsData[playerID_pr].unitCards[randIndex];

            if (!psVanUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !psBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psMihariUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        psRearUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    void GenRandPlVanUnitsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxVanUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();

        int index = 0;

        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            UnitCardData unitCardData_tp = psUnitCardsData[playerID_pr].unitCards[index];

            if (!unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !psRearUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psMihariUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }

            //
            index++;
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        psVanUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    void GenRandPlRearUnitsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxRearUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();

        int index = psVanUnitCardsData[playerID_pr].unitCards.Count;

        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            UnitCardData unitCardData_tp = psUnitCardsData[playerID_pr].unitCards[index];

            if (!psVanUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !psBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psMihariUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }

            //
            index++;
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        psRearUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    public void GenerateRandomPlayerMihariUnitCardsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxStandUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();
        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, psUnitCardsData[playerID_pr].unitCards.Count);
            UnitCardData unitCardData_tp = psUnitCardsData[playerID_pr].unitCards[randIndex];

            if (!psVanUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psRearUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !unitCardsData_tp.unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        psMihariUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    public void GenRandPlMUnitsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxStandUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();

        int index = 0;

        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            UnitCardData unitCardData_tp = psUnitCardsData[playerID_pr].unitCards[index];

            if (!psVanUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psRearUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !psBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !unitCardsData_tp.unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }

            //
            index++;
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        psMihariUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Supply TakaraCardsData
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Supply TakaraCardsData

    //--------------------------------------------------
    public void SupplyTakaraCardsData_Phases()
    {
        List<TakaraCardData> takaraCardDatas_tp = dataStorage.takaraCardsData.takaraCards;
        takaraDatas = new List<TakaraCardData>(takaraCardDatas_tp);

        // Use Fisher-Yates shuffle to randomize the order of takaraCardDatas
        int n = takaraDatas.Count;
        System.Random random = new System.Random();
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            TakaraCardData temp = takaraDatas[i];
            takaraDatas[i] = takaraDatas[j];
            takaraDatas[j] = temp;
        }


    }

    #endregion

    //--------------------------------------------------
    public UnitCardData GetUnitCardDataFromCardIndex(int cardIndex_pr)
    {
        List<UnitCardData> unitCardData_Cps_pr = dataStorage.unitCardsData.unitCards;

        return DataManager_Gameplay.GetUnitCardDataFromCardIndex(unitCardData_Cps_pr, cardIndex_pr);
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Static methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region StaticMethods

    //------------------------------
    public static bool IsCard(GameObject target_GO_pr)
    {
        bool result = false;

        List<int> cardLayers = new List<int>() { 6, 7, 8 };

        for (int i = 0; i < cardLayers.Count; i++)
        {
            if (target_GO_pr.layer == cardLayers[i])
            {
                result = true;
                break;
            }
        }

        return result;
    }

    //------------------------------
    public static bool LoadSprite(string path, out Sprite sprite_pr)
    {
        bool result = true;

        sprite_pr = Resources.Load<Sprite>(path);

        if(sprite_pr == null)
        {
            result = false;
        }

        return result;
    }

    //------------------------------
    public static UnitCardData GetUnitCardDataFromCardIndex(List<UnitCardData> unitCardData_Cps_pr,
        int cardIndex_pr)
    {
        UnitCardData result = new UnitCardData();

        for (int i = 0; i < unitCardData_Cps_pr.Count; i++)
        {
            if (unitCardData_Cps_pr[i].id == cardIndex_pr)
            {
                result = unitCardData_Cps_pr[i];
                break;
            }
        }

        return result;
    }

    //------------------------------
    public static UnitCard GetUnitFromCardIndex(List<UnitCard> unit_Cps_pr, int cardIndex_pr)
    {
        UnitCard result = null;

        for (int i = 0; i < unit_Cps_pr.Count; i++)
        {
            if (unit_Cps_pr[i].cardIndex == cardIndex_pr)
            {
                result = unit_Cps_pr[i];
                break;
            }
        }

        return result;
    }

    #endregion

}
