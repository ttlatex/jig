﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.IO
{
    public class SevenZip
    {
        #region メンバ変数
        /// <summary>
        /// 初期化の際にコンフィグセクションを読み込んだかどうか
        /// </summary>
        public bool IsReadSection { private set; get; }

        #endregion

        /// <summary>
        /// 生成部 "SevenZipSettings"セクションを読み込みます
        /// </summary>
        public SevenZip()
            : this("SevenZipSettings")
        {
        }

        /// <summary>
        /// コンフィグセクション指定読み込み
        /// </summary>
        /// <param name="sectionName"></param>
        public SevenZip(string sectionName)
        {
            // コンフィグセクション読込
            var settings = ConfigurationManager.GetSection(sectionName) as NameValueCollection; // NamevalueConfigCollectionでは？
            if (settings == null) throw new Exception(sectionName + "セクションの読み込みに失敗しました");
            this.IsReadSection = true;

            // 7z実行パス設定

        }
    }
}
