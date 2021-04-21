using System;

namespace StateMode._4
{
  class Program
  {
    static void Main(string[] args)
    {
      //紧急项目
      Work emergencyProjects = new Work();

      emergencyProjects.Hour = 9;
      emergencyProjects.WriteProgram();

      emergencyProjects.Hour = 10;
      emergencyProjects.WriteProgram();

      emergencyProjects.Hour = 12;
      emergencyProjects.WriteProgram();

      emergencyProjects.Hour = 13;
      emergencyProjects.WriteProgram();

      emergencyProjects.Hour = 14;
      emergencyProjects.WriteProgram();

      emergencyProjects.Hour = 17;
      emergencyProjects.WriteProgram();

      // emergencyProjects.TaskFinished = true;
      emergencyProjects.TaskFinished = false;

      emergencyProjects.WriteProgram();

      emergencyProjects.Hour = 19;
      emergencyProjects.WriteProgram();

      emergencyProjects.Hour = 22;
      emergencyProjects.WriteProgram();

      Console.Read();
    }
  }
}
