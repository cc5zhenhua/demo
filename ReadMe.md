#关于项目

项目基于asp.netcore 3.1,使用VisualStudion 2022开发， 提供了一个WebApi和swagger页面来获取读取存储在内存数据库中的数据。 

项目同时包含一个后台Service，从ZEISS的webSocket数据流持续获取数据并保存在本地数据库中。 

(注：首次启动项目由于后台获取WebSocket数据和前端WebApi异步进行，有可能出现WebApi拿不到数据的情况，请耐心等20秒左右等后台服务自动连接webScoket数据源并获取数据流之后再刷新一下)

#关于部署
工程工程目录有DockerFile可使用[Docker build](https://docs.docker.com/engine/reference/commandline/build/)命令来打包，关于命令的详细用法可参考对应连接。

#项目运行截图
 ![运行后截图](https://i.postimg.cc/kGh4vv3t/zeiss.jpg)

#工程细节
1. 本项目使用Sqlite数据库也测试通过
(Sqlite需要初始化Table Schema)
Startup.cs中
             services.AddDbContext<MachineLogMemoryDbContext>(opt=> opt.UseInMemoryDatabase(databaseName: "localDb"));
            //services.AddDbContext<MachineLogMemoryDbContext>(options =>
            //    options.UseSqlite("Filename=./messages.db"));

2. 内存数据库由于内存占用限制，后台接收数据流的时候对Message存储数量和Api返回数量都做了限制。