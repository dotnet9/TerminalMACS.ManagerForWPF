//using System;

//namespace TheMementoPattern._1
//{
//	class Program
//	{
//		static void Main(string[] args)
//		{
//			// 大战Boss前
//			GameRole lixiaoyao = new GameRole();

//			// 大战Boss前，获得初始角色状态
//			lixiaoyao.GetInitState();
//			lixiaoyao.StateDisplay();

//			//保存进度
//			GameRole backup = new GameRole();

//			// 通过‘游戏角色’的新实例，来保存进度
//			backup.Vitality = lixiaoyao.Vitality;
//			backup.Attack = lixiaoyao.Attack;
//			backup.Defense = lixiaoyao.Defense;

//			//大战Boss时，损耗严重
//			lixiaoyao.Fight();

//			// 大战Boss 时, 损耗严重所有数据全部损耗为零
//			lixiaoyao.StateDisplay();

//			//恢复之前状态,GameOver不甘心,恢复之前进度，重新来玩
//			lixiaoyao.Vitality = backup.Vitality;
//			lixiaoyao.Attack = backup.Attack;
//			lixiaoyao.Defense = backup.Defense;
//			lixiaoyao.StateDisplay();

//			Console.Read();
//		}
//	}
//}

