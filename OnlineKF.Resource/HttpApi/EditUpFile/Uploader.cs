using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Collections;
using OnlineKF.Utils;


namespace OnlineKF.Resource
{
    /// <summary>
    /// UEditor编辑器通用上传类
    /// </summary>
    public class Uploader
    {
        string state = "SUCCESS";

        string URL = "";
        string currentType = "";
        string uploadpath = "";
        string filename = "";
        string originalName = "";
        string imgInfo = "";
        HttpPostedFile uploadFile = null;
        string resourcePath = ConfigHelper.GetResourcePath()+"/";

        
       
        string[] filetype;
        string[] imgType;

        /**
      * 上传文件的主处理方法
      * @param HttpContext
      * @param string
      * @param  string[]
      *@param int
      * @return Hashtable
      */
        public Hashtable upFile(HttpContext cxt, string pathbase, int size)
        {
            pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            uploadpath = resourcePath + pathbase;//获取文件上传路径

            string imgTypeStr = OnlineKF.Utils.ConfigHelper.GetConfigString("imgType");
            string fileTypeStr = OnlineKF.Utils.ConfigHelper.GetConfigString("fileType");

            imgType = imgTypeStr.Split(',');

            var fileTypeNewStr = imgTypeStr + "," + fileTypeStr;
            filetype = fileTypeNewStr.Split(',');
             

            try
            {
                uploadFile = cxt.Request.Files[0];
                originalName = uploadFile.FileName;

                //目录创建
                createFolder();

                //格式验证
                if (checkType(filetype))
                {
                    state = "不允许的文件类型";
                }
                //大小验证
                if (checkSize(size))
                {
                    state = "文件大小超出网站限制";
                }
                //获取图片信息
                if (hasImage()) {
                    imgInfo = getImgInfo();
                }
                //保存图片
                if (state == "SUCCESS")
                {
                    filename = reName();
                    uploadFile.SaveAs(uploadpath + filename);
                    URL = pathbase + filename;
                }
            }
            catch (Exception e)
            {
                LogUtils.LogWriterToIISOut(e);
                state = "未知错误";
                URL = "";
            }
            return getUploadInfo();
        }

        /**
     * 上传涂鸦的主处理方法
      * @param HttpContext
      * @param string
      * @param  string[]
      *@param string
      * @return Hashtable
     */
        public Hashtable upScrawl(HttpContext cxt, string pathbase, string tmppath, string base64Data)
        {
            pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            uploadpath = resourcePath + pathbase;//获取文件上传路径
            FileStream fs = null;
            try
            {
                //创建目录
                createFolder();
                //生成图片
                filename = System.Guid.NewGuid() + ".png";
                fs = File.Create(uploadpath + filename);
                byte[] bytes = Convert.FromBase64String(base64Data);
                fs.Write(bytes, 0, bytes.Length);

                URL = pathbase + filename;
            }
            catch (Exception e)
            {
                state = "未知错误";
                URL = "";
            }
            finally
            {
                fs.Close();
                deleteFolder(cxt.Server.MapPath(tmppath));
            }
            return getUploadInfo();
        }

        /**
    * 获取文件信息
    * @param context
    * @param string
    * @return string
    */
        public string getOtherInfo(HttpContext cxt, string field)
        {
            string info = null;
            if (cxt.Request.Form[field] != null && !String.IsNullOrEmpty(cxt.Request.Form[field]))
            {
                info = field == "fileName" ? cxt.Request.Form[field].Split(',')[1] : cxt.Request.Form[field];
            }
            return info;
        }
        private bool hasImage()
        {
            return checkType(imgType) == false;
        }
        private string getImgInfo() {
            System.Drawing.Image image = System.Drawing.Image.FromStream(uploadFile.InputStream);
            string width = image.Width.ToString();
            string height = image.Height.ToString();
            image.Dispose();
            return width + "*" + height;
        }
        /**
         * 获取上传信息
         * @return Hashtable
         */
        private Hashtable getUploadInfo()
        {
            Hashtable infoList = new Hashtable();

            infoList.Add("state", state);
            infoList.Add("url", URL);
            infoList.Add("originalName", originalName);
            infoList.Add("imgInfo", imgInfo);
            try
            {
                infoList.Add("name", Path.GetFileName(URL));
                infoList.Add("size", uploadFile.ContentLength);
                infoList.Add("type", Path.GetExtension(originalName));
            }
            catch (Exception ex)
            {
                LogUtils.LogWriterToIISOut(ex);
                infoList.Add("name", "");
                infoList.Add("size", "");
                infoList.Add("type", "");
            }

            return infoList;
        }

        /**
         * 重命名文件
         * @return string
         */
        private string reName()
        {
            return System.Guid.NewGuid() + getFileExt();
        }

        /**
         * 文件类型检测
         * @return bool
         */
        public bool checkType(string[] filetype)
        {
            currentType = getFileExt();

            return Array.IndexOf(filetype, currentType) == -1;
        }

        /**
         * 文件大小检测
         * @param int
         * @return bool
         */
        private bool checkSize(int size)
        {
            return uploadFile.ContentLength >= (size * 1024 * 1024);
        }

        /**
         * 获取文件扩展名
         * @return string
         */
        private string getFileExt()
        {
            string[] temp = uploadFile.FileName.Split('.');
            return "." + temp[temp.Length - 1].ToLower();
        }

        /**
         * 按照日期自动创建存储文件夹
         */
        private void createFolder()
        {
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }
        }

        /**
         * 删除存储文件夹
         * @param string
         */
        public void deleteFolder(string path)
        {
            //if (Directory.Exists(path))
            //{
            //    Directory.Delete(path, true);
            //}
        }
    }
}