using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using System.Text.RegularExpressions;

namespace OutputModelPath
{
    /// <summary>
    /// 读取xlsx文件
    /// </summary>
    public class DataImport
    {

        public List<string> modelPathList = new List<string>();
        private List<CatalogMsg> carPartlist;
        public void ClearData()
        {
            modelPathList.Clear();
        }
        public List<CatalogMsg> GetCataList()
        {
            return carPartlist;
        }
        public void ReadFile(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.Extension.ToLower() == ".xlsx")
            {
                carPartlist = CarProXmL.GetCarXmlListForBin(fi.Name.Replace(fi.Extension, ""));
            }
            else
            {
                Debug.Log("DataImport.ReadFile():不支持的文件" + fi.FullName);
            }
        }

        public void SaveModelPathToLocal(string outputPath)
        {
            int length = carPartlist.Count;

            for (int i = 0; i < length; i++)
            {
                CatalogMsg catalogMsg = carPartlist[i];
                if ((int)catalogMsg.menuID <= 1 || string.IsNullOrEmpty(catalogMsg.SourceName)) continue;
                modelPath = null;
                GetSingleModelPath(catalogMsg, true);
            }

            WriteDataToLocal(outputPath);
        }

        /// <summary>
        /// 写模型路径信息到本地去
        /// </summary>
        /// <param name="path"></param>
        void WriteDataToLocal(string path)
        {
            string fileName = "AllModel.json";
            string filePath = Path.Combine(path, fileName).Replace("\\", "/");
            if (File.Exists(filePath)) File.Delete(filePath);
            //StreamWriter sw = File.CreateText(filePath);
            //List<JsonData>
            FileInfo fi = new FileInfo(filePath);
            File.WriteAllLines(filePath, modelPathList.ToArray(), Encoding.UTF8);
            //JsonMapper.ToJson(modelPathList, new JsonWriter(sw) { PrettyPrint = true });//使用带缩进的打印
            //sw.Flush();
            //sw.Close();

            Debug.Log("数据输出完成");
        }

        string modelPath;
        /// <summary>
        /// 获得单个模型路径的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="firstCall"></param>
        void GetSingleModelPath(CatalogMsg data, bool firstCall)
        {
            if ((int)data.menuID != 0)
            {
                if (firstCall) modelPath = data.text;
                else
                {
                    modelPath = data.text + "#" + modelPath;
                }
                CatalogMsg tempCatalogMsg = FindFIDBySID(data.SID);
                if (tempCatalogMsg == null) { Debug.Log("没有找到对应FID的数据  " + modelPath);  return; }
                GetSingleModelPath(tempCatalogMsg, false);
            }
            else
            {
                modelPath = data.text + "#" + modelPath;
                modelPathList.Add(modelPath);
                //modelPathList.Add(Regex.Unescape(modelPath));
            }
        }

        CatalogMsg FindFIDBySID(string SID)
        {
            CatalogMsg catalogMsg = null;
            int length = carPartlist.Count;
            for (int i = 0; i < length; i++)
            {
                if (carPartlist[i].FID == SID)
                {
                    return carPartlist[i];
                }
            }
            return catalogMsg;
        }
    }
}
