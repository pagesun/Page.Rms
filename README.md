# RmsUtils
Rms监控系统接入包

![](https://github.com/pagesun/RmsUtils/workflows/.NET%20Core/badge.svg)



## 配置RMS接口信息

```json
"Rms": {
    "ProjectName": "项目名称",
    "ProjectCode": "项目简码",
    "Key": "接口密钥",
    "RestoreUrl": "告警api地址",
    "NoticeUrl": "自动恢复api地址"
  }
```

## 注入RMS服务

```c#
//rms服务依赖于httpclient
services.AddHttpClient();
//注入rms
services.AddRms(Configuration);
```

## 接口调用

```c#
//触发告警
rms.NoticeAsync();
//自动恢复
rms.RestoreAsync();
```



