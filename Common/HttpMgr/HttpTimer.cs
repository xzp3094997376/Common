using GTA;
using System;
using UnityEngine;

namespace Http
{
    public class HttpTimer
    {
        private TimerMgr timerMgr;
        private string timerName = "HttpTimer";

        /// <summary>
        /// 开始计时器
        /// </summary>
        public void StartTimer()
        {
            this.timerMgr = Framework.Instance.TimerMgr;
            Timer timer = null;
            try
            {
                int time = int.Parse(ConstantEConfig.GetValue(ConstantEnum.HttpHeartBeatRate));
                timer = this.timerMgr.CreateTimer(this.timerName, time);
            }
            catch (Exception)
            {
                Debug.LogError("格式化错误");
                return;
            }

            Action onOtherThread = () =>
            {
                Action onMainThread = () =>
                {
                    timer.OnEnd = this.OnTimerEnd;
                    timer.Start();
                };
                LoomUtil.QueueOnMainThread(onMainThread);
            };
            LoomUtil.RunAsync(onOtherThread);
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        public void StopTimer()
        {
            if (this.timerMgr != null)
            {
                this.timerMgr.KillTimer(this.timerName);
            }
        }

        /// <summary>
        /// 计时结束
        /// </summary>
        /// <param name="timer"></param>
        private void OnTimerEnd(Timer timer)
        {
            //心跳请求
            string data = "";
            Framework.Instance.HttpProtocolMgr.UserProtocol.OnHttpHeartBeat();
            timer.Start();
        }
    }
}