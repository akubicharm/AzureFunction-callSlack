#r "Newtonsoft.Json"

using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    log.LogInformation("Input Data: " + requestBody);

    var arr = JArray.Parse(requestBody);
    var slack_text = "温度異常発生::";
    foreach (var item in arr) {
        slack_text += item["time"] + " " + item["temp"] + ",";
    }

    log.LogInformation("Msg to Slack: " + slack_text);

    var wc = new WebClient();

    var WEBHOOK_URL =
"https://hooks.slack.com/services/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"; //incoming hookのURL
    var data = JsonConvert.SerializeObject(new
    {
        text = slack_text,
        icon_emoji = ":ghost:", //アイコンを動的に変更する
        username = "テストBot",  //名前を動的に変更する
        link_names = "1"  //メンションを有効にする
    });
    log.LogInformation("json=" + data);
    wc.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
    wc.Encoding = Encoding.UTF8;
    wc.UploadString(WEBHOOK_URL, data);

    return (ActionResult)new OkObjectResult($"Hello");
}
