using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using WTTServerCommonLib.Models;
using Range = SemanticVersioning.Range;

namespace EukyreBetterOKP7;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.eukyre.betterokp7";
    public override string Name { get; init; } = "Eukyre-BetterOKP7";
    public override string Author { get; init; } = "GrooveypenguinX, probablyEukyre";
    public override List<string>? Contributors { get; init; } = null;
    public override SemanticVersioning.Version Version { get; init; } = new(typeof(ModMetadata).Assembly.GetName().Version?.ToString(3));
    public override Range SptVersion { get; init; } = new("~4.0.13");
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, Range>? ModDependencies { get; init; } = new()
    {
        { "com.wtt.commonlib", new Range("~2.0.20") }
    };
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; } = true;
    public override string License { get; init; } = "MIT";
}


[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
public class EukyreBetterOKP7(
    WTTServerCommonLib.WTTServerCommonLib wttCommon) : IOnLoad
{
    public async Task OnLoad()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        await wttCommon.CustomLocaleService.CreateCustomLocales(assembly);
        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);
        await Task.CompletedTask;
    }
}
