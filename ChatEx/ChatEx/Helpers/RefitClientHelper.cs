using Refit;

namespace ChatEx.Helpers
{
    public class RefitClientHelper
    {
        //private static readonly IMediator _mediator;
        //private static readonly ILogger _logger;

        static RefitClientHelper()
        {
            //_mediator = ServiceLocator.Instance.GetRequiredService<IMediator>();
            //_logger = ServiceLocator.Instance.GetRequiredService<ILogger<RefitClientHelper>>();
        }

        #region 包裹函数

        /// <summary>
        /// 包裹RefitClient访问Web Api的函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<ReturnResult<T>> WrapAsync<T>(Task<T> func)
        {
            List<string> errors = new List<string>();
            Exception exception = null;

            try
            {
                //访问Web Api
                var result = await func;

                return new ReturnResult<T>(result);
            }
            catch (ValidationApiException ex)
            {
                exception = ex;

                //提取每一个参数验证错误，这是唯一有价值的信息
                errors.Add($"{ex.HttpMethod} {ex.Uri} 参数无效");
                foreach (var err in ex.Content.Errors)
                {
                    errors.Add($"{err.Key}: {string.Join(", ", err.Value)}");
                }
            }
            catch (ApiException ex)
            {
                //if (ex is null)
                if (ex.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    //服务端返回null对象，204 No Content，会进入这个判断分支，VS2022调试模式提示ex=null，实际上ex不为空
                    errors.Add("服务端返回空");
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    exception = ex;
                    errors.Add("您没有访问权限，请重新登录");
                }
                else
                {
                    exception = ex;

                    errors.Add($"{ex.HttpMethod} {ex.Uri} Api错误, {ex.Content}");

                    //如果是未授权错误，发布消息，触发网页客户端跳转到登录页面
                    //if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    //    _mediator.Publish(new UnauthorizedEvent(ex.HttpMethod, ex.RequestMessage.RequestUri));
                }
            }
            catch (Exception ex)
            {
                exception = ex;

                errors.Add($"网络错误, {ex.Message}");
            }

            //_logger.LogError(exception, $"访问WebApi出错, {errors}");
            //_logger.LogError($"访问WebApi出错, {errors}, {Environment.NewLine}{exception.StackTrace}");
            //Blazor WebAssembly项目会在浏览器控制台输出错误信息
            //Console.WriteLine($"{DateTimeOffset.Now}, 访问WebApi出错, {string.Join(", ", errors)}, {Environment.NewLine}{exception.StackTrace}");
            //等Net6发布改为声明式Log

            //网络安全临时整改方案，简化错误提示，后续需要重构Web Api返回数据结构，屏蔽技术错误信息，提示友好指示信息
            errors = new List<string>() { "处理您的请求时出错，请尝试重新操作，如有需要请联系管理员" };

            return ReturnResult<T>.ErrResult(errors);
        }

        #endregion

    }
}
