using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FiveRound
{
    public class UI_TopCanvas : MonoBehaviour
    {

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Types
        /// </summary>
        //////////////////////////////////////////////////////////////////////
        #region Types

        //--------------------------------------------------
        public enum GameState_En
        {
            Nothing, Inited, Playing, WillFinish,
            TitleShowed,
            LoginStarted,
            LoadStarted, LoadFinished,
        }

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fields
        /// </summary>
        //////////////////////////////////////////////////////////////////////
        #region

        //-------------------------------------------------- serialize fields
        [SerializeField] Animator blankAnim_Cp, bgdAnim_Cp, splashAnim_Cp;
        [SerializeField] GameObject advanceBgd_GO;
        [SerializeField] UnityAction onLoginBtnClick, onCreateAccountBtnClick, onSignupCancelBtnClick, onSignupBtnClick;

        //-------------------------------------------------- public fields
        [ReadOnly]
        public List<GameState_En> gameStates = new List<GameState_En>();

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Properties
        /// </summary>
        //////////////////////////////////////////////////////////////////////
        #region

        //-------------------------------------------------- public properties
        public GameState_En mainGameState
        {
            get { return gameStates[0]; }
            set { gameStates[0] = value; }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Methods
        /// </summary>
        //////////////////////////////////////////////////////////////////////

        // Start is called before the first frame update
        void Start()
        {
            
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Manage gameStates
        /// </summary>
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
        /// <summary>
        /// Initialize
        /// </summary>
        //////////////////////////////////////////////////////////////////////
        #region Initialize

        //--------------------------------------------------
        public void Init()
        {
            StartCoroutine(Corou_Init());
        }

        IEnumerator Corou_Init()
        {
            AddMainGameState(GameState_En.Nothing);

            advanceBgd_GO.SetActive(true);
            splashAnim_Cp.gameObject.SetActive(true);

            Hash128 hash_tp = HashHandler.RegRandHash();
            PlayTriggerAnimation(blankAnim_Cp, "Out", "Fade Out", hash_tp);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash_tp));

            mainGameState = GameState_En.Inited;
        }

        //--------------------------------------------------
        public void SetUnityActions(UnityAction onLoginBtnClick_tp, UnityAction onCreateAccountBtnClick_tp,
            UnityAction onSignupCancelBtnClick_tp, UnityAction onSignupBtnClick_tp)
        {
            onLoginBtnClick = onLoginBtnClick_tp;
            onCreateAccountBtnClick = onCreateAccountBtnClick_tp;
            onSignupCancelBtnClick = onSignupCancelBtnClick_tp;
            onSignupBtnClick = onSignupBtnClick_tp;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Play
        /// </summary>
        //////////////////////////////////////////////////////////////////////
        #region Play

        //--------------------------------------------------
        public void Play_PressAnyKey()
        {
            StartCoroutine(Corou_Play_PressAnyKey());
        }

        IEnumerator Corou_Play_PressAnyKey()
        {
            Hash128 hash_tp = HashHandler.RegRandHash();
            PlayTriggerAnimation(splashAnim_Cp, "PressAnyKey", "SS Press Any Key", hash_tp);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash_tp));

            mainGameState = GameState_En.TitleShowed;
        }

        //--------------------------------------------------
        public void Play_Login()
        {
            StartCoroutine(Corou_Play_Login());
        }

        IEnumerator Corou_Play_Login()
        {
            Hash128 hash_tp = HashHandler.RegRandHash();
            PlayTriggerAnimation(splashAnim_Cp, "Login", "SS Login", hash_tp);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash_tp));

            mainGameState = GameState_En.LoginStarted;
        }

        //--------------------------------------------------
        public void Play_Load()
        {
            StartCoroutine(Corou_Play_Load());
        }

        IEnumerator Corou_Play_Load()
        {
            Hash128 hash_tp = HashHandler.RegRandHash();
            PlayTriggerAnimation(splashAnim_Cp, "Loading", "SS Loading", hash_tp);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash_tp));

            mainGameState = GameState_En.LoadStarted;
        }

        //--------------------------------------------------
        public void Finish_Load()
        {
            StartCoroutine(Corou_Finish_Load());
        }

        IEnumerator Corou_Finish_Load()
        {
            Hash128 hash3_tp = HashHandler.RegRandHash();
            PlayTriggerAnimation(splashAnim_Cp, "LoadingOut", "SS Loading Out", hash3_tp);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash3_tp));

            Hash128 hash4_tp = HashHandler.RegRandHash();
            PlayTriggerAnimation(bgdAnim_Cp, "Out", "Fade Out", hash4_tp);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash4_tp));

            mainGameState = GameState_En.LoadFinished;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Functionalities
        /// </summary>
        //////////////////////////////////////////////////////////////////////
        #region Functionalities

        //--------------------------------------------------
        public void PlayTriggerAnimation(Animator anim_Cp, string triggerName, string clipName,
            Hash128 hash_pr)
        {
            StartCoroutine(Corou_PlayTriggerAnimation(anim_Cp, triggerName, clipName, hash_pr));
        }

        IEnumerator Corou_PlayTriggerAnimation(Animator anim_Cp, string triggerName, string clipName,
            Hash128 hash_pr)
        {
            Hash128 hash_tp = HashHandler.RegRandHash();
            AnimatorFunc.AddEvent(anim_Cp, clipName, hash_tp);
            anim_Cp.SetTrigger(triggerName);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash_tp));
            AnimatorFunc.RemoveEvent(anim_Cp, clipName);

            HashHandler.RemoveHash(hash_pr);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Callback from UI
        /// </summary>
        //////////////////////////////////////////////////////////////////////
        #region Callback from UI

        //--------------------------------------------------
        public void OnLoginBtnClick()
        {
            StartCoroutine(Corou_OnLoginBtnClick());
        }

        IEnumerator Corou_OnLoginBtnClick()
        {
            Hash128 hash_tp = HashHandler.RegRandHash();
            PlayTriggerAnimation(splashAnim_Cp, "Login2Loading", "SS Login to Loading", hash_tp);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash_tp));

            onLoginBtnClick?.Invoke();

            mainGameState = GameState_En.LoadStarted;
        }

        //--------------------------------------------------
        public void OnCreateAccountBtnClick()
        {
            splashAnim_Cp.SetTrigger("Login2Signup");

            onCreateAccountBtnClick?.Invoke();
        }

        //--------------------------------------------------
        public void OnSignUpCancelBtnClick()
        {
            splashAnim_Cp.SetTrigger("Signup2Login");

            onSignupCancelBtnClick?.Invoke();
        }

        //--------------------------------------------------
        public void OnSignUpBtnClick()
        {
            splashAnim_Cp.SetTrigger("Signup2Login");

            onSignupCancelBtnClick?.Invoke();
        }

        #endregion

    }

}