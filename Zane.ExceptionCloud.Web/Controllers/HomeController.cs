using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Zane.ExceptionCloud.Web.Controllers
{
    public class CustomResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member,MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            property.ShouldSerialize = instance =>
            {
                try
                {
                    PropertyInfo prop = (PropertyInfo)member;
                    if (prop.CanRead)
                    {
                        prop.GetValue(instance, null);
                        return true;
                    }
                }
                catch
                {
                }
                return false;
            };

            return property;
        }
    }
    public class MemoryStreamJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(MemoryStream).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var bytes = serializer.Deserialize<byte[]>(reader);
            return bytes != null ? new MemoryStream(bytes) : new MemoryStream();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bytes = ((MemoryStream)value).ToArray();
            serializer.Serialize(writer, bytes);
        }
    }
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content(ToJsonString(Request));
        }
        public string ToJsonString(object obj)
        {
            StringBuilder sb = new StringBuilder();
            JsonSerializer serializer = new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            //serializer.ContractResolver = new CustomResolver();
            serializer.Converters.Add(new MemoryStreamJsonConverter());
            serializer.Error += delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
            {
            
                args.ErrorContext.Handled = true;
            };
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, obj);
            return sb.ToString();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
