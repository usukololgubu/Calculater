using System;
using System.Windows.Automation;

namespace Calculater
{
    internal static class AutomationElementExtensions
    {
        public static AutomationElement FindFirstChildren(this AutomationElement automationElement, Condition condition)
        {
            return automationElement.FindFirst(TreeScope.Children, condition);
        }

        public static AutomationElement FindLastChildren(this AutomationElement automationElement, Condition condition)
        {
            var all = automationElement.FindAll(TreeScope.Children, condition);
            return all.Count > 0 ? all[all.Count - 1] : null;
        }

        public static AutomationElement FindFirstChildrenById(this AutomationElement automationElement, string automationId)
        {
            return automationElement.FindFirstChildren(
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationId)
            );
        }

        public static AutomationElement FindFirstChildrenByClassName(this AutomationElement automationElement, string className)
        {
            return automationElement.FindFirstChildren(
                new PropertyCondition(AutomationElement.ClassNameProperty, className)
            );
        }

        public static void Click(this AutomationElement element)
        {
            var button = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            if (button == null) throw new ArgumentException($"{nameof(element)} must be InvokePattern object");
            button.Invoke();
        }
    }
}