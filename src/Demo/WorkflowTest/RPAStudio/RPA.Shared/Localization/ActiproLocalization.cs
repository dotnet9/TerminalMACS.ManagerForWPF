using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Localization
{
    public static class ActiproLocalization
    {
        public static void Init()
        {
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UIEditorSearchScopeDocumentText.ToString(), "文档");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UIEditorSearchScopeSelectionText.ToString(), "选择区域");

            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneCloseButtonToolTip.ToString(), "关闭 ({0})");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneFindAllButtonToolTip.ToString(), "查找全部");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneFindNextButtonToolTip.ToString(), "查找下一项 ({0})");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneFindWhatTextBoxPlaceholderText.ToString(), "查找…");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneMatchCaseButtonToolTip.ToString(), "区分大小写 ({0})");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneMatchWholeWordButtonToolTip.ToString(), "全字匹配 ({0})");

            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneReplaceAllButtonToolTip.ToString(), "全部替换 ({0})");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneReplaceNextButtonToolTip.ToString(), "替换下一项 ({0})");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneReplaceWithTextBoxPlaceholderText.ToString(), "替换为…");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneSearchUpButtonToolTip.ToString(), "向上搜索 ({0})");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneToggleReplaceButtonToolTip.ToString(), "切换替换模式");
            ActiproSoftware.Products.SyntaxEditor.SR.SetCustomString(ActiproSoftware.Products.SyntaxEditor.SRName.UISearchOverlayPaneUseRegularExpressionsButtonToolTip.ToString(), "使用正则表达式 ({0})");


            

            ActiproSoftware.Products.Shared.SR.SetCustomString(ActiproSoftware.Products.Shared.SRName.UICommandMinimizeWindowText.ToString(), "最小化");
            ActiproSoftware.Products.Shared.SR.SetCustomString(ActiproSoftware.Products.Shared.SRName.UICommandMaximizeWindowText.ToString(), "最大化");
            ActiproSoftware.Products.Shared.SR.SetCustomString(ActiproSoftware.Products.Shared.SRName.UICommandRestoreWindowText.ToString(), "还原");
            ActiproSoftware.Products.Shared.SR.SetCustomString(ActiproSoftware.Products.Shared.SRName.UICommandCloseWindowText.ToString(), "关闭");


            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExDockingWindowNoDockSiteRegistered‎.ToString(), "此操作无效，因为停靠窗口尚未在 DockSite 中注册。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExDockingWindowTargetNotLinked‎.ToString(), "此操作无效，因为停靠窗口只能移至已链接的 DockSite。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExDockSiteDockingWindowAlreadyRegistered‎.ToString(), "停靠窗口“{0}”已在另一个 DockSite 中注册。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExDockSiteNoMdiHost.ToString(), "无法将窗口置于 MDI 中，因为未将 MDI 主体(选项卡式 Mdi 主体、标准 Mdi 主体等)定位为 DockSite 工作区的直接子项。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExDockSiteNoTargetDockHost.ToString(), "无法打开窗口，因为未找到任何目标 DockHost。  如果尚未加载 DockSite，则会发生这种情况，因为其模板中注入了一个主 DockHost。  确保在加载 DockSite 之后进行所有的停靠操作。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExDockSiteRegistryAlreadyContainsDockingWindow‎.ToString(), "注册表中已包含具有唯一 ID“{0}”的停靠窗口。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExSplitContainerChildrenCollectionReset‎.ToString(), "拆分容器的子集合不允许重置。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExSplitContainerSlotResizeRatioMustBeGreaterThanZero‎.ToString(), "槽调整大小比例必须大于 0。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExToolWindowRequired‎.ToString(), "“{0}”实例类型必须是或继承工具窗口。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.ExWorkspaceCannotRemoveFromParent‎.ToString(), "无法从父项“{0}”中删除工作区。");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIAdvancedTabControlScrollBackwardButtonToolTip.ToString(), "向后滚动");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIAdvancedTabControlScrollForwardButtonToolTip‎.ToString(), "向前滚动");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIAdvancedTabItemCloseButtonToolTip.ToString(), "关闭");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIAdvancedTabItemKeepOpenButtonToolTip‎.ToString(), "保持打开");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIAdvancedTabItemToggleLayoutKindButtonToolTip‎.ToString(), "切换固定状态");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandActivateNextDocumentText.ToString(), "下一个文档");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandActivatePreviousDocumentText‎.ToString(), "上一个文档");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandActivatePrimaryDocumentText‎.ToString(), "激活主文档");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandCloseAllDocumentsText‎.ToString(), "关闭所有文档");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandCloseAllInContainerText‎.ToString(), "关闭选项卡组");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandCloseOthersText‎.ToString(), "关闭其他");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandClosePrimaryDocumentText‎.ToString(), "关闭主文档");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandCloseWindowText.ToString(), "关闭");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandKeepTabOpenText.ToString(), "保持选项卡打开");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMakeDockedWindowText‎.ToString(), "停靠");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMakeDocumentWindowText‎.ToString(), "停靠为文档");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMakeFloatingWindowText‎.ToString(), "浮动");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToNewHorizontalContainerText.ToString(), "新建水平选项卡组");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToNewVerticalContainerText.ToString(), "新建垂直选项卡组");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToNextContainerText.ToString(), "移至下一个选项卡组");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToPreviousContainerText‎.ToString(), "移至上一个选项卡组");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandMoveToPrimaryMdiHostText.ToString(), "移至主文档组");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandPinTabText‎.ToString(), "固定选项卡");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UICommandToggleWindowAutoHideStateText‎.ToString(), "自动隐藏");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIStandardSwitcherDocumentsText.ToString(), "活动文件");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIStandardSwitcherToolWindowsText‎.ToString(), "活动工具窗口");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIToolWindowContainerCloseButtonToolTip.ToString(), "关闭");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIToolWindowContainerMaximizeButtonToolTip.ToString(), "最大化");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIToolWindowContainerMinimizeButtonToolTip‎.ToString(), "最小化");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIToolWindowContainerOptionsButtonToolTip‎.ToString(), "选项");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIToolWindowContainerRestoreButtonToolTip.ToString(), "还原");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIToolWindowContainerToggleAutoHideButtonToolTip‎.ToString(), "自动隐藏");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIWindowControlCloseButtonToolTip‎.ToString(), "关闭");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIWindowControlMaximizeButtonToolTip‎.ToString(), "最大化");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIWindowControlMinimizeButtonToolTip‎.ToString(), "最小化");
            ActiproSoftware.Products.Docking.SR.SetCustomString(ActiproSoftware.Products.Docking.SRName.UIWindowControlRestoreButtonToolTip.ToString(), "还原");


            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIApplicationButtonLabelText‎.ToString(), "文件");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIApplicationButtonScreenTipHeaderText‎.ToString(), "应用程序按钮");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIColorPickerGalleryItemShadeDarkerToolTip.ToString(), "较暗");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIColorPickerGalleryItemShadeLighterToolTip‎.ToString(), "较亮");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemAddToQatText.ToString(), "添加至快速访问工具栏");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemCustomizeQatText‎.ToString(), "自定义快速访问工具栏");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemMinimizeRibbonText‎.ToString(), "最小化功能区");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemMoreCommandsText.ToString(), "更多命令…");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemRemoveFromQatText‎.ToString(), "从快速访问工具栏中删除");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemShowAboveRibbonText‎.ToString(), "在功能区上方显示");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemShowBelowRibbonText.ToString(), "在功能区下方显示");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemShowQatAboveRibbonText.ToString(), "在功能区上方显示快速访问工具栏");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeMenuItemShowQatBelowRibbonText.ToString(), "在功能区下方显示快速访问工具栏");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeQatApplicationMenuText‎.ToString(), "应用程序菜单");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeQatMenuHeaderText‎.ToString(), "自定义快速访问工具栏");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeQatSeparatorText‎.ToString(), "&lt;分隔符&gt;");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UICustomizeQatTabText.ToString(), "选项卡");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIPopupGalleryFilterAllText‎.ToString(), "全部");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIQuickAccessToolBarCustomizeButtonToolTip.ToString(), "自定义快速访问工具栏");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIQuickAccessToolBarOverflowButtonToolTip.ToString(), "更多控件");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRecentDocumentListHeaderText‎.ToString(), "最近的文档");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRecentDocumentPinButtonPinAutomationName‎.ToString(), "取消固定文档");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRecentDocumentPinButtonPinToolTip‎.ToString(), "将此文档固定至“最近的文档”列表中。");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRecentDocumentPinButtonUnpinAutomationName.ToString(), "固定文档");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRecentDocumentPinButtonUnpinToolTip‎.ToString(), "将此文档从“最近的文档”列表中取消固定。");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRibbonWindowTitleBarButtonCloseToolTip.ToString(), "关闭");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRibbonWindowTitleBarButtonMaximizeToolTip‎.ToString(), "最大化");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRibbonWindowTitleBarButtonMinimizeToolTip.ToString(), "最小化");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIRibbonWindowTitleBarButtonRestoreToolTip.ToString(), "还原");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIScreenTipPressF1ForMoreHelpText.ToString(), "有关更多帮助，请按 F1。");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIScreenTipToggleMinimizationDownDescription‎.ToString(), "显示功能区，使其始终处于展开状态，即使在单击命令之后亦是如此。");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIScreenTipToggleMinimizationDownHeader‎.ToString(), "展开功能区");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIScreenTipToggleMinimizationUpDescription.ToString(), "仅在功能区上显示选项卡名称。");
            ActiproSoftware.Products.Ribbon.SR.SetCustomString(ActiproSoftware.Products.Ribbon.SRName.UIScreenTipToggleMinimizationUpHeader‎.ToString(), "最小化功能区");


        }
    }
}

