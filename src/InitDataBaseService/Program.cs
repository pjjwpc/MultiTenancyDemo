using System;
using StackExchange.Redis;

namespace InitDataBaseService 
{
    class Program 
    {
        static string createGoods=@"CREATE TABLE `Goods` (
                                    `Id` int(11) NOT NULL AUTO_INCREMENT,
                                    `Name` varchar(200) DEFAULT NULL,
                                    `Price` double NOT NULL,
                                    `Image` longtext,
                                    `UserId` int(11) NOT NULL,
                                    `TenantId` int(11) NOT NULL,
                                    `Status` int(11) NOT NULL,
                                    `CreateTime` datetime(6) NOT NULL,
                                    `UpdateTime` datetime(6) NOT NULL,
                                    PRIMARY KEY (`Id`),
                                    KEY `IX_Goods_Name` (`Name`),
                                    KEY `IX_Goods_TenantId` (`TenantId`),
                                    KEY `IX_Goods_UserId` (`UserId`),
                                    CONSTRAINT `FK_Goods_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`id`) ON DELETE CASCADE
                                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8;";
        static string createUser=@"CREATE TABLE `User` (
                                    `Id` int(11) NOT NULL AUTO_INCREMENT,
                                    `Name` varchar(255) DEFAULT NULL,
                                    `Status` int(11) NOT NULL,
                                    `TenantId` int(11) NOT NULL,
                                    `CreateTime` datetime(6) NOT NULL,
                                    `UpdateTime` datetime(6) NOT NULL,
                                    PRIMARY KEY (`Id`),
                                    KEY `IX_User_Name` (`Name`)
                                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8;";

        static string createOrder=@"CREATE TABLE `Orders` (
                                    `Id` int(11) NOT NULL AUTO_INCREMENT,
                                    `UserId` int(11) NOT NULL,
                                    `TenantId` int(11) NOT NULL,
                                    `OrderDes` longtext,
                                    `CreateTime` datetime(6) NOT NULL,
                                    `UpdateTime` datetime(6) NOT NULL,
                                    PRIMARY KEY (`Id`),
                                    KEY `IX_Orders_UserId` (`UserId`),
                                    CONSTRAINT `FK_Orders_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`id`) ON DELETE CASCADE
                                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8;";

        static void Main (string[] args) 
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect ("127.0.0.1:6379")) 
            {
                ISubscriber sub = redis.GetSubscriber();
                sub.Subscribe("createtenant", (channel, message) => {
                    Console.WriteLine($"收到消息:{message}\n 开始创建表……");
                    using(MySql.Data.MySqlClient.MySqlConnection connection=new MySql.Data.MySqlClient.MySqlConnection(message))
                    {
                        Console.WriteLine("打开数据库连接");
                        connection.Open();
                        Console.WriteLine("数据库连接打开成功");
                        Console.WriteLine("开始创建用户表");
                        var cmd=connection.CreateCommand();
                        cmd.CommandText=createUser;
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("用户表创建成功\n 开始创建订单表");
                        cmd.CommandText=createOrder;
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("订单表创建成功\n 开始创建商品表");
                        cmd.CommandText=createGoods;
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("商品表创建成功");
                        cmd.Dispose();
                    }
                });
                Console.WriteLine("已订阅 messages");
                Console.ReadKey();
            }
        }
    }
}