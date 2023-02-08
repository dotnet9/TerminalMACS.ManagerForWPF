using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Tracking;

namespace TrackingParticipantSample
{
    /// <summary>
    /// 自定义用户活动
    /// </summary>
    public sealed class UserActivity : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            Console.WriteLine("用户活动开始执行");

            CustomTrackingRecord customRecord = new CustomTrackingRecord("User");
            var data = customRecord.Data;

            data["姓名"] = "张三";
            data["年龄"] = 23;

            //触发自定义跟踪记录
            Console.WriteLine("用户活动触发自定义跟踪记录");
            context.Track(customRecord);

            Console.WriteLine("用户活动结束执行");
        }
    }
}
