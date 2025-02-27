using System.Globalization;
using ChatEx.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace ChatEx.ViewModels
{
    public class TranslateViewModel
    {
        private readonly IChatApi _chatApi;
        public TranslateViewModel(IChatApi chatApi)
        {
            _chatApi = chatApi;
        }

        /// <summary>
        /// 原始文本
        /// </summary>
        public string OriginalContext { get; set; } = string.Empty;

        /// <summary>
        /// 目标文本
        /// </summary>
        public string ResultContent { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;    

        public List<string> ChooseLan()
        {
            // 获取所有文化信息
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            // 使用 HashSet 去重（基于基础语言代码）
            var uniqueLanguages = new HashSet<string>();

            // 设置当前线程的文化为中文（简体）
            var currentCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = new CultureInfo("zh-CN");

            foreach (var culture in cultures)
            {
                try
                {
                    // 获取基础语言代码（例如 "en" 表示英语）
                    var baseLanguageCode = culture.TwoLetterISOLanguageName;

                    // 获取该语言的中文名称
                    var chineseName = new CultureInfo(baseLanguageCode).DisplayName;

                    // 添加到 HashSet 中
                    uniqueLanguages.Add(chineseName);
                }
                catch (CultureNotFoundException)
                {
                    // 忽略无效的文化信息
                    continue;
                }
            }

            // 恢复原始文化
            CultureInfo.CurrentCulture = currentCulture;

            // 转换为列表并排序
            var sortedLanguages = uniqueLanguages.ToList();

            sortedLanguages = sortedLanguages.Where(x => IsChinese(x)).ToList();

            sortedLanguages.Remove("中文");
            sortedLanguages.Add("中文(简体)");
            sortedLanguages.Add("中文(繁体)");
            sortedLanguages.Add("中文(文言文)");
            sortedLanguages.Add("中文()");
            sortedLanguages.Sort();

            return sortedLanguages;
        }

        private bool IsChinese(string str)
        {
            foreach (char c in str)
            {
                // 检查字符是否在中文 Unicode 编码范围内 
                if ((c >= '\u4e00' && c <= '\u9fa5'))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public async Task TranslateAsync()
        {
            ResultContent = "你好" + OriginalContext;

        }
    }
}
