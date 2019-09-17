using Newtonsoft.Json;
using System;

namespace SandboxCSharp.Generic
{
    class AGeneric
    {
        public static void ExecuteWithType()
        {
            string s = AGeneric.ExecuteJsonCallAction<object>(() =>
            {
                return new string("Hello");
            }, x => new BaseResponse
            {
                Data = new { UpdateProducts = true, UpdateCart = true },
                StateInformation = x.ToString()
            });
            Console.WriteLine(s);
        }

        public static void ExecuteWithNoType()
        {
            string sNoGeneric = AGeneric.ExecuteJsonCallAction(() =>
            {
                return new string("NoGeneric");
            }, x => new BaseResponse
            {
                Data = x,
                StateInformation = "MaisVideInfo"
            });
            Console.WriteLine(sNoGeneric);
        }

        /// <summary>
        ///     Wrapper für Calls mit JSON-Response
        /// </summary>
        /// <typeparam name="T">Rückgabetype der gewrappten Methode</typeparam>
        /// <param name="action">Aufruf für gewrappte Methode</param>
        /// <param name="mappingFunction">Mappingfuntion von Rückgabewert auf JSON-Response (optional)</param>
        /// <returns>Instanz von JsonResult</returns>
        private static string ExecuteJsonCallAction<T>(Func<T> action, Func<T, BaseResponse> mappingFunction = null)
        {
            try
            {
                var result = action();
                BaseResponse response;
                if (mappingFunction == null)
                {
                    response = new BaseResponse { Data = result };
                }
                else
                {
                    response = mappingFunction(result);
                }
                return JsonConvert.SerializeObject(response);

            }
            catch (Exception ex)
            {
                var response = new BaseResponse();
                return JsonConvert.SerializeObject(response);
            }
        }


        public class BaseResponse
        {
            public BaseResponse()
            {
            }

            public string StateInformation { get; set; }

            public object Data { get; set; }

            public override string ToString()
            {
                return $"{GetType().Name}: StateInfo:'{StateInformation}' data:'{Data.ToString()}'";
            }
        }
    }
}
