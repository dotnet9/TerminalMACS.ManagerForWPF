using System;

namespace TheMementoPattern._3
{
	class Program
	{
		static void Main(string[] args)
		{
			// 大战Boss前
			GameRole lixiaoyao = new GameRole();

			// 游戏角色初始状态，三项指标数据都是100
			lixiaoyao.GetInitState();
			lixiaoyao.StateDisplay();

			// 保存进度
			RoleStateCaretaker stateAdmin = new RoleStateCaretaker();

			// 保存进度时，由于封装在Memento中，因此我们并不知道保存了哪些具体的角色数据
			stateAdmin.Memento = lixiaoyao.SaveState();
			
			// 大战Boss时，损耗严重
			// 开始大战Boss，三项指标数据都下降很多，非常糟糕，GameOver了
			lixiaoyao.Fight();
			lixiaoyao.StateDisplay();


			// 恢复之前状态
			// 不行，恢复保存的状态，重新来过
			lixiaoyao.RecoveryState(stateAdmin.Memento);
			lixiaoyao.StateDisplay();

			Console.Read();
		}
	}
}
