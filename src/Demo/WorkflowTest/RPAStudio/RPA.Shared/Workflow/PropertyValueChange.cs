using System;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Workflow
{
    public class PropertyValueChange : Change
    {
        public ModelItem ActivityToChange { get; set; }

        public override string Description
        {
            get
            {
                return "改变活动属性";
            }
        }

        public object NewValue { get; set; }

        public string PropertyName { get; set; }

        private object OldValue { get; set; }

        public override bool Apply()
        {
            if (this.ActivityToChange.IsFlowStep())
            {
                this.ActivityToChange = this.ActivityToChange.Properties["Action"].Value;
            }
            this.OldValue = this.ActivityToChange.Properties[this.PropertyName].ComputedValue;
            this.ActivityToChange.Properties[this.PropertyName].SetValue(this.NewValue);
            return true;
        }

        public override Change GetInverse()
        {
            return new PropertyValueChange
            {
                NewValue = this.OldValue,
                PropertyName = this.PropertyName,
                ActivityToChange = this.ActivityToChange
            };
        }
    }
}
