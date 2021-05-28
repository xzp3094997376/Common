using System;
using System.Collections.Generic;
using BestHTTP;
using UnityEngine;
using Google.Protobuf;
using GTA.Message;
using Http;

namespace GTA
{
    public struct HttpMessageArgs
    {
        public int messageID;//消息id
        public string dataStr;//消息
    }
    /// <summary>
    /// 消息控制器
    /// </summary>
    public class HttpMessageDispatcherMgr
    {
        private Queue<HttpMessageArgs> messageQueue = new Queue<HttpMessageArgs>();
        private bool isEnableHandler;//是否启用处理
        /// <summary>
        /// 禁启用消息处理
        /// </summary>
        /// <param name="isEnable">是否启用</param>
        public void EnableMessageHandler(bool isEnable)
        {
            if (isEnable)
            {
                if (this.isEnableHandler)
                {
                    return;
                }
                this.isEnableHandler = true;

                //注册信息派发事件
                Singleton<HttpMessageHandler>.Instance.RegisterEvent();

                Framework.Instance.onUpdateEvent += this.OnUpdate;
            }
            else
            {
                if (this.isEnableHandler == false)
                {
                    return;
                }
                this.isEnableHandler = false;

                //注销信息派发事件
                Singleton<HttpMessageHandler>.Instance.UnRegisterEvent();

                Framework.Instance.onUpdateEvent -= this.OnUpdate;
            }
        }
        /// <summary>
        /// 添加消息到队列
        /// </summary>
        /// <param name="args">消息体</param>
        public void AddMessage(HttpMessageArgs args)
        {
            this.messageQueue.Enqueue(args);
        }
        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public byte[] MergeData(MessageID id, IMessage message)
        {
            byte[] idByte = BitConverter.GetBytes((int)id);
            //序列化protobuf
            byte[] data = ProtobufTool.Serialize(message);
            byte[] newData = new byte[idByte.Length + data.Length];

            Array.Copy(idByte, newData, idByte.Length);
            Array.Copy(data, 0, newData, idByte.Length, data.Length);
            return newData;
        }
        /// <summary>
        /// 分割数据
        /// </summary>
        /// <param name="data"></param>
        public void SlitData(HTTPRequest originalRequest, HTTPResponse response)
        {

            // //获取消息类型
            // byte[] idByte = new byte[4];
            // Array.Copy(data, idByte, idByte.Length);
            // int messageID = BitConverter.ToInt32(idByte, 0);
            //
            // //获取消息
            // byte[] newData = new byte[data.Length - idByte.Length];
            // Array.Copy(data, idByte.Length, newData, 0, newData.Length);
            //
            //TODO:根据request里面的id来设置messageID
            HttpMessageArgs args;
            args.messageID = 1000;
            args.dataStr = response.DataAsText;
            //添加消息到队列
            this.AddMessage(args);
        }

        private void OnUpdate()
        {
            if (this.messageQueue.Count > 0)
            {
                HttpMessageArgs args = this.messageQueue.Dequeue();
                //发送websocket事件
                EventDispatcher.SendEvent((int)EventID.HttpSystem, args);
            }
        }
    }
}
