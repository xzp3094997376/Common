public class HttpMgr
    {
        static HTTPRequest DownLoadRequest;

        /// <summary>
        /// 上传数据后不使用返回数据,上传格式Json字符串
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <param name="successCallback"></param>
        /// <param name="failureCallback"></param>
        public static void PostData(string path, string data, Action successCallback = null, Action failureCallback = null)
        {
            HTTPRequest request;
            request = new HTTPRequest(new Uri(path), HTTPMethods.Post, (HTTPRequest originalRequest, HTTPResponse response) =>
            {
                if (ReqExceptHandler(originalRequest))
                {
                    failureCallback?.Invoke();
                    return;
                }
                else
                {
                    successCallback?.Invoke();
                }
            });
            request.SetHeader("Content-Type", "application/json;charset=UTF-8");
            request.IsKeepAlive = false;
            request.RawData = Encoding.UTF8.GetBytes(data);
            request.Timeout = TimeSpan.FromSeconds(20f);
            request.ConnectTimeout = TimeSpan.FromSeconds(10f);
            request.Send();
        }

        /// <summary>
        /// 上传数据后返回数据，上传格式Json字符串
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <param name="successCallback"></param>
        /// <param name="failureCallback"></param>
        public static void PostDataReturnData(string path, string data, Action<string> successCallback, string token = null)
        {
            HTTPRequest request;
            //TODO:需要把该请求编号传进来，存到request里面
            request = new HTTPRequest(new Uri(path), HTTPMethods.Post, (HTTPRequest originalRequest, HTTPResponse response) =>
            {
                if (ReqExceptHandler(originalRequest))
                {
                  
                    // failureCallback?.Invoke();
                    return;
                }
                else
                {                  

                    Debug.Log("响应状态 ：" + response.IsSuccess);

                    successCallback?.Invoke(response.DataAsText);
                    //分割数据
                    //Framework.Instance.HttpMessageDispatcherMgr.SlitData(originalRequest, response);
                }
            });
            request.SetHeader("Content-Type", "application/json;charset=UTF-8");
            if (token != null)
                request.SetHeader("token", token);
            //if (token != null)
            //    request.AddField("token", token);
            request.IsKeepAlive = false;
            request.RawData = Encoding.UTF8.GetBytes(data);
            request.Timeout = TimeSpan.FromSeconds(10f);
            request.ConnectTimeout = TimeSpan.FromSeconds(2f);
            request.Send();
        }

        /// <summary>
        /// 上传数据后返回数据，上传格式Json字符串-带有tocken
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <param name="successCallback"></param>
        /// <param name="failureCallback"></param>
        public static void PostDataReturnData(string path, string data, string token = null)
        {
            HTTPRequest request;
            //TODO:需要把该请求编号传进来，存到request里面
            request = new HTTPRequest(new Uri(path), HTTPMethods.Post, (HTTPRequest originalRequest, HTTPResponse response) =>
            {
                if (ReqExceptHandler(originalRequest))
                {
                    // failureCallback?.Invoke();
                    return;
                }
                else
                {
                    // successCallback?.Invoke(response.DataAsText);
                    //分割数据
                   // Framework.Instance.HttpMessageDispatcherMgr.SlitData(originalRequest, response);
                }
            });
            request.SetHeader("Content-Type", "application/json;charset=UTF-8");
            if (token != null)
                request.AddField("token", token);
            request.IsKeepAlive = false;
            request.RawData = Encoding.UTF8.GetBytes(data);
            request.Timeout = TimeSpan.FromSeconds(10f);
            request.ConnectTimeout = TimeSpan.FromSeconds(2f);
            request.Send();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="successCallback">成功后回调函数返回String</param>
        /// <param name="failureCallback"></param>
        public static void GetData(string path, Action<string> successCallback, Action failureCallback = null)
        {
            var request = new HTTPRequest(new Uri(path), HTTPMethods.Get, (HTTPRequest originalRequest, HTTPResponse response) =>
            {
                if (ReqExceptHandler(originalRequest))
                {
                    failureCallback?.Invoke();
                    return;
                }
                else
                {
                    successCallback?.Invoke(response.DataAsText);
                }
            });
            request.IsKeepAlive = false;
            request.SetHeader("Content-Type", "application/json;charset=UTF-8");
            request.Timeout = TimeSpan.FromSeconds(10f);
            request.ConnectTimeout = TimeSpan.FromSeconds(2f);
            request.Send();
        }

        /// <summary>
        /// 下载大文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="savePath"></param>
        /// <param name="OnDownloadProgress"></param>
        public static void DownLoadBigFile(string path, string savePath, OnDownloadProgressDelegate OnDownloadProgress = null)
        {
            DownLoadRequest = new HTTPRequest(new Uri(path), (req, resp) =>
            {
                List<byte[]> fragments = resp.GetStreamedFragments();
                FileStream fs = new FileStream(savePath, FileMode.Append);
                foreach (byte[] data in fragments)
                {
                    fs.WriteAsync(data, 0, data.Length);
                }

                fs.Close();
                if (resp.IsStreamingFinished)
                    Debug.Log("Download finished!");
            });
            DownLoadRequest.IsKeepAlive = false;
            DownLoadRequest.OnProgress = OnDownloadProgress;
            DownLoadRequest.UseStreaming = true;
            DownLoadRequest.StreamFragmentSize = 1 * 1024 * 1024; // 1MB
#if !UNITY_WEBGL
            DownLoadRequest.DisableCache = true;
#endif
            DownLoadRequest.Send();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="savePath"></param>
        public static void DownLoadFile(string path, string savePath)
        {
            var request = new HTTPRequest(new Uri(path), (req, resp) =>
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath); //存在则删除
                }

                FileStream fs = new FileStream(savePath, FileMode.Append);
                fs.Write(resp.Data, 0, resp.Data.Length);
                fs.Close();
            });
            request.IsKeepAlive = false;
#if !UNITY_WEBGL
            request.DisableCache = true;
#endif
            request.Send();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filePath"></param>
        /// <param name="successCallback"></param>
        /// <param name="failureCallback"></param>
        public static void UploadFile(string path, string filePath, Action successCallback = null, Action failureCallback = null)
        {
            var request = new HTTPRequest(new Uri(path), HTTPMethods.Post, (req, resp) =>
            {
                if (ReqExceptHandler(req))
                {
                    Debug.LogError("上传失败");
                    failureCallback?.Invoke();
                    return;
                }
                else
                {
                    Debug.Log("上传成功" + resp.DataAsText);
                    successCallback?.Invoke();
                }
            });
            request.UploadStream = new FileStream(filePath, FileMode.Open);
            request.UseUploadStreamLength = false;
            request.Send();
        }

        /// <summary>
        /// 停止下载大文件
        /// </summary>
        public static void StopDownLoad()
        {
            if (DownLoadRequest != null && DownLoadRequest.State < HTTPRequestStates.Finished)
            {
                DownLoadRequest.OnProgress = null;
                DownLoadRequest.Callback = null;
                DownLoadRequest.Abort();
            }
        }

        /// <summary>
        /// 失败原因
        /// </summary>
        /// <param name="originalRequest"></param>
        /// <returns></returns>
        private static bool ReqExceptHandler(HTTPRequest originalRequest)
        {
            if (originalRequest.State == HTTPRequestStates.Error)
            {
                Debug.Log(originalRequest.Uri + "网络连接错误！");
                //EventDispatcher.SendEvent((int) HttpMessageID.HttpError, originalRequest.Uri + "网络连接错误！");
                return true;
            }

            if (originalRequest.State == HTTPRequestStates.ConnectionTimedOut)
            {
                Debug.Log(originalRequest.Uri + "网络连接超时！");
                //EventDispatcher.SendEvent((int)HttpMessageID.HttpError, originalRequest.Uri + "网络连接超时！");
                return true;
            }

            if (originalRequest.State == HTTPRequestStates.TimedOut)
            {
                Debug.Log(originalRequest.Uri + "网络请求处理超时！");
               // EventDispatcher.SendEvent((int)HttpMessageID.HttpError, originalRequest.Uri + "网络请求处理超时！");
                return true;
            }

            if (originalRequest.State == HTTPRequestStates.Aborted)
            {
                Debug.Log(originalRequest.Uri + "请求终止！");
                //EventDispatcher.SendEvent((int)HttpMessageID.HttpError, originalRequest.Uri + "请求终止！");
                return true;
            }

            return false;
        }
    }