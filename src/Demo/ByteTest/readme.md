# C#百万对象序列化与网络传输实践

| 日期       | 更新内容               | 版本  | 作者         |
| ---------- | ---------------------- | ----- | ------------ |
| 2023-12-16 | 初建，添加网络对象定义 | 0.0.1 | 沙漠尽头的狼 |
|            |                        |       |              |
|            |                        |       |              |

[TOC]

## 1. 背景

完善文章《[C#百万对象序列化深度剖析：如何在网络传输中实现速度与体积的完美平衡 (dotnet9.com)](https://dotnet9.com/2023/12/deep-analysis-of-csharp-million-object-serialization-how-to-achieve-a-perfect-balance-between-speed-and-volume-in-network-transm)》，以客户端实时获取服务端进程信息为测试案例：

1. 添加TCP通信，客户端通过命令方式向服务端请求数据，服务端可被动响应，也可主动推送；
2. 添加UDP通信，服务端可组播数据；
3. WPF百万数据DataGrid加载、实时更新。

## 2. 功能

### 2.1. 网络对象

#### 2.1.1. 数据包格式

**数据包=头部+数据**

数据包头部

| 字段名        | 数据类型 | 说明         |
| ------------- | -------- | ------------ |
| PacketSize    | int      | 数据包总大小 |
| SystemId      | long     | 系统Id       |
| ObjectId      | byte     | 对象Id       |
| ObjectVersion | byte     | 对象版本     |

数据包数据部分

使用MessagePack对对象进行二进制压缩，数据包会比常规二进制序列化小，以下是目前使用到的对象列表：

| 对象名              | 对象Id | 对象版本 | 说明                         |
| ------------------- | ------ | -------- | ---------------------------- |
| RequestBaseInfo     | 1      | 1        | 请求服务基本信息             |
| ResponseBaseInfo    | 2      | 1        | 响应服务基本信息             |
| RequestProcess      | 3      | 1        | 请求进程                     |
| ResponseProcess     | 4      | 1        | 响应进程                     |
| UpdateProcess       | 5      | 1        | 更新进程                     |
| UpdateActiveProcess | 6      | 1        | 更新进程常变化数据           |
| ChangeProcess       | 7      | 1        | 进程结构变化：增加、减少进程 |
|                     |        |          |                              |
| Heartbeat           | 255    | 1        | TCP心跳包                    |

#### 2.1.2. 网络对象定义

##### RequestBaseInfo

| 字段名 | 数据类型 | 说明   |
| ------ | -------- | ------ |
| TaskId | int      | 任务Id |

##### ResponseBaseInfo

| 字段名              | 数据类型 | 说明                           |
| ------------------- | -------- | ------------------------------ |
| TaskId              | int      | 任务Id                         |
| OperatingSystemType | string?  | 服务器操作系统类型             |
| MemorySize          | int      | 系统内存大小（单位MB）         |
| ProcessorCount      | int      | 处理器个数                     |
| TotalDiskSpace      | int      | 硬盘总容量（单位GB）           |
| NetworkBandwidth    | int      | 网络带宽（单位Mbps）           |
| IpAddress           | string?  | 服务器IP地址                   |
| ServerName          | string?  | 服务器名称                     |
| DataCenterLocation  | string?  | 数据中心位置                   |
| IsRunning           | byte     | 运行状态，0：未运行，1：已运行 |
| LastUpdateTime      | long     | 最后更新时间                   |

##### RequestProcess

| 字段名 | 数据类型 | 说明   |
| ------ | -------- | ------ |
| TaskId | int      | 任务Id |

##### ResponseProcess

| 字段名    | 数据类型         | 说明       |
| --------- | ---------------- | ---------- |
| TaskId    | int              | 任务Id     |
| TotalSize | int              | 总数据大小 |
| PageSize  | int              | 分页大小   |
| PageCount | int              | 总页数     |
| PageIndex | int              | 页索引     |
| Processes | `List<Process>?` | 进程列表   |

##### Process

| 字段名          | 数据类型 | 说明             |
| --------------- | -------- | ---------------- |
| PID             | int      | 进程ID           |
| Name            | string?  | 进程名称         |
| Type            | byte     | 进程类型         |
| Status          | byte     | 进程状态         |
| Publisher       | string?  | 发布者           |
| CommandLine     | string?  | 命令行           |
| CPUUsage        | double   | CPU使用率        |
| MemoryUsage     | double   | 内存使用率       |
| DiskUsage       | double   | 磁盘使用大小     |
| NetworkUsage    | double   | 网络使用值       |
| GPU             | double   | GPU              |
| GPUEngine       | string?  | GPU引擎          |
| PowerUsage      | byte     | 电源使用情况     |
| PowerUsageTrend | byte     | 电源使用情况趋势 |
| LastUpdateTime  | long     | 上次更新时间     |
| UpdateTime      | long     | 更新时间         |

##### UpdateProcess

| 字段名    | 数据类型         | 说明     |
| --------- | ---------------- | -------- |
| Processes | `List<Process>?` | 进程列表 |

##### UpdateActiveProcess

| 字段名    | 数据类型               | 说明     |
| --------- | ---------------------- | -------- |
| Processes | `List<ActiveProcess>?` | 进程列表 |

##### ActiveProcess

| 字段名          | 数据类型 | 说明             |
| --------------- | -------- | ---------------- |
| PID             | int      | 进程ID           |
| CPUUsage        | double   | CPU使用率        |
| MemoryUsage     | double   | 内存使用率       |
| DiskUsage       | double   | 磁盘             |
| NetworkUsage    | double   | 网络使用值       |
| GPU             | double   | GPU              |
| PowerUsage      | byte     | 电源使用情况     |
| PowerUsageTrend | byte     | 电源使用情况趋势 |
| UpdateTime      | long     | 更新时间         |

##### ChangeProcess

| 字段名 | 数据类型 | 说明 |
| ------ | -------- | ---- |
|        |          |      |

##### Heartbeat

| 字段名 | 数据类型 | 说明 |
| ------ | -------- | ---- |
|        |          |      |