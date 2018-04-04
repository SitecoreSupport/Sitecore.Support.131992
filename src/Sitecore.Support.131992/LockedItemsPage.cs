using ComponentArt.Web.UI;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Controls;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Extensions;
using Sitecore.Globalization;
using Sitecore.Web.UI.Grids;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XamlSharp.Ajax;
using Sitecore.Web.UI.XamlSharp.Xaml;
using System;
using System.Linq;

namespace Sitecore.Support.Shell.Applications.WebEdit.Dialogs.LockedItems
{
    public class LockedItemsPage : Sitecore.Shell.Applications.WebEdit.Dialogs.LockedItems.LockedItemsPage
    {
        protected class LockedItem : SearchResultItem
        {
            [IndexField("__lock")]
            internal string LockedBy
            {
                get;
                set;
            }

            [IndexField("_language")]
            internal string LanguageName
            {
                get;
                set;
            }

            [IndexField("_latestversion")]
            internal bool LatestVersion
            {
                get;
                set;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.CanRunApplication("/sitecore/content/Applications/Content Editor/Ribbons/Chunks/Locks/My Items");
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!XamlControl.AjaxScriptManager.IsEvent)
            {
                Item[] array;
                using (IProviderSearchContext providerSearchContext = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext(0))
                {
                    array = (from result in providerSearchContext.GetQueryable<LockedItemsPage.LockedItem>()
                             where result.LockedBy == Sitecore.Context.User.Name.Replace("\\", string.Empty) && result.LanguageName == Sitecore.Context.Language.Name && result.LatestVersion
                             select Client.ContentDatabase.GetItem(result.ItemId)).ToArray<Item>();
                }
                ComponentArtGridHandler<Item>.Manage(this.Items, new GridSource<Item>(array), true);
                this.Items.GroupingNotificationText = Translate.Text("To group your items by column, drag and drop the column here.");
                CommonExtensions.LocalizeGrid(this.Items);
            }
        }

        protected override void ExecuteAjaxCommand(AjaxCommandEventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            if (e.Name == "lockeditems:refresh")
            {
                SheerResponse.Eval("Items.callback()");
                return;
            }
            base.ExecuteAjaxCommand(e);
        }
    }
}
