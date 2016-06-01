if (typeof LIM == "undefined")
    window.LIM = {};

LIM.config = {
    "msgid": 0,
    "isSend": false,
    "readSpeed": 1200,
    "companyID": 1,
    "companyName": "七乐游戏",
    "companyInfo": "郴州七乐网络有限公司，国内最专业的棋牌游戏开发商，是一家专业从事网络棋牌游戏软件开发、制定、销售及运营支持的软件开发公司。本公司核心技术人员具有多年的专业棋牌开发经验，可根据客户需求制定游戏产品和服务。公司经过多年以来积累的棋牌开发经验及技术实力,平台稳定性极佳,开发速度在国内堪称一流！",
    "baseChatClient": "/chatClient", "skin": "userColor", "city": "",
    "questionId": "",
    sendPostUrl: "/req.ashx?act=MessageDataAjax.newQuestion",
    sendType: { txt: 0, img: 1, fileType: 2 },
    lastTime: 0,
    isFirstAdd: true,
    mesBubble: "2",
    sendKjj:"CE", //定义快捷发送的方式
    "fileUploadPath": "/UI/UploadFile.html",
    filePostPath: "http://localhost:49840/HttpApi/EditUpFile/FileUpLoader.ashx",
    resourcePath: "http://localhost:49840/resource/",
    imgAvg:0.8,
    isFirstReady:true,
    reloadType:0
    
};