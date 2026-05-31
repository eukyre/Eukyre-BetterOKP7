using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;

namespace EukTestBed.Utilities;

[Injectable(typePriority: OnLoadOrder.PostDBModLoader + 3)]
public class BaseGameItemEdits(
    ISptLogger<BaseGameItemEdits> logger,
    DatabaseService databaseService
):IOnLoad
{
    public Task OnLoad()
    {
        EditFilters();
        return Task.CompletedTask;
    }

    private void EditFilters()
    {
        var dbItems = databaseService.GetItems();
        foreach (var (id, item) in dbItems)
        {
            switch (id)
            {
                case "570fd79bd2720bc7458b4583":
                    item.Properties.Prefab.Path = "scope_all_ekb_okp7.bundle";
                    List<double> aimSens = [0.65,0.65,0.65];
                    List<List<double>> aimSensList = new List<List<double>>();
                    aimSensList.Add(aimSens);
                    item.Properties.AimSensitivity = aimSensList;
                    List<double> zooms = [1, 1, 1];
                    List<List<double>> zoomlist = new List<List<double>>();
                    zoomlist.Add(zooms);
                    item.Properties.Zooms = zoomlist;
                    item.Properties.ModesCount = new int[] { 3 };
                    item.Properties.Name = "willowedit_scope_all_ekb_okp7";
                    break; //Normal OKP7 settings
                case "57486e672459770abd687134":
                    item.Properties.Prefab.Path = "scope_dovetail_ekb_okp7_dovetail.bundle";
                    List<double> aimSensDT = [0.65,0.65,0.65];
                    List<List<double>> aimSensListDT = new List<List<double>>();
                    aimSensListDT.Add(aimSensDT);
                    item.Properties.AimSensitivity = aimSensListDT;
                    List<double> dovetailZooms = [1, 1, 1];
                    List<List<double>> dovetailZoomlist = new List<List<double>>();
                    dovetailZoomlist.Add(dovetailZooms);
                    item.Properties.Zooms = dovetailZoomlist;
                    item.Properties.ModesCount = new int[] { 3 };
                    item.Properties.Name = "willowedit_scope_dovetail_ekb_okp7_dovetail";
                    break; //Dovetail OKP7 settings
            }
        }
    }
    
    private void ReplaceSlotFilters(TemplateItem item, int slotIndex, int filterIndex, HashSet<MongoId> ids)
    {
        var slot = GetSlotAtIndex(item, slotIndex);
        var filter = GetSlotFilterAtIndex(slot, filterIndex);

        filter.Filter = ids;
    }

    private void ModifySlotFilters(TemplateItem item, int slotIndex, int filterIndex, List<MongoId> ids, bool isCartridge = false)
    {
        var slot = GetSlotAtIndex(item, slotIndex, isCartridge);
        var filter = GetSlotFilterAtIndex(slot, filterIndex);

        filter.Filter!.UnionWith(ids);
    }
    
    private Slot GetSlotAtIndex(TemplateItem item, int index, bool isCartridge = false)
    {
        var slots = isCartridge ? item.Properties?.Cartridges?.ToArray() : item.Properties?.Slots?.ToArray();

        if (index >= 0 && index < slots?.Length)
        {
            return slots[index];
        }

        throw new IndexOutOfRangeException($"Index on item slot property `{item.Name}` is out of range");
    }

    private SlotFilter GetSlotFilterAtIndex(Slot slot, int index)
    {  
        var slotFilter = slot.Properties?.Filters?.ToArray() ?? [];

        if (index >= 0 && index < slotFilter.Length)
        {
            return slotFilter[index];
        }

        throw new IndexOutOfRangeException($"Index on slot property `{slot.Name}` is out of range");
    }
}