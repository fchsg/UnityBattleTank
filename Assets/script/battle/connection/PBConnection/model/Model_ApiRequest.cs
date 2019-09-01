using UnityEngine;
using System.Collections;

using SlgPB;

public class Model_ApiRequest {
	
	public ApiRequest api;
	
	public Model_ApiRequest()
	{
		api = new ApiRequest ();
		api.v = "1.00.00";      //版本号，x.xx.xx
		api.callId = "" + TimeHelper.GetCurrentRealTimestamp(); //发起请求时的毫秒数，服务端以此为依据避免同样请求的重复提交
		api.sign = "";   //数据签名，各接口按照约定顺序把关键数据拼串后生成，用于数据校验
		api.ticket = InstancePlayer.instance.ticket; //登录接口不需要此票。登录后的接口请求需要验票。如果用户不是使用注册帐号登录，则该数据等于device_id
		api.s = 0;       //接口请求来源, 0.production 1.dev
		api.snid = 0;    //接口请求平台id
	}
}
