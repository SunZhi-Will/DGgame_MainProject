using UnityEngine;
using System.Collections;

//Koroshi
//約2017寫的動畫偵測工具
namespace AnimAction
{
    public static class AnimScript
    {
        #region 直接播放動畫
        /// <summary>
        /// 無視「現在」播放動畫，直接播放「指定」動畫
        /// 說明：就算和現在播放的動畫相同也回強制播放
        /// </summary>
        /// <param name="anim">參數1:Animator組件</param>
        /// <param name="_animName">參數2:欲播放動畫名稱</param>
        public static void Play(Animator anim, string _animName)
        {
            //播放動畫
            anim.Play(_animName, 0, 0);
        }
        #endregion

        #region 直接播放動畫，並判斷重播
        /// <summary>
        /// 直接播放指定動畫
        /// 說明:播放該動畫，並判斷是否重播
        /// </summary>
        /// <param name="anim">參數1:Animator組件</param>
        /// <param name="_animName">參數2:欲播放動畫名稱</param>
        /// <param name="fps">參數3:是否重播</param>
        /// <param name="Reset">參數3:是否重播，預設為關</param>
        public static void Play(Animator anim, string _animName, bool Reset)
        {
            //動畫狀態
            AnimatorStateInfo animState;

            //動畫狀態擷取
            animState = anim.GetCurrentAnimatorStateInfo(0);

            //如果尚未處於該動畫狀態
            if (animState.IsName(_animName) == false)
            {
                //播放動畫
                anim.Play(_animName, 0, 0);
            }
            //當該動畫播完時
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= (anim.GetCurrentAnimatorStateInfo(0).length / anim.GetCurrentAnimatorStateInfo(0).length))
            {
                //判斷是否重播
                if (Reset == true)
                    //重新播放動畫
                    anim.Play(_animName, 0, 0);

                //否則只播放一次
                else if (animState.IsName(_animName) == false)
                    anim.Play(_animName, 0, 0);
            }
        }
        #endregion

        #region 直接播放動畫，並判斷重播 - 混合動畫型
        /// <summary>
        /// 直接播放指定動畫 - 混合動畫型
        /// 說明:播放該動畫，並判斷是否重播
        /// </summary>
        /// <param name="anim">參數1:Animator組件</param>
        /// <param name="_animName">參數2:欲播放動畫名稱</param>
        /// <param name="fps">參數3:是否重播</param>
        /// <param name="Reset">參數3:是否重播，預設為關</param>
        public static void Play(Animator anim, string _animName, bool Reset, float CrossTimed)
        {
            //動畫狀態
            AnimatorStateInfo animState;

            //動畫狀態擷取
            animState = anim.GetCurrentAnimatorStateInfo(0);

            //如果尚未處於該動畫狀態
            if (animState.IsName(_animName) == false)
            {
                //播放動畫
                anim.CrossFadeInFixedTime(_animName, CrossTimed);
            }
            //當該動畫播完時
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= (anim.GetCurrentAnimatorStateInfo(0).length / anim.GetCurrentAnimatorStateInfo(0).length))
            {
                //判斷是否重播
                if (Reset == true)
                    //重新播放動畫
                    anim.CrossFadeInFixedTime(_animName, CrossTimed);

                //否則只播放一次
                else if (animState.IsName(_animName) == false)
                    anim.CrossFadeInFixedTime(_animName, CrossTimed);
            }
        }
        #endregion

        #region 現在動畫狀態判斷
        /// <summary>
        /// 動畫狀態判斷
        /// 判斷「現在播放動畫名稱」使否與「輸入的動畫名稱」狀態相同
        /// </summary>
        /// <param name="anim">參數1:Animator組件</param>
        /// <param name="animInfo">參數2:輸入的動畫名稱</param>
        public static bool AnimStateJudgment(Animator anim, string _animName)
        {
            //動畫狀態
            AnimatorStateInfo animState;

            //動畫狀態擷取
            animState = anim.GetCurrentAnimatorStateInfo(0);

            //動畫狀態判斷
            if (animState.IsName(_animName) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region 剩餘動畫時間回傳 - 無轉換(抓取當前動畫時間)
        /// <summary>
        /// 直接獲得時間 
        /// </summary>
        /// <param name="anim">動畫組件</param>
        /// <param name="InAdvanceOver">提前結束時間，非必要用預設即可</param>
        /// <returns></returns>
        public static IEnumerator WaitForAnimState(Animator anim, float InAdvanceOver = 0.03f)
        {
            yield return new WaitForSeconds(AnimTimedCalculation(anim, InAdvanceOver));
        }
        #endregion

        #region 剩餘動畫時間回傳 - 混合動畫(抓取下一個動畫)

        /// <summary>
        /// 獲得剩餘動畫時間 - 混合動畫播放
        /// </summary>
        /// <param name="anim">Animator組件</param>
        /// <param name="_animName">欲播放動畫名稱</param>
        /// <param name="CrossTimed">混合時間</param>
        /// <param name="InAdvanceOver">提前結束時間，非必要用預設即可</param>
        /// <returns></returns>
        public static IEnumerator WaitForAnimCrossFade(Animator anim, string _animName, float CrossTimed = 0.3f, float InAdvanceOver = 0.03f)
        {
            //播放指定混合動畫
            anim.CrossFadeInFixedTime(_animName, CrossTimed);
            yield return new WaitForSeconds(AnimTimedCalculation(anim, InAdvanceOver));
        }
        #endregion

        #region 當前動畫-剩餘時間獲得 - 無Coroutine
        /// <summary>
        /// 直接獲得「當前動畫」剩餘時間
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="_InAdvanceOver"></param>
        /// <returns></returns>
        public static float GetRemainingTime(Animator anim, float _InAdvanceOver = 0.03f) 
        {
            return AnimTimedCalculation(anim, _InAdvanceOver);
        }
        #endregion

        #region 下一個動畫-剩餘時間獲得 - 無Coroutine
        /// <summary>
        /// 直接獲得「下一個動畫」剩餘時間
        /// </summary>
        public static float GetNextTime(Animator anim, float _InAdvanceOver = 0.03f)
        {
            return AnimTimedCalculation(anim, _InAdvanceOver, true);
        }
        #endregion

        #region 特殊播放動畫 - Coroutine
        /// <summary>
        /// 當下一個動畫不存在時，播放指定動畫
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="animName"></param>
        /// <param name="CrossTimed"></param>
        /// <returns></returns>
        ///  其他說明：
        /// 1-Animator在更新動畫時不會立即刷新，而是在下一禎後刷新，當同時進行播放與擷取時，會擷取不到下一個動畫數值
        /// 2-修改Anim更新狀態後，在Update狀態下同一時間點使用播放與擷取，會出現動畫瞬間刷新閃爍BUG
        //  3-使用Coroutine將禎數延緩一禎後再更新並播放可解決閃爍BUG
        public static IEnumerator PlayCoroutine(Animator anim, string animName, float CrossTimed = 0.3f)
        {
            yield return new WaitForSeconds(0.07f);
            anim.Update(0f);
            if (anim.GetNextAnimatorStateInfo(0).normalizedTime == 0)
                anim.CrossFadeInFixedTime(animName, CrossTimed);
        }
        #endregion

        #region 剩餘動畫時間計算

        /// <summary>
        /// 動畫時間計算
        /// </summary>
        /// <param name="anim">動畫組件</param>
        /// <param name="_InAdvanceOver">提前時間</param>
        /// <param name="NextAnimGet">是否只截取下一個動畫狀態</param>
        /// <returns></returns>
        private static float AnimTimedCalculation(Animator anim, float _InAdvanceOver = 0.03f , bool NextAnimGet = false)
        {                        
            AnimatorStateInfo animState;
            
            anim.Update(0);
            animState = anim.GetNextAnimatorStateInfo(0);
            

            //當只擷取下一個動畫狀態為關閉時，判斷是否有無下一個動畫狀態
            if (!NextAnimGet)
            {
                //無下一個動畫狀態，擷取當前狀態
                if (animState.shortNameHash == 0)
                    animState = anim.GetCurrentAnimatorStateInfo(0);
            }            

            //經過幀數
            float nowFPS = animState.normalizedTime;

            //當循環過1次以上時，將經過幀數歸零
            if (nowFPS >= 1)
                nowFPS -= Mathf.Floor(nowFPS);

            float _timed = 0;

            //計算時間(秒)
            //PS：這邊有加提前結束，不然他會跑到完全歸0才跳，如果有Loop的動畫他會出現破綻
            _timed = (animState.length - _InAdvanceOver) - (nowFPS * animState.length);

            if (_timed >= 0)
                return _timed;
            else
                return 0;
        }
        #endregion
    }

    public static class ObjectSetScript
    {
        #region 遊戲物件設定
        /// <summary>
        /// 設定開關顯示物件
        /// </summary>
        /// <param name="chance">設置開關</param>
        public static void SetMethod(this GameObject _gameObject, bool chance)
        {
            _gameObject.SetActive(chance);
        }

        /// <summary>
        /// 設定開關顯示物件、物件到指定位置
        /// </summary>
        /// <param name="pos">設置位置</param>
        public static void SetMethod(this GameObject _gameObject, bool chance, Vector3 pos)
        {
            _gameObject.SetActive(chance);
            _gameObject.transform.localPosition = pos;
        }
        #endregion
    }
}