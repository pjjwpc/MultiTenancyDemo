# 多租户Demo 
  基于MySql 实现的一个简单的多租户Demo ，算是多租户设计的一个演示
  技术栈：asp.net core 、EFCore、Apollo、Redis、Dapper。
  用到的Apollo配置中心（作用很简单，就是项目启动时加载数据库连接字符串，因为配置中心也要调研所以就直接用了），Redis发布订阅（有新的使用独立数据库租户创建则发布事件，客户端程序进行数据库初始化）
  
